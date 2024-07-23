using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCraftingOption : TileCraftingOption
{
    protected override void CraftTile()
    {
        if (CurrentAmount >= AmountToCraft)
        {
            int auxRemoveAmount = AmountToCraft;
            CurrentAmount -= auxRemoveAmount;
            AmountToCraft = Mathf.FloorToInt(AmountToCraft * 1.2f);
            
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
