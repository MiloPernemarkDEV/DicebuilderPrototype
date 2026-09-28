using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private PlayerLanes player;
    [SerializeField] private PathGenerator path;
    [SerializeField] private LaneItem[] itemPrefabs;

    [Tooltip("How far ahead of the player dice rows appear (should be off screen)")]
    [SerializeField] private float spawnDistance = 17f;
    [SerializeField] private float minSpawnInterval = 1.5f;
    [SerializeField] private float maxSpawnInterval = 2.5f;
    [Tooltip("Don't spawn if a junction is closer than spawn distance plus this")]
    [SerializeField] private float junctionClearance = 3f;

    private float timer;

    private void Start()
    {
        timer = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void Update()
    {
        // Only count down while the player is running
        if (!player.IsRunning) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            // Skip this row if it would land in or past a junction
            if (!path.IsJunctionWithin(spawnDistance + junctionClearance))
            {
                SpawnRow();
            }
            timer = Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }

    // Spawns one random die in every lane, side by side, ahead of the player
    private void SpawnRow()
    {
        DiceRow row = new DiceRow();

        for (int lane = 0; lane < 3; lane++)
        {
            LaneItem prefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];
            Vector3 pos = player.GetLanePosition(lane, spawnDistance);
            LaneItem item = Instantiate(prefab, pos, player.HeadingRotation);
            item.Init(player, row);
            row.Add(item);
        }
    }
}