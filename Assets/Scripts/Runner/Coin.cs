using UnityEngine;

public class Coin : MonoBehaviour
{
    [Tooltip("Removed once this far from the player (after being passed)")]
    [SerializeField] private float despawnDistance = 35f;

    private PlayerLanes player;

    public void Init(PlayerLanes owner)
    {
        player = owner;
    }

    private void Update()
    {
        if (player == null) return;

        if ((transform.position - player.transform.position).sqrMagnitude > despawnDistance * despawnDistance)
        {
            Destroy(gameObject);
        }
    }

    // Keep the sprite upright on screen, whatever way the camera is turned
    private void LateUpdate()
    {
        Camera cam = Camera.main;
        if (cam != null) transform.rotation = cam.transform.rotation;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerLanes owner = other.GetComponent<PlayerLanes>();
        if (owner == null) return;

        owner.CollectCoin();
        Destroy(gameObject);
    }
}