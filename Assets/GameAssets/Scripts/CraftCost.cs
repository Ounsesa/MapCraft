using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CraftType
{
    Piece,
    MapExtension,
    AmountBuff,
    TimeBuff
}

public class CraftCost
{
    //How many resources are needed for the craft
    public int CurrentCost;

    //How much will increase the cost for the next craft. CurrentCost *= NextMultiplierCostIncrease
    public float NextMultiplierCost;

    //The maximum value that the multiplier will cost. Don't think anything bigger than 3. It's exponential
    public float MaxMultiplierCost;

    //In how many crafts the multiplier will reach the max multiplier
    public int MultiplierSubdivisions;

    private float MultiplierIncrease;


    public int GetCurrentCost()
    {
        int AuxCost = CurrentCost;
        CurrentCost = Mathf.FloorToInt(CurrentCost * NextMultiplierCost);
        NextMultiplierCost += MultiplierIncrease;
        return AuxCost;
    }
    public CraftCost Clone()
    {
        return new CraftCost
        {
            CurrentCost = this.CurrentCost,
            NextMultiplierCost = this.NextMultiplierCost,
            MaxMultiplierCost = this.MaxMultiplierCost,
            MultiplierSubdivisions = this.MultiplierSubdivisions,
            MultiplierIncrease = (MaxMultiplierCost - 1) / MultiplierSubdivisions
        };
    }
}
