using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BiomeType
{
    Forest,     //0
    Desert,     //1
    Mountain,   //2
    Plains      //3
}

enum ResourceType
{
    Wood,       //0
    Sand,       //1
    Stone,      //2
    Food        //3
}

enum MaterialType
{
    Fiber,      //0
    Clay,       //1
    Iron,       //2
    Leather     //3
}

public enum PieceType
{
    Resource,   //0 //1 * BiomeType.Num + BiomeType
    Material    //1 //2 * BiomeType.Num + BiomeType
}

public class ResourcesManager : Manager
{    
    public Inventory Inventory;
    public static int GetEnumLength<T>() where T : System.Enum
    {
        return System.Enum.GetValues(typeof(T)).Length;
    }


    public void ParseIntToResource(int value)
    {
        if(value < 0)
        {
            Debug.Log("Invalid tile");
            return;
        }
        int biomeCount = GetEnumLength<BiomeType>();
        int EnumValue = value / biomeCount;  
        
        switch (EnumValue)
        {
            case 0:
                BiomeType biome = (BiomeType)(value % biomeCount);
                //Debug.Log($"Resource: {biome}");
                break;
            case 1:
                ResourceType resource = (ResourceType)(value % biomeCount);
                Inventory.AddResource((value % biomeCount), 1);
                Debug.Log($"Resource: {resource}");
                break;
            case 2:
                MaterialType material = (MaterialType)(value % biomeCount);
                Inventory.AddMaterial((value % biomeCount), 1);
                Debug.Log($"Material: {material}");
                break;
        }
    }
}
