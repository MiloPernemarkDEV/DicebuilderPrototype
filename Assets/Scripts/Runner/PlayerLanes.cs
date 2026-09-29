using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLanes : MonoBehaviour
{
    [SerializeField] private float runSpeed = 5f;
    [Tooltip("Distance between the centers of two lanes")]
    [SerializeField] private float laneWidth = 1.5f;
    [SerializeField] private float laneSwitchSpeed = 20f;

    // Direction the player is running in (starts going up the screen)
    public Vector2 Heading { get; private set; } = Vector2.up;
    // Direction to the player's right, relative to the heading
    public Vector2 Right => new Vector2(Heading.y, -Heading.x);
    // Rotation that makes "up" point along the heading (used for camera and spawned items)
    public Quaternion HeadingRotation =>
        Quaternion.Euler(0f, 0f, Mathf.Atan2(Heading.y, Heading.x) * Mathf.Rad2Deg - 90f);

    public bool IsRunning { get; private set; } = true;

    // Point on the middle lane that the player is level with
    public Vector3 PathCenter { get; private set; }

    public int Coins { get; private set; }

    private int currentLane = 1;
    private float laneOffset;

    private readonly Dictionary<DiceColor, int> diceCounts = new Dictionary<DiceColor, int>
    {
        { DiceColor.Red, 0 },
        { DiceColor.Blue, 0 },
        { DiceColor.Green, 0 },
        { DiceColor.Yellow, 0 },
    };

    private void Start()
    {
        PathCenter = transform.position;
    }

    private void Update()
    {
        Keyboard kb = Keyboard.current;
        if (kb != null && IsRunning)
        {
            if (kb.qKey.wasPressedThisFrame) currentLane = 0;
            if (kb.wKey.wasPressedThisFrame) currentLane = 1;
            if (kb.eKey.wasPressedThisFrame) currentLane = 2;
        }

        if (IsRunning)
        {
            PathCenter += (Vector3)(Heading * runSpeed * Time.deltaTime);
        }

        laneOffset = Mathf.MoveTowards(laneOffset, GetLaneOffset(currentLane), laneSwitchSpeed * Time.deltaTime);
        transform.position = PathCenter + (Vector3)(Right * laneOffset);
    }

    // Keep the sprite upright on screen, whatever way the camera is turned
    private void LateUpdate()
    {
        Camera cam = Camera.main;
        if (cam != null) transform.rotation = cam.transform.rotation;
    }

    // Stops the run at a point (used at junctions). The player slides back to the middle lane.
    public void StopAt(Vector3 stopPoint)
    {
        IsRunning = false;
        PathCenter = stopPoint;
        currentLane = 1;
    }

    // Starts running again in the same direction (used when leaving the shop)
    public void Resume()
    {
        IsRunning = true;
    }

    // Starts running again in a new direction from a point (used after choosing a junction path)
    public void Turn(Vector3 newCenter, Vector2 newHeading)
    {
        PathCenter = newCenter;
        Heading = newHeading.normalized;
        currentLane = 1;
        laneOffset = 0f;
        IsRunning = true;
    }

    // 0 = left, 1 = middle, 2 = right
    public float GetLaneOffset(int lane)
    {
        return (lane - 1) * laneWidth;
    }

    // World position in a lane, a given distance ahead of the player
    public Vector3 GetLanePosition(int lane, float distanceAhead)
    {
        return PathCenter + (Vector3)(Heading * distanceAhead + Right * GetLaneOffset(lane));
    }

    public void Collect(LaneItem item)
    {
        diceCounts[item.Color]++;
        Debug.Log($"Red: {diceCounts[DiceColor.Red]} | Blue: {diceCounts[DiceColor.Blue]} | " +
                  $"Green: {diceCounts[DiceColor.Green]} | Yellow: {diceCounts[DiceColor.Yellow]}");
    }

    public void CollectCoin()
    {
        Coins++;
        Debug.Log($"Coins: {Coins}");
    }

    public int GetCount(DiceColor color)
    {
        return diceCounts[color];
    }
}