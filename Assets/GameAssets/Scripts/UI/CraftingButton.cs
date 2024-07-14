using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingButton : MonoBehaviour
{
    public CraftingGrid grid;
    public PieceController PieceController;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(TryToCraft);
    }
    private void TryToCraft()
    {
        if(grid.CanCraft)
        {
            Piece piece = new Piece();
            piece.Type = grid.CraftType;
            piece.Matrix = grid.Grid;
            PieceController.SavePiece(piece);
            Debug.Log("Can craft");
        }
        else 
        {
            Debug.Log("Can't craft");
        }
    }
}
