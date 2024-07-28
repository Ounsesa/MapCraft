using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ResourcesManager ResourcesManager;
    public BuffsController BuffsController;

    Dictionary<ResourceType, int> ResourcesList = new Dictionary<ResourceType, int>();
    Dictionary<MaterialType, int> MaterialsList = new Dictionary<MaterialType, int>();
    Dictionary<PieceType, int> AssetsTileList = new Dictionary<PieceType, int>();

    Dictionary<BiomeType, int> MapExtensionTileList = new Dictionary<BiomeType, int>();

    //Piece type [Resource/Material], int [id], int [amount]
    public event System.Action<PieceType, int, int> OnItemAmountChanged;
    //Piece type [Resource/Material], int [id]
    public event System.Action<PieceType, int> OnAssetTileAmountChanged;
    //Piece type [Resource/Material], int [id]
    public event System.Action<BiomeType, int> OnMapExtensionTileAmountChanged;


    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < ResourcesManager.GetEnumLength<ResourceType>(); i++)
        {
            ResourcesList.Add((ResourceType)i, 0);
        }

        for(int i = 0; i < ResourcesManager.GetEnumLength<MaterialType>(); i++)
        {
            MaterialsList.Add((MaterialType)i, 0);
        }

        AssetsTileList.Add(PieceType.Resource, 0);
        AssetsTileList.Add(PieceType.Material, 0);

        for (int i = 0; i < ResourcesManager.GetEnumLength<BiomeType>(); i++)
        {
            MapExtensionTileList.Add((BiomeType)i, 0);
        }
    }

    public void AddResource(int resource, int amount)
    {
        ResourcesList[(ResourceType)resource] += Mathf.FloorToInt(amount * BuffsController.BiomeMultiplier[(BiomeType)resource]);
        OnItemAmountChanged?.Invoke(PieceType.Resource, resource, ResourcesList[(ResourceType)resource]);
    }

    public void AddMaterial(int material, int amount)
    {
        MaterialsList[(MaterialType)material] += Mathf.FloorToInt(amount * BuffsController.BiomeMultiplier[(BiomeType)material]);
        OnItemAmountChanged?.Invoke(PieceType.Material, material, MaterialsList[(MaterialType)material]);
    }

    public void AddAssetTile(PieceType pieceType, int amount)
    {
        AssetsTileList[pieceType] += amount;
        OnAssetTileAmountChanged?.Invoke(pieceType, AssetsTileList[pieceType]);
    }
    public void RemoveAssetTile(PieceType pieceType, int amount)
    {
        AssetsTileList[pieceType] -= amount;
        OnAssetTileAmountChanged?.Invoke(pieceType, AssetsTileList[pieceType]);
    }


    public int GetAssetTileAmount(PieceType pieceType) 
    {
        return AssetsTileList[pieceType];
    }
    public void RemoveResource(int resource, int amount)
    {
        ResourcesList[(ResourceType)resource] -= amount;
        OnItemAmountChanged?.Invoke(PieceType.Resource, resource, ResourcesList[(ResourceType)resource]);
    }

    public void RemoveMaterial(int material, int amount)
    {
        MaterialsList[(MaterialType)material] -= amount;
        OnItemAmountChanged?.Invoke(PieceType.Material, material, MaterialsList[(MaterialType)material]);
    }


    public void AddMapExtensionTile(BiomeType type, int amount)
    {
        MapExtensionTileList[type] += amount;
        OnMapExtensionTileAmountChanged?.Invoke(type, MapExtensionTileList[type]);
    }
    public void RemoveMapExtensionTile(BiomeType type, int amount)
    {
        MapExtensionTileList[type] -= amount;
        OnMapExtensionTileAmountChanged?.Invoke(type, MapExtensionTileList[type]);
    }
    public int GetMapExtensionTile(BiomeType type)
    {
        return MapExtensionTileList[type];
    }

    public void PrintInventory()
    {
        foreach(KeyValuePair<ResourceType, int> resource in ResourcesList)
        {
            Debug.Log($"{resource.Key}: {resource.Value}");
        }

        foreach(KeyValuePair<MaterialType, int> material in MaterialsList)
        {
            Debug.Log($"{material.Key}: {material.Value}");
        }
    }
}
