using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowCraftingUI : MonoBehaviour
{
    public GameObject CraftingUI;
    public List<GameObject> OtherCraftingUI;
    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(ToggleUIVisibility);
    }

    // Update is called once per frame
    public void ToggleUIVisibility()
    {
        CraftingUI.SetActive(!CraftingUI.activeSelf);

        for(int i = 0;  i < OtherCraftingUI.Count; i++) 
        {
            OtherCraftingUI[i].SetActive(false);
        }
    }
}
