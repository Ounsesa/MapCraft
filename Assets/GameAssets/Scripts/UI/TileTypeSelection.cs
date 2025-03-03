using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileTypeSelection : MonoBehaviour
{
    [SerializeField]
    private PieceType ItemType;

    [SerializeField]
    private GameObject TileAmountText;

    private int CurrentAmount = 0;

    [SerializeField]
    private Inventory Inventory;

    private Button Button;

    public CraftingGrid CraftingGrid;
    public TileTypeSelection OtherType;

    // Start is called before the first frame update
    void Awake()
    {
        Inventory.onAssetTileAmountChanged += UpdateItemUI;
        Button = GetComponent<Button>();
        Button.onClick.AddListener(SelectCraftType);
    }
    private void Start()
    {
        UpdateItemUI(ItemType, CurrentAmount);
    }

    private void UpdateItemUI(PieceType type, int amount)
    {
        if (ItemType == type)
        {
            TextMeshProUGUI textMeshPro = TileAmountText.GetComponent<TextMeshProUGUI>();
            CurrentAmount = amount;
            textMeshPro.text = "" + CurrentAmount;
        }

    }

    private void SelectCraftType()
    {
        CraftingGrid.RestartCraft();
        CraftingGrid.craftType = ItemType;
        GetComponent<Image>().color = Color.yellow;
        OtherType.GetComponent<Image>().color = Color.gray;


    }
}
