using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ResourcesManager ResourcesManager;

    Dictionary<ResourceType, int> ResourcesList = new Dictionary<ResourceType, int>();
    Dictionary<MaterialType, int> MaterialsList = new Dictionary<MaterialType, int>();


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
    }

    public void AddResource(int resource, int amount)
    {
        ResourcesList[(ResourceType)resource] += amount;
    }

    public void AddMaterial(int material, int amount)
    {
        MaterialsList[(MaterialType)material] += amount;
    }

    public void RemoveResource(int resource, int amount)
    {
        ResourcesList[(ResourceType)resource] -= amount;
    }

    public void RemoveMaterial(int material, int amount)
    {
        MaterialsList[(MaterialType)material] -= amount;
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
