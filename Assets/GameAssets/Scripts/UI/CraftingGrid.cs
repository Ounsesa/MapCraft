using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CraftingGrid : MonoBehaviour
{
    #region Variables
    [HideInInspector]
    public PieceType craftType;
    // Initialize a 3x3 grid of integers using List<List<int>>
    public List<List<int>> grid = new List<List<int>>
    {
        new List<int> {-1, -1, -1},
        new List<int> {-1, -1, -1},
        new List<int> {-1, -1, -1}
    };

    protected int selectedTiles = 0;
    protected List<Vector2Int> notAdjacentTilesList = new List<Vector2Int>();
    [SerializeField]
    protected Inventory inventory;
    [SerializeField]
    protected Player player;
    [SerializeField]
    protected int minTilesForCraft = 4;

    [SerializeField]
    private List<CraftingTile> m_craftingTileList = new List<CraftingTile>();
    #endregion

    public void Awake()
    {
        for (int i = 0; i < m_craftingTileList.Count; i++)
        {
            m_craftingTileList[i].craftingGrid = this;
        }
    }

    public virtual void ActivateTile(Vector2Int Position, bool Activated)
    {
        grid[Position.x][Position.y] = Activated ? 0 : -1;

        selectedTiles += Activated ? 1 : -1;

        CheckAdjacency(Position, Activated);

        if(!Activated) 
        {
            if (notAdjacentTilesList.Contains(Position))
            {
                notAdjacentTilesList.Remove(Position);
            }
        }

    }

    public void RestartCraft()
    {
        selectedTiles = 0;
        for (int i = 0; i < m_craftingTileList.Count; i++)
        {
            m_craftingTileList[i].Deselect();
            grid[m_craftingTileList[i].gridPosition.x][m_craftingTileList[i].gridPosition.y] = GameManager.INVALID_TILE;
        }
    }

    public virtual bool ValidCraft()
    {
        if(player.currentPiece != null)
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
        if (inventory.GetAssetTileAmount(craftType) < selectedTiles)
        {
            return false;
        }

        return true;
    }

    public virtual void RemoveTilesUsed()
    {
        inventory.RemoveAssetTile(craftType, selectedTiles);
    }

    protected void CheckAdjacency(Vector2Int Position, bool Activated)
    {
        if (Activated)
        {
            bool IsAdjacent = false;

            Vector2Int AdjacentPosition = new Vector2Int(Position.x + 1, Position.y);
            if (AdjacentPosition.x < grid.Count && grid[AdjacentPosition.x][AdjacentPosition.y] != GameManager.INVALID_TILE)
            {
                IsAdjacent = true;
                if (notAdjacentTilesList.Contains(AdjacentPosition))
                {
                    notAdjacentTilesList.Remove(AdjacentPosition);
                }
            }
            AdjacentPosition = new Vector2Int(Position.x - 1, Position.y);
            if (AdjacentPosition.x >= 0 && grid[AdjacentPosition.x][AdjacentPosition.y] != GameManager.INVALID_TILE)
            {
                IsAdjacent = true;
                if (notAdjacentTilesList.Contains(AdjacentPosition))
                {
                    notAdjacentTilesList.Remove(AdjacentPosition);
                }
            }
            AdjacentPosition = new Vector2Int(Position.x, Position.y + 1);
            if (AdjacentPosition.y < grid[0].Count && grid[AdjacentPosition.x][AdjacentPosition.y] != GameManager.INVALID_TILE)
            {
                IsAdjacent = true;
                if (notAdjacentTilesList.Contains(AdjacentPosition))
                {
                    notAdjacentTilesList.Remove(AdjacentPosition);
                }
            }
            AdjacentPosition = new Vector2Int(Position.x, Position.y - 1);
            if (AdjacentPosition.y >= 0 && grid[AdjacentPosition.x][AdjacentPosition.y] != GameManager.INVALID_TILE)
            {
                IsAdjacent = true;
                if (notAdjacentTilesList.Contains(AdjacentPosition))
                {
                    notAdjacentTilesList.Remove(AdjacentPosition);
                }
            }

            if (!IsAdjacent)
            {
                notAdjacentTilesList.Add(Position);
            }
        }
        else
        {
            Vector2Int AdjacentPosition = new Vector2Int(Position.x + 1, Position.y);
            if (AdjacentPosition.x < grid.Count && grid[AdjacentPosition.x][AdjacentPosition.y] != GameManager.INVALID_TILE)
            {
                CheckAdjacency(AdjacentPosition, true);
            }
            AdjacentPosition = new Vector2Int(Position.x - 1, Position.y);
            if (AdjacentPosition.x >= 0 && grid[AdjacentPosition.x][AdjacentPosition.y] != GameManager.INVALID_TILE)
            {
                CheckAdjacency(AdjacentPosition, true);
            }
            AdjacentPosition = new Vector2Int(Position.x, Position.y + 1);
            if (AdjacentPosition.y < grid[0].Count && grid[AdjacentPosition.x][AdjacentPosition.y] != GameManager.INVALID_TILE)
            {
                CheckAdjacency(AdjacentPosition, true);
            }
            AdjacentPosition = new Vector2Int(Position.x, Position.y - 1);
            if (AdjacentPosition.y >= 0 && grid[AdjacentPosition.x][AdjacentPosition.y] != GameManager.INVALID_TILE)
            {
                CheckAdjacency(AdjacentPosition, true);
            }
        }
    }
}
