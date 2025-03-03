using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCraftingOption : TileCraftingOption
{
    protected override void Start()
    {
        CraftCost = CraftCostsController.instance.craftingInitialCosts[CraftType.MapExtension].Clone();        
        UpdateItemUI(ItemType, Id, CurrentAmount);

    }
    protected override void CraftTile()
    {
        if (CurrentAmount >= CraftCost.CurrentCost)
        {
            int auxRemoveAmount = CraftCost.GetCurrentCost();
            CurrentAmount -= auxRemoveAmount;
            
            Inventory.RemoveMaterial(Id, auxRemoveAmount);
            Inventory.AddMapExtensionTile((BiomeType)Id, 1);
            
            Debug.Log("Craft");
        }
        else
        {
            Debug.Log("Missin resources");
        }
    }
}
