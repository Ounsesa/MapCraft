using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftCostsController : MonoBehaviour
{
    #region Variables
    public static CraftCostsController instance;
    public Dictionary<CraftType, CraftCost> craftingInitialCosts;

    [SerializeField]
    private string m_craftCostsPath = "CraftingCosts";
    #endregion

    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        instance = this; 
        
        
        craftingInitialCosts = CSVParser.ParseCSVToDictionary(m_craftCostsPath);
    }


}
