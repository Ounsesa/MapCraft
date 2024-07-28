using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCraftingGrid : CraftingGrid
{
    public BiomeType MapCraftType;
    public override void ActivateTile(Vector2Int Position, bool Activated)
    {
        Grid[Position.x][Position.y] = Activated ? (int)MapCraftType : -1;

        SelectedTiles += Activated ? 1 : -1;

        CheckAdjacency(Position, Activated);
        if (!Activated)
        {
            if (NotAdjacentTilesList.Contains(Position))
            {
                NotAdjacentTilesList.Remove(Position);
            }
        }
    }

    public override bool ValidCraft()
    {
        if (NotAdjacentTilesList.Count > 0)
        {
            Debug.Log("Not adjacent");
            return false;
        }
        if (SelectedTiles < MinTilesForCraft)
        {
            return false;
        }
        

        return true;
    }

    public override void RemoveTilesUsed()
    {
        Inventory.RemoveMapExtensionTile(MapCraftType, SelectedTiles);
    }
}
