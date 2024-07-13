using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TileTypeSelection : MonoBehaviour
{
    [SerializeField]
    private PieceType ItemType;

    [SerializeField]
    private GameObject TileAmountText;

    private int CurrentAmount = 0;

    [SerializeField]
    private Inventory Inventory;

    // Start is called before the first frame update
    void Awake()
    {
        Inventory.OnAssetTileAmountChanged += UpdateItemUI;
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
}
