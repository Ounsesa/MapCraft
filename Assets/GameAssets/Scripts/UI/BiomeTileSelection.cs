using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BiomeTileSelection : MonoBehaviour
{
    [SerializeField]
    private BiomeType BiomeType;

    [SerializeField]
    private GameObject TileAmountText;

    private int CurrentAmount = 0;

    [SerializeField]
    private Inventory Inventory;

    private Button Button;

    public MapCraftingGrid MapCraftingGrid;

    public List<BiomeTileSelection> OtherTileSelections;

    // Start is called before the first frame update
    void Awake()
    {
        Inventory.OnMapExtensionTileAmountChanged += UpdateItemUI;
        Button = GetComponent<Button>();
        Button.onClick.AddListener(SelectCraftType);
    }
    private void Start()
    {
        UpdateItemUI(BiomeType, CurrentAmount);
        if(BiomeType == BiomeType.Forest)
        {
            GetComponent<Image>().color = Color.yellow;
        }
    }

    private void UpdateItemUI(BiomeType type, int amount)
    {
        if (BiomeType == type)
        {
            TextMeshProUGUI textMeshPro = TileAmountText.GetComponent<TextMeshProUGUI>();
            CurrentAmount = amount;
            textMeshPro.text = "" + CurrentAmount;
        }

    }

    private void SelectCraftType()
    {
        MapCraftingGrid.MapCraftType = BiomeType;
        GetComponent<Image>().color = Color.yellow;

        for(int i = 0; i < OtherTileSelections.Count; i++)
        {
            OtherTileSelections[i].GetComponent<Image>().color = Color.gray;
        }

    }
}
