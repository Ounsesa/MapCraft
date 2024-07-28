using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingTile : MonoBehaviour
{
    public Vector2Int GridPosition = new Vector2Int();
    public CraftingGrid CraftingGrid;
    public MapCraftingGrid MapCraftingGrid;

    protected bool Selected = false;

    protected Image Image;
    protected Button Button;

    protected void Awake()
    {
        Image = GetComponent<Image>();
        Button = GetComponent<Button>();
        Button.onClick.AddListener(SelectTile);
    }

    protected virtual void SelectTile()
    {
        Selected = !Selected;
        if(Selected)
        {
            if(MapCraftingGrid != null)
            {
                switch (MapCraftingGrid.MapCraftType)
                {
                    case BiomeType.Forest:
                        Image.color = Color.green;
                        break;
                    case BiomeType.Desert:
                        Image.color = Color.yellow;
                        break;
                    case BiomeType.Mountain:
                        Image.color = Color.gray;
                        break;
                    case BiomeType.Plains:
                        Image.color = Color.blue;
                        break;
                }
                MapCraftingGrid.ActivateTile(GridPosition, Selected);
            }
            else
            {
                Image.color = Color.yellow;
                CraftingGrid.ActivateTile(GridPosition, Selected);
            }
        }
        else 
        {
            if (MapCraftingGrid != null)
            {
                Image.color = Color.black;
                MapCraftingGrid.ActivateTile(GridPosition, Selected);
            } 
            else
            {
                Image.color = Color.gray;
                CraftingGrid.ActivateTile(GridPosition, Selected);
            }
        }
    }

    public void Deselect()
    {
        Selected = false;
        Image.color = Color.gray;
    }
}
