using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private BuffsController m_buffsController;

    private Dictionary<ResourceType, int> m_resourcesList = new Dictionary<ResourceType, int>();
    private Dictionary<MaterialType, int> m_materialsList = new Dictionary<MaterialType, int>();
    private Dictionary<PieceType, int> m_assetsTileList = new Dictionary<PieceType, int>();
    private Dictionary<BiomeType, int> m_mapExtensionTileList = new Dictionary<BiomeType, int>();

    private int m_finalExtensionTile = 0;

    //Piece type [Resource/Material], int [id], int [amount]
    public event System.Action<PieceType, int, int> onItemAmountChanged;
    //Piece type [Resource/Material], int [id]
    public event System.Action<PieceType, int> onAssetTileAmountChanged;
    //Piece type [Resource/Material], int [id]
    public event System.Action<BiomeType, int> onMapExtensionTileAmountChanged;
    //Final extension Tile
    public event System.Action<int> onFinalExtensionTileAmountChanged;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < ResourcesManager.GetEnumLength<ResourceType>(); i++)
        {
            m_resourcesList.Add((ResourceType)i, 0);
        }

        for(int i = 0; i < ResourcesManager.GetEnumLength<MaterialType>(); i++)
        {
            m_materialsList.Add((MaterialType)i, 0);
        }

        m_assetsTileList.Add(PieceType.Resource, 0);
        m_assetsTileList.Add(PieceType.Material, 0);

        for (int i = 0; i < ResourcesManager.GetEnumLength<BiomeType>(); i++)
        {
            m_mapExtensionTileList.Add((BiomeType)i, 0);
        }
    }

    public void AddResource(int resource, int amount)
    {
        m_resourcesList[(ResourceType)resource] += Mathf.FloorToInt(amount * m_buffsController.biomeMultiplier[(BiomeType)resource]);
        onItemAmountChanged?.Invoke(PieceType.Resource, resource, m_resourcesList[(ResourceType)resource]);
    }

    public void AddMaterial(int material, int amount)
    {
        m_materialsList[(MaterialType)material] += Mathf.FloorToInt(amount * m_buffsController.biomeMultiplier[(BiomeType)material]);
        onItemAmountChanged?.Invoke(PieceType.Material, material, m_materialsList[(MaterialType)material]);
    }

    public void AddAssetTile(PieceType pieceType, int amount)
    {
        m_assetsTileList[pieceType] += amount;
        onAssetTileAmountChanged?.Invoke(pieceType, m_assetsTileList[pieceType]);
    }
    public void RemoveAssetTile(PieceType pieceType, int amount)
    {
        m_assetsTileList[pieceType] -= amount;
        onAssetTileAmountChanged?.Invoke(pieceType, m_assetsTileList[pieceType]);
    }


    public int GetAssetTileAmount(PieceType pieceType) 
    {
        return m_assetsTileList[pieceType];
    }
    public void RemoveResource(int resource, int amount)
    {
        m_resourcesList[(ResourceType)resource] -= amount;
        onItemAmountChanged?.Invoke(PieceType.Resource, resource, m_resourcesList[(ResourceType)resource]);
    }

    public void RemoveMaterial(int material, int amount)
    {
        m_materialsList[(MaterialType)material] -= amount;
        onItemAmountChanged?.Invoke(PieceType.Material, material, m_materialsList[(MaterialType)material]);
    }


    public void AddMapExtensionTile(BiomeType type, int amount)
    {
        m_mapExtensionTileList[type] += amount;
        onMapExtensionTileAmountChanged?.Invoke(type, m_mapExtensionTileList[type]);
    }
    public void RemoveMapExtensionTile(BiomeType type, int amount)
    {
        m_mapExtensionTileList[type] -= amount;
        onMapExtensionTileAmountChanged?.Invoke(type, m_mapExtensionTileList[type]);
    }
    public int GetMapExtensionTile(BiomeType type)
    {
        return m_mapExtensionTileList[type];
    }

    public void PrintInventory()
    {
        foreach(KeyValuePair<ResourceType, int> resource in m_resourcesList)
        {
            Debug.Log($"{resource.Key}: {resource.Value}");
        }

        foreach(KeyValuePair<MaterialType, int> material in m_materialsList)
        {
            Debug.Log($"{material.Key}: {material.Value}");
        }
    }


    public void AddFinalExtensionTile()
    {
        m_finalExtensionTile += 1;
        onFinalExtensionTileAmountChanged?.Invoke(m_finalExtensionTile);
    }
    public void RemoveFinalExtensionTile()
    {
        m_finalExtensionTile = 0;
        onFinalExtensionTileAmountChanged?.Invoke(m_finalExtensionTile);
    }
}
