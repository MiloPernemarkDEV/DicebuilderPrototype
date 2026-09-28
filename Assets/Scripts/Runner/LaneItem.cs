using UnityEngine;

public enum DiceColor { Red, Blue, Green, Yellow }

public class LaneItem : MonoBehaviour
{
    [SerializeField] private DiceColor diceColor;
    public DiceColor Color => diceColor;

    [Tooltip("How see-through the dice you didn't pick become (0 = invisible, 1 = solid)")]
    [SerializeField] private float skippedAlpha = 0.3f;
    [Tooltip("Removed once this far from the player (after being passed)")]
    [SerializeField] private float despawnDistance = 35f;

    private PlayerLanes player;
    private DiceRow row;

    public void Init(PlayerLanes owner, DiceRow diceRow)
    {
        player = owner;
        row = diceRow;
    }

    private void Update()
    {
        if (player == null) return;

        if ((transform.position - player.transform.position).sqrMagnitude > despawnDistance * despawnDistance)
        {
            Destroy(gameObject);
        }
    }

    // Called when another die in the same row was picked
    public void MarkSkipped()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            var c = sr.color;
            c.a = skippedAlpha;
            sr.color = c;
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
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

        // Another die in this row was already picked
        if (row != null && row.Claimed) return;

        PlayerLanes owner = other.GetComponent<PlayerLanes>();
        if (owner == null) return;

        owner.Collect(this);
        row?.Claim(this);
        Destroy(gameObject);
    }
}