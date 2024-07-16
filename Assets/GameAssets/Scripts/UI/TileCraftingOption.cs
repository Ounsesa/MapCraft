using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileCraftingOption : MonoBehaviour
{
    [SerializeField]
    private PieceType ItemType;

    [SerializeField]
    private int Id = 0;

    [SerializeField]
    private Inventory Inventory;

    [SerializeField]
    private GameObject AssetAmountText;

    [SerializeField]
    private Button CraftButton;

    public int AmountToCraft = 10;

    private int CurrentAmount = 0;
    
    // Start is called before the first frame update
    void Awake()
    {
        Inventory.OnItemAmountChanged += UpdateItemUI;
        CraftButton.onClick.AddListener(CraftTile);
    }

    private void Start()
    {
        UpdateItemUI(ItemType, Id, CurrentAmount);
    }


    private void UpdateItemUI(PieceType type, int id, int amount)
    {
        if (Id == id && ItemType == type)
        {
            TextMeshProUGUI textMeshPro = AssetAmountText.GetComponent<TextMeshProUGUI>();
            CurrentAmount = amount;
            textMeshPro.text = "" + CurrentAmount + "/" + AmountToCraft;
        }

    }
    
    private void CraftTile()
    {
        if(CurrentAmount >= AmountToCraft)
        {
            int auxRemoveAmount = AmountToCraft;
            CurrentAmount -= auxRemoveAmount;
            AmountToCraft = Mathf.FloorToInt(AmountToCraft * 1.2f);
            if (ItemType == PieceType.Resource)
            {
                Inventory.RemoveResource(Id, auxRemoveAmount);
                Inventory.AddAssetTile(PieceType.Material, 1);
            }
            else if(ItemType == PieceType.Material)
            {
                Inventory.RemoveMaterial(Id, auxRemoveAmount);
                Inventory.AddAssetTile(PieceType.Resource, 1);
            }
            Debug.Log("Craft");
        }
        else 
        {
            Debug.Log("Missin resources");
        }
    }
}
