using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowCraftingUI : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private GameObject m_craftingUI;
    [SerializeField]
    private List<GameObject> m_otherCraftingUI;
    [SerializeField]
    private Player m_player;

    private bool m_tutorialShown = false;
    #endregion

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(ToggleUIVisibility);
    }

    public void ToggleUIVisibility()
    {
        if (!GameManager.instance.tutorialOpen)
        {
            m_craftingUI.GetComponent<Canvas>().enabled = !m_craftingUI.GetComponent<Canvas>().enabled;

            m_player.canPlacePiece = !m_craftingUI.GetComponent<Canvas>().enabled;

            for (int i = 0; i < m_otherCraftingUI.Count; i++)
            {
                m_otherCraftingUI[i].GetComponent<Canvas>().enabled = false;
            }


            if(!m_tutorialShown)
            {
                m_tutorialShown = true;

                if (m_craftingUI.gameObject.name == "CraftingCanvas")
                {
                    Tutorial.instance.ShowCraftTutorial();
                }
                else if (m_craftingUI.gameObject.name == "MapCraftingCanvas")
                {
                    Tutorial.instance.ShowMapExtensionTutorial();
                }
                else if (m_craftingUI.gameObject.name == "TimeBuffCanvas")
                {
                    Tutorial.instance.ShowTimeBuffTutorial();
                }
                else if (m_craftingUI.gameObject.name == "BiomeBuffCanvas")
                {
                    Tutorial.instance.ShowBiomeBuffTutorial();
                }
            }
        }
       
    }
}
