using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinalExtensionUI : MonoBehaviour
{

    [SerializeField]
    private Inventory Inventory;

    [SerializeField]
    private GameObject AssetAmountText;

    [SerializeField]
    private Player Player;
    [SerializeField]
    private PieceController PieceController;

    private int MaxTilesNeeded = 9;
    private int CurrentTileAmount = 0;

    // Start is called before the first frame update
    void Awake()
    {
        Inventory.OnFinalExtensionTileAmountChanged += OnUpdateUI;
        GetComponent<Button>().onClick.AddListener(OnButtonClicked);

        
    }

    void OnButtonClicked()
    {
        if(CurrentTileAmount < MaxTilesNeeded)
        {
            return;
        }
        if(Player.CurrentPiece)
        {
            return;
        }

        GameObject CurrentMapExtension = Instantiate(PieceController.PiecePrefab);
        Piece piece = CurrentMapExtension.GetComponent<Piece>();

        List<List<int>> Matrix = new List<List<int>>()
        {
            new List<int> { 4, 4, 4 },
            new List<int> { 4, 4, 4 },
            new List<int> { 4,4, 4 }
        };

        piece.InitPiece(PieceType.MapExtension, Matrix);
        PieceController.SavePiece(piece);
    }

    void OnUpdateUI(int NewAmount)
    {
        GetComponent<Canvas>().enabled = true;
        CurrentTileAmount = NewAmount;
        TextMeshProUGUI textMeshPro = AssetAmountText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = "" + CurrentTileAmount + "/" + MaxTilesNeeded;
    }
}
