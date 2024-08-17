using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    Light,
    Medium,
    Heavy
}


public abstract class Tower : MonoBehaviour
{

    public string towerName;

    public float sizeXZ = 1;
    public float sizeY = 1;

    public int damage;
    public DamageType damegeType = DamageType.Medium;
    public float range;
    public int attackProjectalsCount = 1;
    public float attackSpeed = 0;
    public float attackSize = 0;
    public float curseDuration = 0;
    public float cursePower = 0;

    public List<Tower> supporters;
    public Tower supportedTower;

    public void Support(Tower towerToSuport)
    {
        supportedTower = towerToSuport;
        supportedTower.AddSupporter(towerToSuport);
        SetSmallStats();
    }

    private void SetSmallStats()
    {
        damegeType = DamageType.Light;
        sizeXZ = 0.7f;
        sizeY = 0.7f;
    }

    public void UnSupport()
    {
        supportedTower.RemoveSupporter(this);
        supportedTower = null;
        ResetStats();
    }

    private void ResetStats()
    {
        damegeType = DamageType.Medium;
        sizeXZ = 1;
        sizeY = 1;
    }

    public abstract void Aim();

    public abstract void AddSupporter(Tower tower);
    public abstract void RemoveSupporter(Tower tower);




}
