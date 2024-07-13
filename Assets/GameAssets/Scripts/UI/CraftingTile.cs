using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingTile : MonoBehaviour
{
    public Vector2Int GridPosition = new Vector2Int();
    public CraftingGrid CraftingGrid;

    private bool Selected = false;

    private Image Image;
    private Button Button;

    private void Awake()
    {
        Image = GetComponent<Image>();
        Button = GetComponent<Button>();
        Button.onClick.AddListener(SelectTile);
    }

    private void SelectTile()
    {
        Selected = !Selected;
        if(Selected)
        {
            Image.color = Color.yellow;
        }
        else 
        {
            Image.color = Color.gray;
        }
        CraftingGrid.ActivateTile(GridPosition, Selected);
    }

    public void Deselect()
    {
        Selected = false;
        Image.color = Color.gray;
    }
}
