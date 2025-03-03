using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingTile : MonoBehaviour
{
    #region Variables
    public Vector2Int gridPosition = new Vector2Int();
    public CraftingGrid craftingGrid;

    protected bool selected = false;
    protected Image image;
    protected Button button;

    [SerializeField]
    private MapCraftingGrid m_mapCraftingGrid;
    #endregion

    protected void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(SelectTile);
    }

    protected virtual void SelectTile()
    {
        selected = !selected;
        if(selected)
        {
            if(m_mapCraftingGrid != null)
            {
                switch (m_mapCraftingGrid.mapCraftType)
                {
                    case BiomeType.Forest:
                        image.color = Color.green;
                        break;
                    case BiomeType.Desert:
                        image.color = Color.yellow;
                        break;
                    case BiomeType.Mountain:
                        image.color = Color.gray;
                        break;
                    case BiomeType.Plains:
                        image.color = Color.blue;
                        break;
                }
                m_mapCraftingGrid.ActivateTile(gridPosition, selected);
            }
            else
            {
                image.color = Color.yellow;
                craftingGrid.ActivateTile(gridPosition, selected);
            }
        }
        else 
        {
            if (m_mapCraftingGrid != null)
            {
                image.color = Color.black;
                m_mapCraftingGrid.ActivateTile(gridPosition, selected);
            } 
            else
            {
                image.color = Color.gray;
                craftingGrid.ActivateTile(gridPosition, selected);
            }
        }
    }

    public void Deselect()
    {
        selected = false;
        if (m_mapCraftingGrid != null)
        {
            image.color = Color.black;
        }
        else
        {
            image.color = Color.gray;
        }
    }
}
