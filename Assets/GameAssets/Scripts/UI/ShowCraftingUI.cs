using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowCraftingUI : MonoBehaviour
{
    public GameObject CraftingUI;
    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(ToggleUIVisibility);
    }

    // Update is called once per frame
    void ToggleUIVisibility()
    {
        CraftingUI.SetActive(!CraftingUI.activeSelf);
    }
}
