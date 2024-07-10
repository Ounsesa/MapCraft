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


public class ResourcesManager : Manager
{    
    public Inventory Inventory;
    public static int GetEnumLength<T>() where T : System.Enum
    {
        return System.Enum.GetValues(typeof(T)).Length;
    }


    
}
