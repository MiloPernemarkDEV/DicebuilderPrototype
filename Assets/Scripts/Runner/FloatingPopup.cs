using TMPro;
using UnityEngine;

// Floats up above a target, pops in, fades out, then removes itself.
// Created by CoinPopupSpawner, you don't add this by hand.
public class FloatingPopup : MonoBehaviour
{
    private Transform target;
    private Vector3 lastTargetPosition;
    private float height;
    private float rise;
    private float duration;
    private float sideOffset;
    private float time;
    private TMP_Text text;
    private SpriteRenderer icon;

    public void Init(Transform followTarget, float heightAbove, float riseDistance, float lifetime,
                     TMP_Text popupText, SpriteRenderer popupIcon)
    {
        target = followTarget;
        height = heightAbove;
        rise = riseDistance;
        duration = Mathf.Max(0.05f, lifetime);
        text = popupText;
        icon = popupIcon;

        // Small random sideways offset so quick pickups don't stack exactly on top of each other
        sideOffset = Random.Range(-0.25f, 0.25f);

        if (target != null) lastTargetPosition = target.position;
        UpdateVisuals(0f);
    }

    private void LateUpdate()
    {
        time += Time.deltaTime;
        float progress = time / duration;

        if (progress >= 1f)
        {
            Destroy(gameObject);
            return;
        }

        UpdateVisuals(progress);
    }

    private void UpdateVisuals(float progress)
    {
        Camera cam = Camera.main;
        Quaternion rotation = cam != null ? cam.transform.rotation : Quaternion.identity;
        Vector3 up = rotation * Vector3.up;
        Vector3 right = rotation * Vector3.right;

        // Follow the player so the popup stays over their head while running
        if (target != null) lastTargetPosition = target.position;

        float eased = 1f - (1f - progress) * (1f - progress);
        transform.position = lastTargetPosition + up * (height + rise * eased) + right * sideOffset;
        transform.rotation = rotation;

        // Pop in slightly bigger, then settle
        float scale = progress < 0.15f
            ? Mathf.Lerp(0.6f, 1.15f, progress / 0.15f)
            : Mathf.Lerp(1.15f, 1f, Mathf.Clamp01((progress - 0.15f) / 0.15f));
        transform.localScale = Vector3.one * scale;

        // Fade out over the last 40%
        float alpha = progress < 0.6f ? 1f : 1f - (progress - 0.6f) / 0.4f;
        if (text != null) text.alpha = alpha;
        if (icon != null)
        {
            Color c = icon.color;
            c.a = alpha;
            icon.color = c;
        }
    }
}