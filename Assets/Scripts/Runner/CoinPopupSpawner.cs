using TMPro;
using UnityEngine;

// Put this on the Player. Each collected coin shows a "+1" (with a coin icon) above the head.
public class CoinPopupSpawner : MonoBehaviour
{
    [Tooltip("Small coin shown next to the +1. Optional")]
    [SerializeField] private Sprite coinIcon;
    [SerializeField] private string popupText = "+1";

    [Header("Movement")]
    [Tooltip("Where the popup starts, measured up from the player's center")]
    [SerializeField] private float heightAboveHead = 1.3f;
    [Tooltip("How far it floats up before disappearing")]
    [SerializeField] private float riseDistance = 0.8f;
    [SerializeField] private float duration = 0.7f;

    [Header("Look")]
    [Tooltip("Font for the +1. Use LiberationSans SDF (TextMesh Pro > Resources > Fonts & Materials) or any TMP font asset")]
    [SerializeField] private TMP_FontAsset font;
    [SerializeField] private float fontSize = 5f;
    [SerializeField] private Color textColor = new Color(1f, 0.86f, 0.3f);
    [SerializeField] private Color outlineColor = new Color(0.13f, 0.08f, 0.05f);
    [Tooltip("Size of the coin icon in world units")]
    [SerializeField] private float iconSize = 0.45f;
    [Tooltip("Draw order. Keep it above the player (2)")]
    [SerializeField] private int sortingOrder = 20;

    public void Spawn()
    {
        GameObject root = new GameObject("CoinPopup");

        // "+1" text
        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(root.transform, false);

        // Add the text while the object is inactive, so TMP doesn't go looking for
        // its default font before we've given it ours (that's what caused the warning)
        textObject.SetActive(false);
        TextMeshPro text = textObject.AddComponent<TextMeshPro>();
        if (font != null) text.font = font;
        else Debug.LogWarning("CoinPopupSpawner: no font assigned. Drag a TMP font asset into the Font field on the Player.", this);
        textObject.SetActive(true);

        text.text = popupText;
        text.fontSize = fontSize;
        text.fontStyle = FontStyles.Bold;
        text.alignment = TextAlignmentOptions.Center;
        text.color = textColor;
        text.outlineWidth = 0.25f;
        text.outlineColor = outlineColor;
        text.sortingOrder = sortingOrder;
        text.rectTransform.sizeDelta = new Vector2(3f, 1.5f);

        // Coin icon to the left of the text
        SpriteRenderer icon = null;
        if (coinIcon != null)
        {
            textObject.transform.localPosition = new Vector3(iconSize * 0.6f, 0f, 0f);

            GameObject iconObject = new GameObject("Icon");
            iconObject.transform.SetParent(root.transform, false);
            iconObject.transform.localPosition = new Vector3(-iconSize * 0.9f, 0f, 0f);
            icon = iconObject.AddComponent<SpriteRenderer>();
            icon.sprite = coinIcon;
            icon.sortingOrder = sortingOrder;

            float spriteHeight = coinIcon.bounds.size.y;
            if (spriteHeight > 0f) iconObject.transform.localScale = Vector3.one * (iconSize / spriteHeight);
        }

        FloatingPopup popup = root.AddComponent<FloatingPopup>();
        popup.Init(transform, heightAboveHead, riseDistance, duration, text, icon);
    }
}