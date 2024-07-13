using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    [SerializeField]
    private PieceType ItemType;

    [SerializeField]
    private int Id = 0;

    [SerializeField]
    private Inventory Inventory;

    [SerializeField]
    private GameObject InventoryQuantityText;

    private void Awake()
    {
        Inventory.OnItemAmountChanged += UpdateItemUI;
    }
    private void UpdateItemUI(PieceType type, int id, int amount)
    {
        if(Id == id && ItemType == type)
        {
            TextMeshProUGUI textMeshPro = InventoryQuantityText.GetComponent<TextMeshProUGUI>();
            textMeshPro.text = "" + amount;
        }        
    }
}
