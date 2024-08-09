using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCraftingGrid : CraftingGrid
{
    public BiomeType MapCraftType;

    private Dictionary<BiomeType, int> TilesUsed = new Dictionary<BiomeType, int>();


    private void Start()
    {
        TilesUsed.Add(BiomeType.Forest, 0);
        TilesUsed.Add(BiomeType.Desert, 0);
        TilesUsed.Add(BiomeType.Mountain, 0);
        TilesUsed.Add(BiomeType.Plains, 0);
    }

    public override void ActivateTile(Vector2Int Position, bool Activated)
    {
        Grid[Position.x][Position.y] = Activated ? (int)MapCraftType : -1;

        SelectedTiles += Activated ? 1 : -1;

        TilesUsed[MapCraftType] += Activated ? 1 : -1;

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
        if (Player.CurrentPiece != null)
        {
            return false;
        }
        if (NotAdjacentTilesList.Count > 0)
        {
            Debug.Log("Not adjacent");
            return false;
        }
        if (SelectedTiles < MinTilesForCraft)
        {
            return false;
        }
        
        foreach (KeyValuePair<BiomeType, int> pair in TilesUsed)
        {
            if(Inventory.GetMapExtensionTile(pair.Key) < pair.Value)
            {
                return false;
            }
        }        

        return true;
    }

    public override void RemoveTilesUsed()
    {
        for (int i = 0; i < TilesUsed.Count; i++)
        {
            Inventory.RemoveMapExtensionTile((BiomeType)i, TilesUsed[(BiomeType)i]);
            TilesUsed[(BiomeType)i] = 0;
        }
    }
}
