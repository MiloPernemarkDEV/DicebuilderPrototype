using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PathGenerator : MonoBehaviour
{
    [SerializeField] private PlayerLanes player;
    [Tooltip("A plain Square sprite prefab, used for both floor and walls")]
    [SerializeField] private SpriteRenderer blockPrefab;
    [Tooltip("Optional: UI shown while the player is choosing a direction at a junction")]
    [SerializeField] private GameObject junctionPrompt;

    [Header("Junction Arrows")]
    [Tooltip("Arrow sprite that points to the RIGHT. It gets rotated to point down each side path")]
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private float arrowScale = 1f;
    [Tooltip("How far from the middle of the junction the arrows sit, down each side path")]
    [SerializeField] private float arrowDistance = 3f;

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
    [Tooltip("How many straight corridor pieces between junctions")]
    [SerializeField] private int minSegmentsBetweenJunctions = 8;
    [SerializeField] private int maxSegmentsBetweenJunctions = 14;
    [Tooltip("How many corridor pieces to pre-build down each side of a junction")]
    [SerializeField] private int branchSegments = 2;

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
    private int segmentsUntilJunction;

    // Junction state
    private bool junctionPending;      // a junction has been built ahead
    private bool waitingForChoice;     // the player is stopped at it
    private Vector3 junctionCenter;
    private Vector2 junctionRight;

    private readonly List<GameObject> segments = new List<GameObject>();
    private readonly List<GameObject> junctionArrows = new List<GameObject>();

    private void Start()
    {
        buildHeading = player.Heading;
        buildPoint = player.transform.position - (Vector3)(buildHeading * startBehindDistance);
        segmentsUntilJunction = RollSegmentsUntilJunction();

        if (junctionPrompt != null) junctionPrompt.SetActive(false);
    }

    private void Update()
    {
        if (junctionPending)
        {
            HandleJunction();
        }
        else
        {
            BuildAhead();
        }

        RemoveFarSegments();
    }

    // True if there's an upcoming junction closer than this distance (spawners use this)
    public bool IsJunctionWithin(float distance)
    {
        return junctionPending && Vector3.Distance(junctionCenter, player.PathCenter) < distance;
    }

    private void BuildAhead()
    {
        while (!junctionPending && Vector3.Distance(buildPoint, player.PathCenter) < buildAheadDistance)
        {
            if (segmentsUntilJunction <= 0)
            {
                BuildJunction();
            }
            else
            {
                BuildStraight();
                segmentsUntilJunction--;
            }
        }
    }

    private void HandleJunction()
    {
        if (!waitingForChoice)
        {
            // Stop the player once they reach the middle of the junction
            Vector3 toCenter = junctionCenter - player.PathCenter;
            if (Vector2.Dot(toCenter, player.Heading) <= 0f)
            {
                player.StopAt(junctionCenter);
                waitingForChoice = true;
                if (junctionPrompt != null) junctionPrompt.SetActive(true);
                foreach (GameObject arrow in junctionArrows) arrow.SetActive(true);
            }
            return;
        }

        Keyboard kb = Keyboard.current;
        if (kb == null) return;

        if (kb.qKey.wasPressedThisFrame) ChooseDirection(-junctionRight); // left
        else if (kb.eKey.wasPressedThisFrame) ChooseDirection(junctionRight); // right
    }

    private void ChooseDirection(Vector2 direction)
    {
        player.Turn(junctionCenter, direction);

        // Continue building from the end of the pre-built branch in the chosen direction
        buildHeading = direction;
        buildPoint = junctionCenter + (Vector3)(direction * (corridorWidth * 0.5f + branchSegments * segmentLength));

        junctionPending = false;
        waitingForChoice = false;
        segmentsUntilJunction = RollSegmentsUntilJunction();
        if (junctionPrompt != null) junctionPrompt.SetActive(false);

        foreach (GameObject arrow in junctionArrows) Destroy(arrow);
        junctionArrows.Clear();
    }

    private void BuildStraight()
    {
        Vector2 right = new Vector2(buildHeading.y, -buildHeading.x);
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
    }

    // A T-junction: open to the left and right, wall straight ahead
    private void BuildJunction()
    {
        Vector2 right = new Vector2(buildHeading.y, -buildHeading.x);
        Quaternion rotation = RotationFor(buildHeading);

        junctionCenter = buildPoint + (Vector3)(buildHeading * corridorWidth * 0.5f);
        junctionRight = right;

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

        junctionPending = true;
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

    private void RemoveFarSegments()
    {
        for (int i = segments.Count - 1; i >= 0; i--)
        {
            GameObject segment = segments[i];
            if (Vector3.Distance(segment.transform.position, player.PathCenter) > despawnDistance)
            {
                Destroy(segment);
                segments.RemoveAt(i);
            }
        }
    }

    private int RollSegmentsUntilJunction()
    {
        return Random.Range(minSegmentsBetweenJunctions, maxSegmentsBetweenJunctions + 1);
    }

    private static Quaternion RotationFor(Vector2 heading)
    {
        return Quaternion.Euler(0f, 0f, Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg - 90f);
    }
}