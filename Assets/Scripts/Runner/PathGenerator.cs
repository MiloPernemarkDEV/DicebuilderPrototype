using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum StopType { JUNCTION, SHOP, BOSS }

[System.Serializable]
public class KitchenProp
{
    public Sprite sprite;
    [Tooltip("How tall it stands in world units (the player is about 2)")]
    public float height = 1.5f;
}

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

    [Header("Kitchen Props")]
    [Tooltip("Stoves, fridges, pots... placed randomly along the walls")]
    [SerializeField] private KitchenProp[] props;
    [Tooltip("Distance between prop spots along each wall")]
    [SerializeField] private float propSpacing = 2.5f;
    [Tooltip("Chance that a spot gets a prop")]
    [Range(0f, 1f)] [SerializeField] private float propChance = 0.6f;
    [Tooltip("How far from the road edge props stand on the wall at the top of the screen")]
    [SerializeField] private float propDistanceFar = 0.9f;
    [Tooltip("How far from the road edge props stand on the wall at the bottom of the screen (further, so tall props don't cover the road)")]
    [SerializeField] private float propDistanceNear = 2.6f;

    [Header("Top Wall Strip")]
    [Tooltip("A diagonal (isometric) counter picture placed along the wall at the TOP of the screen, one copy per road piece. Leave empty to turn off")]
    [SerializeField] private Sprite wallStrip;
    [Tooltip("How much road one copy covers. Match the road piece length (5) so copies join up")]
    [SerializeField] private float stripLength = 5f;
    [Tooltip("Stretch copies a little so they overlap and hide the joins (1 = no overlap)")]
    [SerializeField] private float stripOverlap = 1.08f;
    [Tooltip("How far from the road edge the strip stands")]
    [SerializeField] private float stripDistance = 0.8f;
    [Tooltip("Moves the strip up (+) or down (-) on screen so its counter front lines up with the road edge")]
    [SerializeField] private float stripRaise = 0.6f;

    [Header("Colors")]
    [SerializeField] private Color floorColorA = new Color(0.42f, 0.30f, 0.33f);
    [SerializeField] private Color floorColorB = new Color(0.47f, 0.34f, 0.37f);
    [Tooltip("Optional picture for the floor. It repeats (tiles) across the road. Floor Color A/B still tint it, so set them to white (or near white)")]
    [SerializeField] private Sprite floorSprite;
    [SerializeField] private Color wallColor = new Color(0.08f, 0.08f, 0.08f);
    [Tooltip("Optional picture for the walls. It repeats (tiles) across each wall. Set Wall Color to white to show it as-is")]
    [SerializeField] private Sprite wallSprite;

    private const int FloorSortingOrder = -10;
    private const int WallSortingOrder = -5;
    private const int ArrowSortingOrder = 5;
    private const int PropSortingOrder = 1;

    // Where the next corridor piece starts (on the middle lane line) and which way it goes
    private Vector3 _buildPoint;
    private Vector2 _buildHeading;
    private bool _useAlternateFloor;

    // What the path leads to next, and how many straight pieces until it
    private StopType _nextStop;
    private int _segmentsUntilStop;

    // Stop state
    private bool _stopBuilt;        // the next stop (junction/shop/boss) has been built ahead
    private bool _waitingAtStop;    // the player is stopped at it
    private Vector3 _stopPoint;     // where the player stops

    // Shop: the corridor after the shop is built early so the road is visible while shopping
    private (StopType type, int pieces) _afterShopStop;
    private int _piecesBuiltPastShop;

    // Junction info
    private Vector3 _junctionCenter;
    private Vector2 _junctionRight;

    private readonly List<GameObject> _segments = new List<GameObject>();
    private readonly List<GameObject> _junctionArrows = new List<GameObject>();

    private float _viewAngle;

    private void Start()
    {
        CameraFollow follow = Camera.main != null ? Camera.main.GetComponent<CameraFollow>() : null;
        _viewAngle = follow != null ? follow.ViewAngle : 0f;

        _buildHeading = player.Heading;
        _buildPoint = player.transform.position - (Vector3)(_buildHeading * startBehindDistance);
        SetNextStop(director.GetFirstStop());
    }

    private void Update()
    {
        if (_stopBuilt)
        {
            if (_nextStop == StopType.SHOP) BuildPastShop();
            else if (_nextStop == StopType.BOSS) BuildPastEnemy();
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
        return _stopBuilt && Vector3.Distance(_stopPoint, player.PathCenter) < distance;
    }

    // Called by the RunDirector when the player closes the shop
    public void ContinueFromShop()
    {
        if (!_waitingAtStop || _nextStop != StopType.SHOP)
        {
            Debug.LogWarning($"ContinueFromShop ignored (_waitingAtStop: {_waitingAtStop}, _nextStop: {_nextStop})");
            return;
        }

        player.Resume();
        ClearStop();

        // Some of the corridor after the shop is already built, so count it
        SetNextStop(_afterShopStop);
        _segmentsUntilStop -= _piecesBuiltPastShop;
    }

    // ---------- Stops ----------

    private void SetNextStop((StopType type, int pieces) stop)
    {
        _nextStop = stop.type;
        _segmentsUntilStop = stop.pieces;
    }

    private void ClearStop()
    {
        _stopBuilt = false;
        _waitingAtStop = false;
    }

    private void HandleStop()
    {
        if (!_waitingAtStop)
        {
            // Stop the player once they reach the stop point
            Vector3 toStop = _stopPoint - player.PathCenter;
            if (Vector2.Dot(toStop, player.Heading) <= 0f)
            {
                player.StopAt(_stopPoint);
                _waitingAtStop = true;
                OnArrived();
            }
            return;
        }

        if (_nextStop == StopType.JUNCTION)
        {
            HandleJunctionInput();
        }
    }

    private void OnArrived()
    {
        switch (_nextStop)
        {
            case StopType.JUNCTION:
                foreach (GameObject arrow in _junctionArrows) arrow.SetActive(true);
                director.OnReachedJunction();
                break;
            case StopType.SHOP:
                director.OnReachedShop();
                break;
            case StopType.BOSS:
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
        Vector2 direction = wentRight ? _junctionRight : -_junctionRight;
        player.Turn(_junctionCenter, direction);

        // Continue building from the end of the pre-built branch in the chosen direction
        _buildHeading = direction;
        _buildPoint = _junctionCenter + (Vector3)(direction * (corridorWidth * 0.5f + branchSegments * segmentLength));

        foreach (GameObject arrow in _junctionArrows) Destroy(arrow);
        _junctionArrows.Clear();

        director.OnLeftJunction();
        ClearStop();
        SetNextStop(director.GetStopAfterJunction(wentRight));
    }

    // ---------- Building ----------

    private void BuildAhead()
    {
        while (!_stopBuilt && Vector3.Distance(_buildPoint, player.PathCenter) < buildAheadDistance)
        {
            if (_segmentsUntilStop <= 0)
            {
                BuildStop();
            }
            else
            {
                BuildStraight();
                _segmentsUntilStop--;
            }
        }
    }

    private void BuildStop()
    {
        switch (_nextStop)
        {
            case StopType.JUNCTION: BuildJunction(); break;
            case StopType.SHOP: BuildShop(); break;
            case StopType.BOSS: BuildBoss(); break;
        }
        _stopBuilt = true;
    }

    private GameObject BuildStraight(bool withProps = true)
    {
        Vector2 right = RightOf(_buildHeading);
        Quaternion rotation = RotationFor(_buildHeading);
        Vector3 center = _buildPoint + (Vector3)(_buildHeading * (segmentLength * 0.5f));

        GameObject segment = CreateSegment("Segment", center);

        // Floor (alternating shades so you can see the movement)
        CreateBlock(segment.transform, center, rotation, new Vector2(corridorWidth, segmentLength), NextFloorColor(), FloorSortingOrder, floorSprite);

        // Walls on both sides
        float wallOffset = corridorWidth * 0.5f + wallThickness * 0.5f;
        Vector2 wallSize = new Vector2(wallThickness, segmentLength);
        CreateBlock(segment.transform, center - (Vector3)(right * wallOffset), rotation, wallSize, wallColor, WallSortingOrder, wallSprite);
        // The wall picture's counter side is on its right edge; mirror the right wall so its counter also faces the road
        CreateBlock(segment.transform, center + (Vector3)(right * wallOffset), rotation, wallSize, wallColor, WallSortingOrder, wallSprite, true);

        if (withProps)
        {
            PlaceProps(segment.transform, center, right);
            PlaceWallStrip(segment.transform, center, right);
        }

        _buildPoint += (Vector3)(_buildHeading * segmentLength);
        return segment;
    }

    // A T-junction: open to the left and right, wall straight ahead
    private void BuildJunction()
    {
        Vector2 right = RightOf(_buildHeading);
        Quaternion rotation = RotationFor(_buildHeading);

        _junctionCenter = _buildPoint + (Vector3)(_buildHeading * (corridorWidth * 0.5f));
        _junctionRight = right;
        _stopPoint = _junctionCenter;

        GameObject segment = CreateSegment("Junction", _junctionCenter);

        // Square floor in the middle
        CreateBlock(segment.transform, _junctionCenter, rotation, new Vector2(corridorWidth, corridorWidth), NextFloorColor(), FloorSortingOrder, floorSprite);

        // Wall straight ahead, wide enough to cover the corners
        Vector3 frontWallPos = _junctionCenter + (Vector3)(_buildHeading * (corridorWidth * 0.5f + wallThickness * 0.5f));
        // Turned sideways so the wall picture's counter side faces back toward the junction
        Vector2 frontWallSize = new Vector2(wallThickness, corridorWidth + wallThickness * 2f);
        CreateBlock(segment.transform, frontWallPos, RotationFor(right), frontWallSize, wallColor, WallSortingOrder, wallSprite);

        // Pre-build a short corridor down each side so both paths are visible
        BuildBranch(-right);
        BuildBranch(right);

        // Arrows on the floor pointing down each side path (hidden until the player stops)
        CreateArrow(segment.transform, -right);
        CreateArrow(segment.transform, right);
    }

    private void BuildBranch(Vector2 direction)
    {
        _buildHeading = direction;
        _buildPoint = _junctionCenter + (Vector3)(direction * (corridorWidth * 0.5f));

        for (int i = 0; i < branchSegments; i++)
        {
            BuildStraight();
        }
    }

    // A normal corridor piece with the shop beside it. The player stops in the middle of it.
    private void BuildShop()
    {
        Vector2 right = RightOf(_buildHeading);
        _stopPoint = _buildPoint + (Vector3)(_buildHeading * (segmentLength * 0.5f));

        GameObject segment = BuildStraight(false);   // no props here, the shop stands on this wall

        if (shopPrefab != null)
        {
            Vector3 shopPos = _stopPoint - (Vector3)(right * (corridorWidth * 0.5f + shopSideOffset));
            Instantiate(shopPrefab, shopPos, Quaternion.identity, segment.transform);
        }

        _afterShopStop = director.GetStopAfterShop();
        _piecesBuiltPastShop = 0;
    }

    // Keeps building the straight corridor after the shop (up to the next junction's worth)
    private void BuildPastShop()
    {
        while (_piecesBuiltPastShop < _afterShopStop.pieces &&
               Vector3.Distance(_buildPoint, player.PathCenter) < buildAheadDistance)
        {
            BuildStraight();
            _piecesBuiltPastShop++;
        }
    }

    // The enemy stands in the middle of the road. The player stops a bit in front of it.
    // (Called "Boss" in the code, it's the enemy at the end of the left path.)
    private void BuildBoss()
    {
        Vector3 enemyPos = _buildPoint + (Vector3)(_buildHeading * (segmentLength * 1.5f));

        BuildStraight();
        GameObject segment = BuildStraight();

        if (bossPrefab != null)
        {
            Instantiate(bossPrefab, enemyPos, Quaternion.identity, segment.transform);
        }

        _stopPoint = enemyPos - (Vector3)(_buildHeading * bossStopDistance);
    }

    // Keeps the road going past the enemy so it doesn't look like a dead end
    private void BuildPastEnemy()
    {
        while (Vector3.Distance(_buildPoint, player.PathCenter) < buildAheadDistance)
        {
            BuildStraight();
        }
    }

    // ---------- Kitchen props ----------

    private void PlaceProps(Transform parent, Vector3 segmentCenter, Vector2 right)
    {
        if (props == null || props.Length == 0 || propSpacing <= 0f) return;

        // Which way is "up" on screen for this stretch of road (camera is turned by the view angle)
        Vector2 screenUp = Rotate(_buildHeading, _viewAngle);
        Vector2 left = -right;
        // The wall on the left of the road is at the top of the screen when the view angle is positive
        Vector2 farSide = _viewAngle >= 0f ? left : right;

        foreach (Vector2 side in new[] { left, right })
        {
            float distance = side == farSide ? propDistanceFar : propDistanceNear;

            for (float along = -segmentLength * 0.5f + propSpacing * 0.5f; along < segmentLength * 0.5f; along += propSpacing)
            {
                if (Random.value > propChance) continue;

                KitchenProp prop = props[Random.Range(0, props.Length)];
                if (prop == null || prop.sprite == null) continue;

                Vector3 anchor = segmentCenter
                                 + (Vector3)(_buildHeading * along)
                                 + (Vector3)(side * (corridorWidth * 0.5f + distance));
                CreateProp(parent, prop, anchor, screenUp);
            }
        }
    }

    // One copy of the diagonal counter picture along the wall at the top of the screen
    private void PlaceWallStrip(Transform parent, Vector3 segmentCenter, Vector2 right)
    {
        if (wallStrip == null) return;

        Vector2 screenUp = Rotate(_buildHeading, _viewAngle);
        Vector2 farSide = _viewAngle >= 0f ? -right : right;

        // The road runs at (90 - _viewAngle) degrees across the screen. Scale the picture so its
        // width covers exactly one road piece measured along that diagonal.
        float roadScreenAngle = (90f - _viewAngle) * Mathf.Deg2Rad;
        float targetWidth = stripLength * Mathf.Cos(roadScreenAngle) * stripOverlap;
        float spriteWidth = wallStrip.bounds.size.x;
        float scale = spriteWidth > 0f ? targetWidth / spriteWidth : 1f;

        GameObject go = new GameObject("WallStrip");
        go.transform.SetParent(parent);
        go.transform.localScale = Vector3.one * scale;

        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = wallStrip;
        sr.sortingOrder = PropSortingOrder;

        Vector3 anchor = segmentCenter + (Vector3)(farSide * (corridorWidth * 0.5f + stripDistance));
        Vector3 position = anchor + (Vector3)(screenUp * stripRaise);
        // Copies further along the road (higher on screen) sit slightly further back, so each copy overlaps the next nicely
        position.z = Vector2.Dot(anchor, screenUp) * 0.001f;
        go.transform.position = position;

        go.AddComponent<FaceCamera>();
    }

    private void CreateProp(Transform parent, KitchenProp prop, Vector3 anchor, Vector2 screenUp)
    {
        GameObject go = new GameObject("KitchenProp");
        go.transform.SetParent(parent);

        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = prop.sprite;
        sr.sortingOrder = PropSortingOrder;

        float spriteHeight = prop.sprite.bounds.size.y;
        float scale = spriteHeight > 0f ? prop.height / spriteHeight : 1f;
        go.transform.localScale = Vector3.one * scale;

        // Stand the prop on its anchor point (sprite pivot is its center)
        Vector3 position = anchor + (Vector3)(screenUp * (prop.height * 0.5f));
        // Props higher up the screen are pushed slightly away from the camera, so nearer props draw in front
        position.z = Vector2.Dot(anchor, screenUp) * 0.001f;
        go.transform.position = position;

        go.AddComponent<FaceCamera>();
    }

    private static Vector2 Rotate(Vector2 v, float degrees)
    {
        float r = degrees * Mathf.Deg2Rad;
        float c = Mathf.Cos(r), s = Mathf.Sin(r);
        return new Vector2(v.x * c - v.y * s, v.x * s + v.y * c);
    }

    // ---------- Helpers ----------

    private void CreateArrow(Transform parent, Vector2 direction)
    {
        if (arrowSprite == null) return;

        GameObject arrow = new GameObject("JunctionArrow");
        arrow.transform.SetParent(parent);
        arrow.transform.position = _junctionCenter + (Vector3)(direction * arrowDistance);
        // The sprite points right (+X), so rotate +X to face the path direction
        arrow.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        arrow.transform.localScale = Vector3.one * arrowScale;

        SpriteRenderer sr = arrow.AddComponent<SpriteRenderer>();
        sr.sprite = arrowSprite;
        sr.sortingOrder = ArrowSortingOrder;

        arrow.SetActive(false);
        _junctionArrows.Add(arrow);
    }

    private GameObject CreateSegment(string segmentName, Vector3 position)
    {
        GameObject segment = new GameObject(segmentName);
        segment.transform.SetParent(transform);
        segment.transform.position = position;
        _segments.Add(segment);
        return segment;
    }

    private void CreateBlock(Transform parent, Vector3 position, Quaternion rotation, Vector2 size, Color color, int sortingOrder, Sprite tileSprite = null, bool flipX = false)
    {
        SpriteRenderer block = Instantiate(blockPrefab, position, rotation, parent);
        block.flipX = flipX;

        if (tileSprite != null)
        {
            // Repeat the picture across the block instead of stretching it
            block.sprite = tileSprite;
            block.drawMode = SpriteDrawMode.Tiled;
            block.size = size;
            block.transform.localScale = Vector3.one;
        }
        else
        {
            block.transform.localScale = new Vector3(size.x, size.y, 1f);
        }

        block.color = color;
        block.sortingOrder = sortingOrder;
    }

    private Color NextFloorColor()
    {
        Color color = _useAlternateFloor ? floorColorB : floorColorA;
        _useAlternateFloor = !_useAlternateFloor;
        return color;
    }

    // Removes corridor pieces that are far away AND not ahead of the player.
    // (Pieces ahead are never removed, even if they were built a little past the despawn distance.)
    private void RemoveFarSegments()
    {
        for (int i = _segments.Count - 1; i >= 0; i--)
        {
            GameObject segment = _segments[i];
            Vector3 toSegment = segment.transform.position - player.PathCenter;
            bool isAhead = Vector2.Dot(toSegment, player.Heading) > segmentLength;

            if (!isAhead && toSegment.magnitude > despawnDistance)
            {
                Destroy(segment);
                _segments.RemoveAt(i);
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