using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCraftingGrid : CraftingGrid
{
    [HideInInspector]
    public BiomeType mapCraftType;

    private Dictionary<BiomeType, int> m_tilesUsed = new Dictionary<BiomeType, int>();


    private void Start()
    {
        m_tilesUsed.Add(BiomeType.Forest, 0);
        m_tilesUsed.Add(BiomeType.Desert, 0);
        m_tilesUsed.Add(BiomeType.Mountain, 0);
        m_tilesUsed.Add(BiomeType.Plains, 0);
    }

    public override void ActivateTile(Vector2Int Position, bool Activated)
    {
        if(Activated)
        {
            m_tilesUsed[mapCraftType] += 1;
        }
        else
        {
            m_tilesUsed[(BiomeType)grid[Position.x][Position.y]] -= 1;
        }

        grid[Position.x][Position.y] = Activated ? (int)mapCraftType : -1;

        selectedTiles += Activated ? 1 : -1;


        CheckAdjacency(Position, Activated);
        if (!Activated)
        {
            if (notAdjacentTilesList.Contains(Position))
            {
                notAdjacentTilesList.Remove(Position);
            }
        }
    }

    public override bool ValidCraft()
    {
        if (player.currentPiece != null)
        {
            return false;
        }
        if (notAdjacentTilesList.Count > 0)
        {
            Debug.Log("Not adjacent");
            return false;
        }
        if (selectedTiles < minTilesForCraft)
        {
            return false;
        }
        
        foreach (KeyValuePair<BiomeType, int> pair in m_tilesUsed)
        {
            if(inventory.GetMapExtensionTile(pair.Key) < pair.Value)
            {
                return false;
            }
        }        

        return true;
    }

    public override void RemoveTilesUsed()
    {
        for (int i = 0; i < m_tilesUsed.Count; i++)
        {
            inventory.RemoveMapExtensionTile((BiomeType)i, m_tilesUsed[(BiomeType)i]);
            m_tilesUsed[(BiomeType)i] = 0;
        }
    }
}
