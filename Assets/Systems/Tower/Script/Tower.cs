using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public enum DamageType
{
    Light,
    Medium,
    Heavy
}

public struct BonusStats
{
    public int attackProjectalsCount;
    public float attackSize;
    public float curseDuration;
    public float cursePower;
}


public class Tower : MonoBehaviour
{

    public string towerName;
    public int tier;

    public float sizeXZ = 1;
    public float sizeY = 1;


    public int damage;
    public DamageType damegeType = DamageType.Medium;
    public float range;
    public float attackSpeed = 1;
    private float timeFromlastAttack;
    private bool isShooting = false;

    public int attackProjectalsCount = 1;
    public float attackSize = 0;
    public float curseDuration = 0;
    public float cursePower = 0;

    public BonusStats supportStats;
    public List<Tower> supporters;
    public Tower supportedTower;

    //Public Enemy Target

    public void Aim()
    {
        //Find Target
        //if true Shot
        //else check Aim
    }


    public void Support(Tower towerToSuport)
    {
        if (!CanSupport()) return;
        
        if(!supportedTower.AddSupporter(towerToSuport)) return;

        supportedTower = towerToSuport;
        UpdateStats();
    }

    public void UnSupport()
    {
        supportedTower.RemoveSupporter(this);
        supportedTower = null;
        UpdateStats();
    }

    public bool AddSupporter(Tower tower)
    {
        //Czy mo¿emy byæ supportowani
        if (!CanBeSupported()) return false;

        AddBonusStats(tower.supportStats);
        supporters.Add(tower);
        UpdateStats();

        return true;

    }

    public void RemoveSupporter(Tower tower)
    {
        RemoveBonusStats(tower.supportStats);
        supporters.Remove(tower);
        UpdateStats();
    }

    private bool CanSupport()
    {
        if (supporters.Count == 0 && supportedTower == null)
            return false;
        return true;
    }

    private bool CanBeSupported()
    {
        if (supporters.Count == 1 + ((tier - 1) * 2) && supportedTower == null)
            return false;
        return true;
    }

    private void UpdateStats()
    {
        if(supportedTower == null)
        {
            int supportersCount = supporters.Count;
            sizeXZ = 1 + (0.06f * supportersCount);
            sizeY = 1 + (0.12f * supportersCount);

            if(supportersCount>0) damegeType = DamageType.Heavy;
            else damegeType = DamageType.Medium;
        }
        else
        {
            damegeType = DamageType.Light;
            sizeXZ = 0.7f;
            sizeY = 0.7f;
        }
    }

    public void AddBonusStats(BonusStats stats)
    {
        attackProjectalsCount += stats.attackProjectalsCount;
        attackSize += stats.attackSize;
        curseDuration += stats.curseDuration;
        cursePower += stats.cursePower;
    }

    public void RemoveBonusStats(BonusStats stats)
    {
        attackProjectalsCount -= stats.attackProjectalsCount;
        attackSize -= stats.attackSize;
        curseDuration -= stats.curseDuration;
        cursePower -= stats.cursePower;
    }


    public void Update()
    {
        if(!isShooting)
        {
            if (timeFromlastAttack >= attackSpeed)
            {
                Aim();

            }
            else
                timeFromlastAttack += Time.deltaTime;
            
        }
        
    }

}
