using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinalExtensionUI : MonoBehaviour
{

    #region Variables
    [SerializeField]
    private Inventory m_inventory;
    [SerializeField]
    private GameObject m_assetAmountText;
    [SerializeField]
    private Player m_player;
    [SerializeField]
    private PieceController m_pieceController;

    private int m_maxTilesNeeded = 9;
    private int m_currentTileAmount = 0;
    private bool m_tutorialShown = false;
    #endregion

    void Awake()
    {
        m_inventory.onFinalExtensionTileAmountChanged += OnUpdateUI;
        GetComponent<Button>().onClick.AddListener(OnButtonClicked);        
    }

    void OnButtonClicked()
    {
        if(m_currentTileAmount < m_maxTilesNeeded)
        {
            return;
        }
        if(m_player.currentPiece)
        {
            return;
        }

        GameObject CurrentMapExtension = Instantiate(m_pieceController.piecePrefab);
        Piece piece = CurrentMapExtension.GetComponent<Piece>();

        List<List<int>> Matrix = new List<List<int>>()
        {
            new List<int> { 4, 4, 4 },
            new List<int> { 4, 4, 4 },
            new List<int> { 4,4, 4 }
        };

        piece.InitPiece(PieceType.MapExtension, Matrix);
        m_pieceController.SavePiece(piece);

        GetComponent<Canvas>().enabled = false;


    }

    void OnUpdateUI(int NewAmount)
    {
        GetComponent<Canvas>().enabled = true;
        m_currentTileAmount = NewAmount;
        TextMeshProUGUI textMeshPro = m_assetAmountText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = "" + m_currentTileAmount + "/" + m_maxTilesNeeded;

        if (m_currentTileAmount == m_maxTilesNeeded)
        {
            m_inventory.onFinalExtensionTileAmountChanged -= OnUpdateUI;
            Tutorial.instance.ShowFinalCraftTutorial();
        }

        if(!m_tutorialShown)
        {
            m_tutorialShown = true;
            Tutorial.instance.ShowFinalTutorial();
        }
    }
}
