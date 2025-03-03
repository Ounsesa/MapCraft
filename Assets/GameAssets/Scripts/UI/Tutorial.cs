using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{

    #region Variables
    public static Tutorial instance;

    private List<string> m_tutorialStrings;
    private int m_startingString = 0;
    private int m_endingString = 5;
    private int m_startTutorialPiecePlaced = 6;
    private int m_endTutorialPiecePlaced = 9;
    private int m_startTutorialCrafting = 10;
    private int m_endTutorialCrafting = 20;
    private int m_startTutorialTrash = 21;
    private int m_endTutorialTrash = 23;
    private int m_startTutorialMapExtensions = 24;
    private int m_endTutorialMapExtensions = 30;
    private int m_startTutorialTimeBuffs = 31;
    private int m_endTutorialTimeBuffs = 33;
    private int m_startTutorialBiomeBuffs = 34;
    private int m_endTutorialBiomeBuffs = 36;
    private int m_startTutorialFinalExtension = 37;
    private int m_endTutorialFinalExtension = 39;
    private int m_startTutorialFinalExtensionCrafted = 40;
    private int m_endTutorialFinalExtensionCrafted = 41;
    private int m_startTutorialFinal = 42;
    private int m_endTutorialFinal = 43;

    [SerializeField]
    private TextMeshProUGUI m_textMeshPro;
    [SerializeField]
    private Player m_player;

    private bool m_playerGivenPiece = false;
    private string m_tutorialFileName = "Tutorial";

    #endregion


    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        instance = this;
        GetComponent<Button>().onClick.AddListener(TutorialClicked);
    }

    private void Start()
    {
        m_tutorialStrings = CSVParser.ParseCSVToTutorial(m_tutorialFileName);

        StartTutorial(m_startingString, m_endingString);
    }


    void TutorialClicked()
    {
        if(m_startingString == m_endTutorialFinal)
        {
            return;
        }

        if (m_startingString >= m_endingString)
        {
            GetComponent<Canvas>().enabled = false;
            GameManager.instance.tutorialOpen = false;
            if (!m_playerGivenPiece)
            {
                m_playerGivenPiece = true;
                m_player.StartPiece();                
            }
            return;
        }

        m_textMeshPro.text = m_tutorialStrings[++m_startingString];
    }

    public void StartTutorial(int StartingLine, int EndingLine)
    {
        m_startingString = StartingLine;
        m_endingString = EndingLine;
        m_textMeshPro.text = m_tutorialStrings[m_startingString];
        GameManager.instance.tutorialOpen = true;
        GetComponent<Canvas>().enabled = true;

    }

    public void StartTutorialFirstPiecePlaced()
    {
        StartTutorial(m_startTutorialPiecePlaced, m_endTutorialPiecePlaced);
    }
    public void ShowCraftTutorial()
    {
        StartTutorial(m_startTutorialCrafting, m_endTutorialCrafting);
    }
    public void ShowMapExtensionTutorial()
    {
        StartTutorial(m_startTutorialMapExtensions, m_endTutorialMapExtensions);
    }
    public void ShowTimeBuffTutorial()
    {
        StartTutorial(m_startTutorialTimeBuffs, m_endTutorialTimeBuffs);
    }
    public void ShowBiomeBuffTutorial()
    {
        StartTutorial(m_startTutorialBiomeBuffs, m_endTutorialBiomeBuffs);
    }
    public void ShowTrashTutorial()
    {
        StartTutorial(m_startTutorialTrash, m_endTutorialTrash);
    }
    public void ShowFinalTutorial()
    {
        StartTutorial(m_startTutorialFinalExtension, m_endTutorialFinalExtension);
    }
    public void ShowFinalCraftTutorial()
    {
        StartTutorial(m_startTutorialFinalExtensionCrafted, m_endTutorialFinalExtensionCrafted);
    }
    public void ShowFinalFinalTutorial()
    {
        StartTutorial(m_startTutorialFinal, m_endTutorialFinal);
    }
}
