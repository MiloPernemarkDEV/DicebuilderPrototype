using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private PlayerLanes player;
    [SerializeField] private PathGenerator path;
    [SerializeField] private Coin coinPrefab;

    [Tooltip("How far ahead of the player the first coin of a line appears (should be off screen)")]
    [SerializeField] private float spawnDistance = 17f;

    [Tooltip("Seconds of running between coin waves")]
    [SerializeField] private float minSpawnInterval = 3f;
    [SerializeField] private float maxSpawnInterval = 6f;

    [Tooltip("How many lanes get coins in the same wave")]
    [SerializeField] private int minLanesAtOnce = 1;
    [SerializeField] private int maxLanesAtOnce = 3;

    [Tooltip("How many coins in one lane's line. Each lane rolls its own number")]
    [SerializeField] private int minCoinsInRow = 1;
    [SerializeField] private int maxCoinsInRow = 10;

    [Tooltip("Distance between coins in a line")]
    [SerializeField] private float coinSpacing = 0.8f;

    [Tooltip("Don't spawn if a junction is closer than the end of the longest possible coin line plus this")]
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
            // Skip this wave if the coin lines could reach into a junction
            float longestLine = spawnDistance + maxCoinsInRow * coinSpacing;
            if (!path.IsJunctionWithin(longestLine + junctionClearance))
            {
                SpawnWave();
            }
            timer = Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }

    // Picks random lanes and places a line of coins in each
    private void SpawnWave()
    {
        int lanesToUse = Mathf.Clamp(Random.Range(minLanesAtOnce, maxLanesAtOnce + 1), 1, 3);

        // Shuffle lane indices so we pick different lanes
        int[] lanes = { 0, 1, 2 };
        for (int i = lanes.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (lanes[i], lanes[j]) = (lanes[j], lanes[i]);
        }

        for (int i = 0; i < lanesToUse; i++)
        {
            int count = Random.Range(minCoinsInRow, maxCoinsInRow + 1);
            for (int c = 0; c < count; c++)
            {
                Vector3 pos = player.GetLanePosition(lanes[i], spawnDistance + c * coinSpacing);
                Coin coin = Instantiate(coinPrefab, pos, player.HeadingRotation);
                coin.Init(player);
            }
        }
    }
}