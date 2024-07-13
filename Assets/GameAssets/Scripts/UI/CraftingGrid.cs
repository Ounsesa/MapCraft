using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CraftingGrid : MonoBehaviour
{
    public PieceType CraftType;
    public Inventory Inventory;

    // Initialize a 3x3 grid of integers
    private int[,] Grid = new int[3, 3];

    private int NeededTiles = 0;
    
    public List<CraftingTile> CraftingTileList = new List<CraftingTile>();

    public void Awake()
    {
        for(int i = 0; i < CraftingTileList.Count; i++) 
        {
            CraftingTileList[i].CraftingGrid = this;
        }
    }

    public void ActivateTile(Vector2Int Position, bool Activated)
    {
        Grid[Position.x, Position.y] = Activated ? 1 : 0;

        NeededTiles += Activated ? 1 : -1;

        if (ValidCraft())
        {
            Debug.Log("ValidCraft");
        }
    }

    public void RestartCraft()
    {
        NeededTiles = 0;
        for (int i = 0; i < CraftingTileList.Count; i++)
        {
            CraftingTileList[i].Deselect();
            Grid[CraftingTileList[i].GridPosition.x, CraftingTileList[i].GridPosition.y] = 0;
        }
    }

    private bool ValidCraft()
    {
        if (Inventory.GetAssetTileAmount(CraftType) < NeededTiles)
        {
            return false;
        }
        return true;
    }
}
