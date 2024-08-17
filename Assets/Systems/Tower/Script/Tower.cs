using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

[Serializable]
public enum DamageType
{
    Light,
    Medium,
    Heavy
}

[Serializable]
public struct BonusStats
{
    public int attackProjectalsCount;
    public float attackSize;
    public float curseDuration;
    public float cursePower;
}


public class Tower : MonoBehaviour
{

    public Transform towerVisual;
    public string towerName;
    public int tier;

    public float sizeXZ = 1;
    public float sizeY = 1;


    public int damage;
    public DamageType damageType = DamageType.Medium;
    public float range;
    public float attackSpeed = 1;
    private float timeFromlastAttack;
    private bool isShooting = false;

    public int attackProjectalsCount = 1;
    public float attackSize = 0;
    public float curseDuration = 0;
    public float cursePower = 0;

    public BonusStats supportStats;
    public List<Tower> supporters = new List<Tower>();
    public Tower supportedTower;

    //Public Enemy Target

    public void Aim()
    {
        //Find Target
        //if true Shot
        //else to nic
    }


    public void Support(Tower towerToSuport)
    {
        if (!CanSupport()) return;
        
        if(!towerToSuport.AddSupporter(this)) return;

        supportedTower = towerToSuport;
        UpdateStats();
    }

    public void UnSupport()
    {
        if (supportedTower != null)
        {
            supportedTower.RemoveSupporter(this);
            supportedTower = null;
            UpdateStats();
        }
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
            return true;
        return false;
    }

    private bool CanBeSupported()
    {
        if (supporters.Count == 1 + ((tier - 1) * 2) || supportedTower != null)
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

            if(supportersCount>0) damageType = DamageType.Heavy;
            else damageType = DamageType.Medium;
        }
        else
        {
            damageType = DamageType.Light;
            sizeXZ = 0.7f;
            sizeY = 0.7f;
        }

        towerVisual.transform.localScale = new Vector3(sizeXZ, sizeY, sizeXZ);

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
