using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffsController : MonoBehaviour
{    
    public Dictionary<BiomeType, float> biomeMultiplier;

    private void Awake()
    {
        biomeMultiplier = new Dictionary<BiomeType, float>
        {
            { BiomeType.Forest, 1f },
            { BiomeType.Desert, 1f },
            { BiomeType.Mountain, 1f },
            { BiomeType.Plains, 1f }
        };
    }

    public void AddMultiplier(BiomeType biomeType, float amount)
    {
        biomeMultiplier[biomeType] += amount;
    }
}
