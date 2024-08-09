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
        if (MapGrid.ValidCraft())
        {

            List<List<int>> trimmedGrid = TrimGrid(MapGrid.Grid);

            CurrentMapExtension = Instantiate(PieceController.PiecePrefab);
            Piece piece = CurrentMapExtension.GetComponent<Piece>();
            piece.InitPiece(PieceType.MapExtension, trimmedGrid);
            PieceController.SavePiece(piece);
            MapGrid.RemoveTilesUsed();
            MapGrid.RestartCraft();

            CraftingUI.GetComponent<Canvas>().enabled = false;

            player.CanPlacePiece = true;
            Debug.Log("Can craft");
        }
        else
        {
            Debug.Log("Can't craft");
        }
    }
}
