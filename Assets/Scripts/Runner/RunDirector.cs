using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Decides what comes next on the scripted run, and runs the shop and map UI.
//
// Start -> Junction 1 (either way) -> Junction 2
// Junction 2 and every junction after: Right = Shop, Left = Boss
// Shop -> new junction (same rule: Right = Shop again, Left = Boss)
// Boss -> the run stops
public class RunDirector : MonoBehaviour
{
    [SerializeField] private PathGenerator path;

    [Header("Path lengths (corridor pieces, 1 piece = 5 units)")]
    [SerializeField] private int piecesToFirstJunction = 10;
    [SerializeField] private int piecesToSecondJunction = 10;
    [SerializeField] private int piecesToShop = 6;
    [SerializeField] private int piecesToBoss = 6;
    [SerializeField] private int piecesAfterShop = 6;

    [Header("Shop (just for show)")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private TMP_Text shopCoinsText;
    [SerializeField] private Button buyMapButton;
    [SerializeField] private TMP_Text buyMapButtonText;
    [SerializeField] private Button leaveShopButton;
    [SerializeField] private int shopCoins = 5000;
    [SerializeField] private int mapPrice = 1000;

    [Header("Map")]
    [Tooltip("Press once to open the map, press again to close it")]
    [SerializeField] private Button mapButton;
    [SerializeField] private GameObject mapPanel;

    private int junctionsPassed;
    private bool hasMap;

    public bool IsMapOpen => mapPanel != null && mapPanel.activeSelf;

    private void Start()
    {
        SetActive(shopPanel, false);
        SetActive(mapPanel, false);
        if (mapButton != null) mapButton.gameObject.SetActive(false);

        if (buyMapButton != null) buyMapButton.onClick.AddListener(BuyMap);
        if (leaveShopButton != null) leaveShopButton.onClick.AddListener(LeaveShop);
        if (mapButton != null) mapButton.onClick.AddListener(ToggleMap);

        RefreshShop();
    }

    // ---------- What comes next on the path ----------

    public (StopType type, int pieces) GetFirstStop()
    {
        return (StopType.JUNCTION, piecesToFirstJunction);
    }

    public (StopType type, int pieces) GetStopAfterJunction(bool wentRight)
    {
        junctionsPassed++;

        // Junction 1: both ways just continue to Junction 2
        if (junctionsPassed == 1)
        {
            return (StopType.JUNCTION, piecesToSecondJunction);
        }

        // Junction 2 and every junction after: Right = Shop, Left = Boss
        return wentRight ? (StopType.SHOP, piecesToShop) : (StopType.BOSS, piecesToBoss);
    }

    public (StopType type, int pieces) GetStopAfterShop()
    {
        return (StopType.JUNCTION, piecesAfterShop);
    }

    // ---------- Called by the PathGenerator ----------

    public void OnReachedJunction()
    {
        if (hasMap && mapButton != null) mapButton.gameObject.SetActive(true);
    }

    public void OnLeftJunction()
    {
        if (mapButton != null) mapButton.gameObject.SetActive(false);
        SetActive(mapPanel, false);
    }

    public void OnReachedShop()
    {
        SetActive(shopPanel, true);
        RefreshShop();
    }

    public void OnReachedBoss()
    {
        Debug.Log("Reached the boss");
    }

    // ---------- UI buttons ----------

    private void BuyMap()
    {
        if (hasMap || shopCoins < mapPrice) return;

        shopCoins -= mapPrice;
        hasMap = true;
        RefreshShop();
    }

    private void LeaveShop()
    {
        SetActive(shopPanel, false);
        path.ContinueFromShop();
    }

    private void ToggleMap()
    {
        SetActive(mapPanel, !IsMapOpen);
    }

    private void RefreshShop()
    {
        if (shopCoinsText != null) shopCoinsText.text = $"Coins: {shopCoins}";
        if (buyMapButtonText != null) buyMapButtonText.text = hasMap ? "Bought" : $"Buy ({mapPrice})";
        if (buyMapButton != null) buyMapButton.interactable = !hasMap;
    }

    private static void SetActive(GameObject go, bool active)
    {
        if (go != null) go.SetActive(active);
    }
}