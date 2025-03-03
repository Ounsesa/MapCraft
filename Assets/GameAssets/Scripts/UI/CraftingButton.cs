using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CraftingButton : MonoBehaviour
{
    public CraftingGrid grid;
    public PieceController PieceController;
    public GameObject CraftingUI;
    public Player player;

    private bool TutorialShown = false;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(TryToCraft);
    }
    protected virtual void TryToCraft()
    {
        if(grid.ValidCraft())
        {
            List<List<int>> trimmedGrid = TrimGrid(grid.grid);

            GameObject tile = Instantiate(PieceController.piecePrefab);
            Piece piece = tile.GetComponent<Piece>();
            piece.InitPiece(grid.craftType, trimmedGrid);
            PieceController.SavePiece(piece);
            grid.RemoveTilesUsed();
            grid.RestartCraft();
            CraftingUI.GetComponent<Canvas>().enabled = false;

            player.canPlacePiece = true;
            Debug.Log("Can craft");

            if(!TutorialShown)
            {
                TutorialShown = true;
                Tutorial.instance.ShowTrashTutorial();
            }
        }
        else 
        {
            Debug.Log("Can't craft");
        }
    }

    protected List<List<int>> TrimGrid(List<List<int>> grid)
    {
        // Step 1: Identify rows to remove
        HashSet<int> rowsToRemove = new HashSet<int>();
        for (int i = 0; i < grid.Count; i++)
        {
            if (grid[i].All(cell => cell == -1))
            {
                rowsToRemove.Add(i);
            }
        }

        // Step 2: Identify columns to remove
        HashSet<int> columnsToRemove = new HashSet<int>();
        for (int j = 0; j < grid[0].Count; j++)
        {
            bool isZeroColumn = true;
            for (int i = 0; i < grid.Count; i++)
            {
                if (grid[i][j] != -1)
                {
                    isZeroColumn = false;
                    break;
                }
            }
            if (isZeroColumn)
            {
                columnsToRemove.Add(j);
            }
        }

        // Step 3: Construct the new trimmed grid
        List<List<int>> trimmedGrid = new List<List<int>>();
        for (int i = 0; i < grid.Count; i++)
        {
            if (!rowsToRemove.Contains(i))
            {
                List<int> newRow = new List<int>();
                for (int j = 0; j < grid[i].Count; j++)
                {
                    if (!columnsToRemove.Contains(j))
                    {
                        newRow.Add(grid[i][j]);
                    }
                }
                trimmedGrid.Add(newRow);
            }
        }

        return trimmedGrid;
    }
}
