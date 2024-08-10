using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public static Tutorial Instance;

    private List<string> TutorialStrings;
    [SerializeField]
    private int StartingString = 0;
    [SerializeField]
    private int EndingString = 5;


    [SerializeField]
    private int StartTutorialPiecePlaced = 6;
    [SerializeField]
    private int EndTutorialPiecePlaced = 9;
    private int StartTutorialCrafting = 10;
    private int EndTutorialCrafting = 20;
    private int StartTutorialTrash = 21;
    private int EndTutorialTrash = 23;
    private int StartTutorialMapExtensions = 24;
    private int EndTutorialMapExtensions = 30;
    private int StartTutorialTimeBuffs = 31;
    private int EndTutorialTimeBuffs = 33;
    private int StartTutorialBiomeBuffs = 34;
    private int EndTutorialBiomeBuffs = 36;
    private int StartTutorialFinalExtension = 37;
    private int EndTutorialFinalExtension = 39;
    private int StartTutorialFinalExtensionCrafted = 40;
    private int EndTutorialFinalExtensionCrafted = 41;
    private int StartTutorialFinal = 42;
    private int EndTutorialFinal = 43;

    [SerializeField]
    private GameObject TutorialText;
    private TextMeshProUGUI textMeshPro;

    private bool PlayerGivenPiece = false;
    private string TutorialFileName = "Tutorial.csv";

    [SerializeField]
    private Player Player;


    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        Instance = this;
        textMeshPro = TutorialText.GetComponent<TextMeshProUGUI>();
        GetComponent<Button>().onClick.AddListener(TutorialClicked);
    }

    private void Start()
    {
        TutorialStrings = CSVParser.ParseCSVToTutorial(GameManager.Instance.GameDataPath + TutorialFileName);

        StartTutorial(StartingString, EndingString);
    }


    void TutorialClicked()
    {

        if (StartingString >= EndingString)
        {
            GetComponent<Canvas>().enabled = false;
            GameManager.Instance.TutorialOpen = false;
            if (!PlayerGivenPiece)
            {
                PlayerGivenPiece = true;
                Player.StartPiece();                
            }
            return;
        }

        textMeshPro.text = TutorialStrings[++StartingString];
    }

    public void StartTutorial(int StartingLine, int EndingLine)
    {
        StartingString = StartingLine;
        EndingString = EndingLine;
        textMeshPro.text = TutorialStrings[StartingString];
        GameManager.Instance.TutorialOpen = true;
        GetComponent<Canvas>().enabled = true;

    }

    public void StartTutorialFirstPiecePlaced()
    {
        StartTutorial(StartTutorialPiecePlaced, EndTutorialPiecePlaced);
    }
    public void ShowCraftTutorial()
    {
        StartTutorial(StartTutorialCrafting, EndTutorialCrafting);
    }
    public void ShowMapExtensionTutorial()
    {
        StartTutorial(StartTutorialMapExtensions, EndTutorialMapExtensions);
    }
    public void ShowTimeBuffTutorial()
    {
        StartTutorial(StartTutorialTimeBuffs, EndTutorialTimeBuffs);
    }
    public void ShowBiomeBuffTutorial()
    {
        StartTutorial(StartTutorialBiomeBuffs, EndTutorialBiomeBuffs);
    }
    public void ShowTrashTutorial()
    {
        StartTutorial(StartTutorialTrash, EndTutorialTrash);
    }
    public void ShowFinalTutorial()
    {
        StartTutorial(StartTutorialFinalExtension, EndTutorialFinalExtension);
    }
    public void ShowFinalCraftTutorial()
    {
        StartTutorial(StartTutorialFinalExtensionCrafted, EndTutorialFinalExtensionCrafted);
    }
    public void ShowFinalFinalTutorial()
    {
        StartTutorial(StartTutorialFinal, EndTutorialFinal);
    }
}
