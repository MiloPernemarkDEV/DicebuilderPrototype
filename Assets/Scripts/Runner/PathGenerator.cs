using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum StopType { Junction, Shop, Boss }

public class PathGenerator : MonoBehaviour
{
    [SerializeField] private PlayerLanes player;
    [SerializeField] private RunDirector director;
    [Tooltip("A plain Square sprite prefab, used for both floor and walls")]
    [SerializeField] private SpriteRenderer blockPrefab;

    [Header("Size")]
    [Tooltip("Length of one corridor piece")]
    [SerializeField] private float segmentLength = 5f;
    [Tooltip("Width of the walkable floor between the walls (3 lanes of 1.5 = 4.5, plus a little margin)")]
    [SerializeField] private float corridorWidth = 5f;
    [Tooltip("How thick each wall is. Thick walls fill the screen edges")]
    [SerializeField] private float wallThickness = 6f;

    [Header("Building")]
    [Tooltip("Keep building corridor until it reaches this far ahead of the player")]
    [SerializeField] private float buildAheadDistance = 30f;
    [Tooltip("Remove corridor pieces once they are this far from the player")]
    [SerializeField] private float despawnDistance = 35f;
    [Tooltip("How much corridor to build behind the player at the start")]
    [SerializeField] private float startBehindDistance = 10f;

    [Header("Junctions")]
    [Tooltip("How many corridor pieces to pre-build down each side of a junction")]
    [SerializeField] private int branchSegments = 2;

    [Header("Junction Arrows")]
    [Tooltip("Arrow sprite that points to the RIGHT. It gets rotated to point down each side path")]
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private float arrowScale = 1f;
    [Tooltip("How far from the middle of the junction the arrows sit, down each side path")]
    [SerializeField] private float arrowDistance = 3f;

    [Header("Shop")]
    [Tooltip("Placed beside the path where the player stops at the shop")]
    [SerializeField] private GameObject shopPrefab;
    [Tooltip("How far into the left wall the shop sits, measured from the corridor edge")]
    [SerializeField] private float shopSideOffset = 1.5f;

    [Header("Enemy (end of the left path)")]
    [Tooltip("Placed in the middle of the road at the end of the left path")]
    [SerializeField] private GameObject bossPrefab;
    [Tooltip("How far in front of the enemy the player stops")]
    [SerializeField] private float bossStopDistance = 4f;

    [Header("Colors")]
    [SerializeField] private Color floorColorA = new Color(0.42f, 0.30f, 0.33f);
    [SerializeField] private Color floorColorB = new Color(0.47f, 0.34f, 0.37f);
    [SerializeField] private Color wallColor = new Color(0.08f, 0.08f, 0.08f);

    private const int FloorSortingOrder = -10;
    private const int WallSortingOrder = -5;
    private const int ArrowSortingOrder = 5;

    // Where the next corridor piece starts (on the middle lane line) and which way it goes
    private Vector3 buildPoint;
    private Vector2 buildHeading;
    private bool useAlternateFloor;

    // What the path leads to next, and how many straight pieces until it
    private StopType nextStop;
    private int segmentsUntilStop;

    // Stop state
    private bool stopBuilt;        // the next stop (junction/shop/boss) has been built ahead
    private bool waitingAtStop;    // the player is stopped at it
    private Vector3 stopPoint;     // where the player stops

    // Shop: the corridor after the shop is built early so the road is visible while shopping
    private (StopType type, int pieces) afterShopStop;
    private int piecesBuiltPastShop;

    // Junction info
    private Vector3 junctionCenter;
    private Vector2 junctionRight;

    private readonly List<GameObject> segments = new List<GameObject>();
    private readonly List<GameObject> junctionArrows = new List<GameObject>();

    private void Start()
    {
        buildHeading = player.Heading;
        buildPoint = player.transform.position - (Vector3)(buildHeading * startBehindDistance);
        SetNextStop(director.GetFirstStop());
    }

    private void Update()
    {
        if (stopBuilt)
        {
            if (nextStop == StopType.Shop) BuildPastShop();
            else if (nextStop == StopType.Boss) BuildPastEnemy();
            HandleStop();
        }
        else
        {
            BuildAhead();
        }

        RemoveFarSegments();
    }

    // True if the next stop (junction, shop or boss) is closer than this distance. Spawners use this.
    public bool IsJunctionWithin(float distance)
    {
        return stopBuilt && Vector3.Distance(stopPoint, player.PathCenter) < distance;
    }

    // Called by the RunDirector when the player closes the shop
    public void ContinueFromShop()
    {
        if (!waitingAtStop || nextStop != StopType.Shop)
        {
            Debug.LogWarning($"ContinueFromShop ignored (waitingAtStop: {waitingAtStop}, nextStop: {nextStop})");
            return;
        }

        player.Resume();
        ClearStop();

        // Some of the corridor after the shop is already built, so count it
        SetNextStop(afterShopStop);
        segmentsUntilStop -= piecesBuiltPastShop;
    }

    // ---------- Stops ----------

    private void SetNextStop((StopType type, int pieces) stop)
    {
        nextStop = stop.type;
        segmentsUntilStop = stop.pieces;
    }

    private void ClearStop()
    {
        stopBuilt = false;
        waitingAtStop = false;
    }

    private void HandleStop()
    {
        if (!waitingAtStop)
        {
            // Stop the player once they reach the stop point
            Vector3 toStop = stopPoint - player.PathCenter;
            if (Vector2.Dot(toStop, player.Heading) <= 0f)
            {
                player.StopAt(stopPoint);
                waitingAtStop = true;
                OnArrived();
            }
            return;
        }

        if (nextStop == StopType.Junction)
        {
            HandleJunctionInput();
        }
    }

    private void OnArrived()
    {
        switch (nextStop)
        {
            case StopType.Junction:
                foreach (GameObject arrow in junctionArrows) arrow.SetActive(true);
                director.OnReachedJunction();
                break;
            case StopType.Shop:
                director.OnReachedShop();
                break;
            case StopType.Boss:
                director.OnReachedBoss();
                break;
        }
    }

    private void HandleJunctionInput()
    {
        // Can't choose while looking at the map
        if (director.IsMapOpen) return;

        Keyboard kb = Keyboard.current;
        if (kb == null) return;

        if (kb.qKey.wasPressedThisFrame) ChooseDirection(false);
        else if (kb.eKey.wasPressedThisFrame) ChooseDirection(true);
    }

    private void ChooseDirection(bool wentRight)
    {
        Vector2 direction = wentRight ? junctionRight : -junctionRight;
        player.Turn(junctionCenter, direction);

        // Continue building from the end of the pre-built branch in the chosen direction
        buildHeading = direction;
        buildPoint = junctionCenter + (Vector3)(direction * (corridorWidth * 0.5f + branchSegments * segmentLength));

        foreach (GameObject arrow in junctionArrows) Destroy(arrow);
        junctionArrows.Clear();

        director.OnLeftJunction();
        ClearStop();
        SetNextStop(director.GetStopAfterJunction(wentRight));
    }

    // ---------- Building ----------

    private void BuildAhead()
    {
        while (!stopBuilt && Vector3.Distance(buildPoint, player.PathCenter) < buildAheadDistance)
        {
            if (segmentsUntilStop <= 0)
            {
                BuildStop();
            }
            else
            {
                BuildStraight();
                segmentsUntilStop--;
            }
        }
    }

    private void BuildStop()
    {
        switch (nextStop)
        {
            case StopType.Junction: BuildJunction(); break;
            case StopType.Shop: BuildShop(); break;
            case StopType.Boss: BuildBoss(); break;
        }
        stopBuilt = true;
    }

    private GameObject BuildStraight()
    {
        Vector2 right = RightOf(buildHeading);
        Quaternion rotation = RotationFor(buildHeading);
        Vector3 center = buildPoint + (Vector3)(buildHeading * segmentLength * 0.5f);

        GameObject segment = CreateSegment("Segment", center);

        // Floor (alternating shades so you can see the movement)
        CreateBlock(segment.transform, center, rotation, new Vector2(corridorWidth, segmentLength), NextFloorColor(), FloorSortingOrder);

        // Walls on both sides
        float wallOffset = corridorWidth * 0.5f + wallThickness * 0.5f;
        Vector2 wallSize = new Vector2(wallThickness, segmentLength);
        CreateBlock(segment.transform, center - (Vector3)(right * wallOffset), rotation, wallSize, wallColor, WallSortingOrder);
        CreateBlock(segment.transform, center + (Vector3)(right * wallOffset), rotation, wallSize, wallColor, WallSortingOrder);

        buildPoint += (Vector3)(buildHeading * segmentLength);
        return segment;
    }

    // A T-junction: open to the left and right, wall straight ahead
    private void BuildJunction()
    {
        Vector2 right = RightOf(buildHeading);
        Quaternion rotation = RotationFor(buildHeading);

        junctionCenter = buildPoint + (Vector3)(buildHeading * corridorWidth * 0.5f);
        junctionRight = right;
        stopPoint = junctionCenter;

        GameObject segment = CreateSegment("Junction", junctionCenter);

        // Square floor in the middle
        CreateBlock(segment.transform, junctionCenter, rotation, new Vector2(corridorWidth, corridorWidth), NextFloorColor(), FloorSortingOrder);

        // Wall straight ahead, wide enough to cover the corners
        Vector3 frontWallPos = junctionCenter + (Vector3)(buildHeading * (corridorWidth * 0.5f + wallThickness * 0.5f));
        Vector2 frontWallSize = new Vector2(corridorWidth + wallThickness * 2f, wallThickness);
        CreateBlock(segment.transform, frontWallPos, rotation, frontWallSize, wallColor, WallSortingOrder);

        // Pre-build a short corridor down each side so both paths are visible
        BuildBranch(-right);
        BuildBranch(right);

        // Arrows on the floor pointing down each side path (hidden until the player stops)
        CreateArrow(segment.transform, -right);
        CreateArrow(segment.transform, right);
    }

    private void BuildBranch(Vector2 direction)
    {
        buildHeading = direction;
        buildPoint = junctionCenter + (Vector3)(direction * corridorWidth * 0.5f);

        for (int i = 0; i < branchSegments; i++)
        {
            BuildStraight();
        }
    }

    // A normal corridor piece with the shop beside it. The player stops in the middle of it.
    private void BuildShop()
    {
        Vector2 right = RightOf(buildHeading);
        stopPoint = buildPoint + (Vector3)(buildHeading * segmentLength * 0.5f);

        GameObject segment = BuildStraight();

        if (shopPrefab != null)
        {
            Vector3 shopPos = stopPoint - (Vector3)(right * (corridorWidth * 0.5f + shopSideOffset));
            Instantiate(shopPrefab, shopPos, Quaternion.identity, segment.transform);
        }

        afterShopStop = director.GetStopAfterShop();
        piecesBuiltPastShop = 0;
    }

    // Keeps building the straight corridor after the shop (up to the next junction's worth)
    private void BuildPastShop()
    {
        while (piecesBuiltPastShop < afterShopStop.pieces &&
               Vector3.Distance(buildPoint, player.PathCenter) < buildAheadDistance)
        {
            BuildStraight();
            piecesBuiltPastShop++;
        }
    }

    // The enemy stands in the middle of the road. The player stops a bit in front of it.
    // (Called "Boss" in the code, it's the enemy at the end of the left path.)
    private void BuildBoss()
    {
        Vector3 enemyPos = buildPoint + (Vector3)(buildHeading * segmentLength * 1.5f);

        BuildStraight();
        GameObject segment = BuildStraight();

        if (bossPrefab != null)
        {
            Instantiate(bossPrefab, enemyPos, Quaternion.identity, segment.transform);
        }

        stopPoint = enemyPos - (Vector3)(buildHeading * bossStopDistance);
    }

    // Keeps the road going past the enemy so it doesn't look like a dead end
    private void BuildPastEnemy()
    {
        while (Vector3.Distance(buildPoint, player.PathCenter) < buildAheadDistance)
        {
            BuildStraight();
        }
    }

    // ---------- Helpers ----------

    private void CreateArrow(Transform parent, Vector2 direction)
    {
        if (arrowSprite == null) return;

        GameObject arrow = new GameObject("JunctionArrow");
        arrow.transform.SetParent(parent);
        arrow.transform.position = junctionCenter + (Vector3)(direction * arrowDistance);
        // The sprite points right (+X), so rotate +X to face the path direction
        arrow.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        arrow.transform.localScale = Vector3.one * arrowScale;

        SpriteRenderer sr = arrow.AddComponent<SpriteRenderer>();
        sr.sprite = arrowSprite;
        sr.sortingOrder = ArrowSortingOrder;

        arrow.SetActive(false);
        junctionArrows.Add(arrow);
    }

    private GameObject CreateSegment(string segmentName, Vector3 position)
    {
        GameObject segment = new GameObject(segmentName);
        segment.transform.SetParent(transform);
        segment.transform.position = position;
        segments.Add(segment);
        return segment;
    }

    private void CreateBlock(Transform parent, Vector3 position, Quaternion rotation, Vector2 size, Color color, int sortingOrder)
    {
        SpriteRenderer block = Instantiate(blockPrefab, position, rotation, parent);
        block.transform.localScale = new Vector3(size.x, size.y, 1f);
        block.color = color;
        block.sortingOrder = sortingOrder;
    }

    private Color NextFloorColor()
    {
        Color color = useAlternateFloor ? floorColorB : floorColorA;
        useAlternateFloor = !useAlternateFloor;
        return color;
    }

    // Removes corridor pieces that are far away AND not ahead of the player.
    // (Pieces ahead are never removed, even if they were built a little past the despawn distance.)
    private void RemoveFarSegments()
    {
        for (int i = segments.Count - 1; i >= 0; i--)
        {
            GameObject segment = segments[i];
            Vector3 toSegment = segment.transform.position - player.PathCenter;
            bool isAhead = Vector2.Dot(toSegment, player.Heading) > segmentLength;

            if (!isAhead && toSegment.magnitude > despawnDistance)
            {
                Destroy(segment);
                segments.RemoveAt(i);
            }
        }
    }

    private static Vector2 RightOf(Vector2 heading)
    {
        return new Vector2(heading.y, -heading.x);
    }

    private static Quaternion RotationFor(Vector2 heading)
    {
        return Quaternion.Euler(0f, 0f, Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg - 90f);
    }
}