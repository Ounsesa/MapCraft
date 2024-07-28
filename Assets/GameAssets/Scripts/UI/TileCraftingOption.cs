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

    protected CraftCost CraftCost;

    protected int CurrentAmount = 0;
    
    // Start is called before the first frame update
    void Awake()
    {
        Inventory.OnItemAmountChanged += UpdateItemUI;
        CraftButton.onClick.AddListener(CraftTile);
    }

    protected virtual void Start()
    {

        switch (ItemType)
        {
            case PieceType.Resource:
                CraftCost = CraftCostsController.Instance.CraftingInitialCosts[CraftType.Piece].Clone();
                break;
            case PieceType.Material:
                CraftCost = CraftCostsController.Instance.CraftingInitialCosts[CraftType.Piece].Clone();
                break;
        }
        switch (CraftingItemType)
        {
            case PieceType.MaterialBuff:
                CraftCost = CraftCostsController.Instance.CraftingInitialCosts[CraftType.TimeBuff].Clone(); 
                break;
            case PieceType.ResourceBuff:
                CraftCost = CraftCostsController.Instance.CraftingInitialCosts[CraftType.TimeBuff].Clone(); 
                break;
            case PieceType.BiomeBuff:
                CraftCost = CraftCostsController.Instance.CraftingInitialCosts[CraftType.AmountBuff].Clone(); 
                break;
        }

        UpdateItemUI(ItemType, Id, CurrentAmount);

    }


    protected void UpdateItemUI(PieceType type, int id, int amount)
    {
        if (Id == id && ItemType == type)
        {
            TextMeshProUGUI textMeshPro = AssetAmountText.GetComponent<TextMeshProUGUI>();
            CurrentAmount = amount;
            textMeshPro.text = "" + CurrentAmount + "/" + CraftCost.CurrentCost;
        }

    }
    
    protected virtual void CraftTile()
    {
        if(CurrentAmount >= CraftCost.CurrentCost)
        {

            int auxRemoveAmount = CraftCost.GetCurrentCost();
            CurrentAmount -= auxRemoveAmount;
            if (CraftingItemType == PieceType.MaterialBuff)
            {
                if (PieceController.CreateBuffPiece(PieceType.MaterialBuff))
                {
                    Inventory.RemoveMaterial(Id, auxRemoveAmount);
                }
            }
            else if (CraftingItemType == PieceType.ResourceBuff)
            {
                if (PieceController.CreateBuffPiece(PieceType.ResourceBuff))
                {
                    Inventory.RemoveResource(Id, auxRemoveAmount);
                }
            }
            else if (CraftingItemType == PieceType.BiomeBuff)
            {
                if (PieceController.CreateBuffPiece(PieceType.BiomeBuff))
                {
                    Inventory.RemoveResource(Id, auxRemoveAmount);
                }
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
