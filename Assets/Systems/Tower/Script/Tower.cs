using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public enum DamageType
{
    Light,
    Medium,
    Heavy
}

[Serializable]
public struct BasicStats
{
    public int damage;
    public float range;
    public float attackSpeed;
    public BonusStats bonusStats;
}


[Serializable]
public struct BonusStats
{
    public int attackProjectalsCount;
    public float attackSize;
    public float curseDuration;
    public float cursePower;
}


public class Tower : GameTileContent
{
    public UnityEvent<Tower> onSupport;
    public UnityEvent<Tower> onUnsupport;

    TargetPoint target;

    public Transform towerVisual;
    public string towerName;
    public int tier;

    public float sizeXZ = 1;
    public float sizeY = 1;

    public int tileId;

    //Basic Stats
    public int damage;
    [SerializeField, Range(1.5f, 10.5f)]
    public float range = 1.5f;
    public float attackSpeed = 1;
    public BasicStats levelUpStats;

    public DamageType damageType = DamageType.Medium;
    private float timeFromlastAttack;
    private bool isShooting = false;

   

    //Specyfic Stats
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

        onSupport.Invoke(towerToSuport);
        supportedTower = towerToSuport;
        UpdateStats();
    }

    public void UnSupport()
    {
        if (supportedTower != null)
        {
            onUnsupport.Invoke(supportedTower);
            supportedTower.RemoveSupporter(this);
            supportedTower = null;
            UpdateStats();
        }
    }

    public bool AddSupporter(Tower tower)
    {
        //Czy mo�emy by� supportowani
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

    public void AddStats(BasicStats stats)
    {
        damage += stats.damage;
        range += stats.range;
        attackSpeed += stats.attackSpeed;
        attackProjectalsCount += stats.bonusStats.attackProjectalsCount;
        attackSize += stats.bonusStats.attackSize;
        curseDuration += stats.bonusStats.curseDuration;
        cursePower += stats.bonusStats.cursePower;
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


    public override void GameUpdate()
    {
        if (AcquireTarget())
        {
            Debug.Log("Acquired target!");
        }
        if (!isShooting)
        {
            if (timeFromlastAttack >= attackSpeed)
            {
                Aim();
                
            }
            else
                timeFromlastAttack += Time.deltaTime;
            
        }
        
    }

    public bool TierUp()
    {
        if (tier < 3)
        {
            tier++;
            AddStats(levelUpStats);
            return true;
        }
        return false;
    }

    public int Sell()
    {
        UnSupport();
        foreach(var i in supporters)
        {
            i.UnSupport();
        }
        //usun wierzyczke, przywroc tile
        return 100;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 position = transform.localPosition;
        position.y += 0.01f;
        Gizmos.DrawWireSphere(position, range);
    }

    bool AcquireTarget()
    {
        Collider[] targets = Physics.OverlapSphere(
            transform.localPosition, range
        );
        if (targets.Length > 0)
        {
            for (int i = 0; i< targets.Length; i++)
            {
                target = targets[i].GetComponent<TargetPoint>();
                if(target != null) break;
                Debug.Log("Targeted not worked on: " + targets[i]);
            }
            Debug.Log(target == null ? "Targeted non-enemy!" : target);
            return true;
        }
        target = null;
        return false;
    }
}
