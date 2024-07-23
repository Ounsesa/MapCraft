using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCraftingButton : CraftingButton
{
    public MapCraftingGrid MapGrid;
    public GameObject MapCraftingUI;
    public GameObject CurrentMapExtension;
    protected override void TryToCraft()
    {
        if (MapGrid.CanCraft)
        {

            List<List<int>> trimmedGrid = TrimGrid(MapGrid.Grid);

            CurrentMapExtension = Instantiate(PieceController.PiecePrefab);
            Piece piece = CurrentMapExtension.GetComponent<Piece>();
            piece.InitPiece(PieceType.MapExtension, PieceController.PiecePrefab, trimmedGrid);
            PieceController.SavePiece(piece);
            MapGrid.RemoveTilesUsed();
            MapGrid.RestartCraft();

            MapCraftingUI.SetActive(!MapCraftingUI.activeSelf);
            Debug.Log("Can craft");
        }
        else
        {
            Debug.Log("Can't craft");
        }
    }
}
