using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftCostsController : MonoBehaviour
{
    public static CraftCostsController Instance;

    public Dictionary<CraftType, CraftCost> CraftingInitialCosts;

    public string CraftCostsPath = "CraftingCosts.csv";
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        Instance = this; 
        
        
        CraftingInitialCosts = CSVParser.ParseCSVToDictionary(GameManager.Instance.GameDataPath + CraftCostsPath);


    }


}
