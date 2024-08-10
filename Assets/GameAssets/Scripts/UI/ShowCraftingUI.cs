using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowCraftingUI : MonoBehaviour
{
    public GameObject CraftingUI;
    public List<GameObject> OtherCraftingUI;
    public Player player;

    private bool TutorialShown = false;
    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(ToggleUIVisibility);
    }

    // Update is called once per frame
    public void ToggleUIVisibility()
    {
        if (!GameManager.Instance.TutorialOpen)
        {
            CraftingUI.GetComponent<Canvas>().enabled = !CraftingUI.GetComponent<Canvas>().enabled;

            player.CanPlacePiece = !CraftingUI.GetComponent<Canvas>().enabled;

            for (int i = 0; i < OtherCraftingUI.Count; i++)
            {
                OtherCraftingUI[i].GetComponent<Canvas>().enabled = false;
            }


            if(!TutorialShown)
            {
                TutorialShown = true;

                if (CraftingUI.gameObject.name == "CraftingCanvas")
                {
                    Tutorial.Instance.ShowCraftTutorial();
                }
                else if (CraftingUI.gameObject.name == "MapCraftingCanvas")
                {
                    Tutorial.Instance.ShowMapExtensionTutorial();
                }
                else if (CraftingUI.gameObject.name == "TimeBuffCanvas")
                {
                    Tutorial.Instance.ShowTimeBuffTutorial();
                }
                else if (CraftingUI.gameObject.name == "BiomeBuffCanvas")
                {
                    Tutorial.Instance.ShowBiomeBuffTutorial();
                }
            }
        }
       
    }
}
