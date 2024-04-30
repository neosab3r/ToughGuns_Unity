using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelDataModel
{
    public int LevelIndex { get; private set; }
    public List<WeaponData> WeaponDatas { get; private set; }

    public LevelDataModel(int index, List<PreLevelWeaponModel> weapons, List<int> deathWeaponsIndex)
    {
        LevelIndex = index;

        WeaponDatas = new List<WeaponData>();
        
        foreach (var weapon in weapons)
        {
            if (deathWeaponsIndex.Contains(weapon.GetCachedCell().CellIndex))
            {
                continue;
            }
            
            WeaponData weaponData = new WeaponData(weapon.GetCachedCell().CellIndex, weapon.MergeType);
            WeaponDatas.Add(weaponData);
        }
    }
}

[Serializable]
public class WeaponData
{
    public int CellIndex { get; private set; }
    public EWeaponsType WeaponType { get; private set; }
    
    public WeaponData(int index, EWeaponsType type)
    {
        CellIndex = index;
        WeaponType = type;
    }
}