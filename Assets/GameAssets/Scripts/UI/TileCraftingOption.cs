using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileCraftingOption : MonoBehaviour
{
    [SerializeField]
    protected PieceType ItemType;
    [SerializeField]
    protected PieceType CraftingItemType;

    [SerializeField]
    protected int Id = 0;

    [SerializeField]
    protected Inventory Inventory;

    [SerializeField]
    protected GameObject AssetAmountText;
    [SerializeField]
    protected PieceController PieceController;

    [SerializeField]
    protected Button CraftButton;

    public int AmountToCraft = 10;

    protected int CurrentAmount = 0;
    
    // Start is called before the first frame update
    void Awake()
    {
        Inventory.OnItemAmountChanged += UpdateItemUI;
        CraftButton.onClick.AddListener(CraftTile);
    }

    protected void Start()
    {
        UpdateItemUI(ItemType, Id, CurrentAmount);
    }


    protected void UpdateItemUI(PieceType type, int id, int amount)
    {
        if (Id == id && ItemType == type)
        {
            TextMeshProUGUI textMeshPro = AssetAmountText.GetComponent<TextMeshProUGUI>();
            CurrentAmount = amount;
            textMeshPro.text = "" + CurrentAmount + "/" + AmountToCraft;
        }

    }
    
    protected virtual void CraftTile()
    {
        if(CurrentAmount >= AmountToCraft)
        {
            int auxRemoveAmount = AmountToCraft;
            CurrentAmount -= auxRemoveAmount;
            AmountToCraft = Mathf.FloorToInt(AmountToCraft * 1.2f);
            if (CraftingItemType == PieceType.MaterialBuff)
            {
                Inventory.RemoveMaterial(Id, auxRemoveAmount);
                PieceController.CreateTimeBuffPiece(PieceType.MaterialBuff);
            }
            else if (CraftingItemType == PieceType.ResourceBuff)
            {
                Inventory.RemoveResource(Id, auxRemoveAmount);
                PieceController.CreateTimeBuffPiece(PieceType.ResourceBuff);
            }
            else if (CraftingItemType == PieceType.BiomeBuff)
            {
                Inventory.RemoveResource(Id, auxRemoveAmount);
                PieceController.CreateBiomeBuffPiece();
            }
            else if (ItemType == PieceType.Resource)
            {
                Inventory.RemoveResource(Id, auxRemoveAmount);
                Inventory.AddAssetTile(PieceType.Material, 1);
            }
            else if(ItemType == PieceType.Material)
            {
                Inventory.RemoveMaterial(Id, auxRemoveAmount);
                Inventory.AddAssetTile(PieceType.Resource, 1);
            }
            else if(ItemType == PieceType.MapExtension)
            {
                Inventory.RemoveMaterial(Id, auxRemoveAmount);
                Inventory.AddMapExtensionTile((BiomeType)Id, auxRemoveAmount);
            }
            Debug.Log("Craft");
        }
        else 
        {
            Debug.Log("Missin resources");
        }
    }
}
