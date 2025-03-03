using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private PieceType m_itemType;
    [SerializeField]
    private int m_id = 0;
    [SerializeField]
    private Inventory m_inventory;
    [SerializeField]
    private GameObject m_inventoryQuantityText;
    #endregion

    private void Awake()
    {
        m_inventory.onItemAmountChanged += UpdateItemUI;
    }

    private void UpdateItemUI(PieceType type, int id, int amount)
    {
        if(m_id == id && m_itemType == type)
        {
            TextMeshProUGUI textMeshPro = m_inventoryQuantityText.GetComponent<TextMeshProUGUI>();
            textMeshPro.text = "" + amount;
        }        
    }
}
