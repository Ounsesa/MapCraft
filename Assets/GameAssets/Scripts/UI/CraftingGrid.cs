using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CraftingGrid : MonoBehaviour
{
    public PieceType CraftType;
    public Inventory Inventory;
    public PieceController PieceController;
    public Player Player;

    // Initialize a 3x3 grid of integers using List<List<int>>
    public List<List<int>> Grid = new List<List<int>>
    {
        new List<int> {-1, -1, -1},
        new List<int> {-1, -1, -1},
        new List<int> {-1, -1, -1}
    };

    protected int SelectedTiles = 0;
    public int MinTilesForCraft = 4;

    public List<CraftingTile> CraftingTileList = new List<CraftingTile>();
    public List<Vector2Int> NotAdjacentTilesList = new List<Vector2Int>();

    public void Awake()
    {
        for (int i = 0; i < CraftingTileList.Count; i++)
        {
            CraftingTileList[i].CraftingGrid = this;
        }
    }

    public virtual void ActivateTile(Vector2Int Position, bool Activated)
    {
        Grid[Position.x][Position.y] = Activated ? 0 : -1;

        SelectedTiles += Activated ? 1 : -1;

        CheckAdjacency(Position, Activated);

    }

    public void RestartCraft()
    {
        SelectedTiles = 0;
        for (int i = 0; i < CraftingTileList.Count; i++)
        {
            CraftingTileList[i].Deselect();
            Grid[CraftingTileList[i].GridPosition.x][CraftingTileList[i].GridPosition.y] = GameManager.Instance.INVALID_TILE;
        }
    }

    public virtual bool ValidCraft()
    {
        if(Player.CurrentPiece != null)
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
        if (Inventory.GetAssetTileAmount(CraftType) < SelectedTiles)
        {
            return false;
        }

        return true;
    }

    public virtual void RemoveTilesUsed()
    {
        Inventory.RemoveAssetTile(CraftType, SelectedTiles);
    }

    protected void CheckAdjacency(Vector2Int Position, bool Activated)
    {
        if (Activated)
        {
            bool IsAdjacent = false;

            Vector2Int AdjacentPosition = new Vector2Int(Position.x + 1, Position.y);
            if (AdjacentPosition.x < Grid.Count && Grid[AdjacentPosition.x][AdjacentPosition.y] != GameManager.Instance.INVALID_TILE)
            {
                IsAdjacent = true;
                if (NotAdjacentTilesList.Contains(AdjacentPosition))
                {
                    NotAdjacentTilesList.Remove(AdjacentPosition);
                }
            }
            AdjacentPosition = new Vector2Int(Position.x - 1, Position.y);
            if (AdjacentPosition.x >= 0 && Grid[AdjacentPosition.x][AdjacentPosition.y] != GameManager.Instance.INVALID_TILE)
            {
                IsAdjacent = true;
                if (NotAdjacentTilesList.Contains(AdjacentPosition))
                {
                    NotAdjacentTilesList.Remove(AdjacentPosition);
                }
            }
            AdjacentPosition = new Vector2Int(Position.x, Position.y + 1);
            if (AdjacentPosition.y < Grid[0].Count && Grid[AdjacentPosition.x][AdjacentPosition.y] != GameManager.Instance.INVALID_TILE)
            {
                IsAdjacent = true;
                if (NotAdjacentTilesList.Contains(AdjacentPosition))
                {
                    NotAdjacentTilesList.Remove(AdjacentPosition);
                }
            }
            AdjacentPosition = new Vector2Int(Position.x, Position.y - 1);
            if (AdjacentPosition.y >= 0 && Grid[AdjacentPosition.x][AdjacentPosition.y] != GameManager.Instance.INVALID_TILE)
            {
                IsAdjacent = true;
                if (NotAdjacentTilesList.Contains(AdjacentPosition))
                {
                    NotAdjacentTilesList.Remove(AdjacentPosition);
                }
            }

            if (!IsAdjacent)
            {
                NotAdjacentTilesList.Add(Position);
            }
        }
        else
        {
            Vector2Int AdjacentPosition = new Vector2Int(Position.x + 1, Position.y);
            if (AdjacentPosition.x < Grid.Count && Grid[AdjacentPosition.x][AdjacentPosition.y] != GameManager.Instance.INVALID_TILE)
            {
                CheckAdjacency(AdjacentPosition, true);
            }
            AdjacentPosition = new Vector2Int(Position.x - 1, Position.y);
            if (AdjacentPosition.x >= 0 && Grid[AdjacentPosition.x][AdjacentPosition.y] != GameManager.Instance.INVALID_TILE)
            {
                CheckAdjacency(AdjacentPosition, true);
            }
            AdjacentPosition = new Vector2Int(Position.x, Position.y + 1);
            if (AdjacentPosition.y < Grid[0].Count && Grid[AdjacentPosition.x][AdjacentPosition.y] != GameManager.Instance.INVALID_TILE)
            {
                CheckAdjacency(AdjacentPosition, true);
            }
            AdjacentPosition = new Vector2Int(Position.x, Position.y - 1);
            if (AdjacentPosition.y >= 0 && Grid[AdjacentPosition.x][AdjacentPosition.y] != GameManager.Instance.INVALID_TILE)
            {
                CheckAdjacency(AdjacentPosition, true);
            }
        }
    }
}
