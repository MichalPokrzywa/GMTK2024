using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.ParticleSystem;
using UnityEngine.Rendering;
using UnityEditor;
using System.Drawing;
using UnityEditor.UIElements;

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

    public int damage;
    public DamageType damageType = DamageType.Medium;
    [SerializeField, Range(1.5f, 10.5f)]
    public float range = 1.5f;
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


    public override void GameUpdate()
    {
        if (!isShooting)
        {
            if (timeFromlastAttack >= attackSpeed)
                AcquireTarget();
            else
                timeFromlastAttack += Time.deltaTime;
        }
    }

    public bool TierUp()
    {
        if (tier < 3)
        {
            tier++;
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
        Gizmos.color = UnityEngine.Color.yellow;
        Vector3 position = transform.localPosition;
        position.y += 0.01f;
        Gizmos.DrawWireSphere(position, range);
    }

    bool AcquireTarget()
    {
        Collider[] targets = Physics.OverlapSphere(
            transform.localPosition, range
        );

        //DEBUGOWY
        attackSpeed = 0.5f;

        if (targets.Length > 0)
        {
            switch (towerName)
            {
                case "Mage":
                    //try to fing uncursed target
                    for (int i = 0; i < targets.Length; i++)
                    {
                        target = targets[i].GetComponent<TargetPoint>();
                        if (target == null) continue;
                        if (!target.Enemy.getCurse())
                        {
                            //invoke attack instead of debug
                            Debug.Log("Targeted worked on uncursed: " + targets[i]);
                            UnityEngine.Color color = new UnityEngine.Color(0.3f, 0.6f, 1.0f);
                            Debug.DrawLine(gameObject.transform.position, target.transform.position, UnityEngine.Color.red, 0.5f);
                            timeFromlastAttack = 0;
                            target.Enemy.setCurse(4, 0.5f);
                            return true;
                        }
                    }
                    //attack random cursed target
                    for (int i = 0; i < targets.Length; i++)
                    {
                        target = targets[i].GetComponent<TargetPoint>();
                        if (target == null) continue;
                        Debug.Log("Targeted worked on: " + targets[i]);
                        Debug.DrawLine(gameObject.transform.position, target.transform.position, UnityEngine.Color.blue, 0.5f);
                        return true;
                    }
                    break;
                case "Bowman":
                    //find minimum (min przed przecinkiem, max po)
                    int min = ((int) range) + 1; ;
                    float max = 0.0f;
                    for (int i = 0; i < targets.Length; i++)
                    {
                        target = targets[i].GetComponent<TargetPoint>();
                        if (target == null) continue;   
                        float progress = target.Enemy.getProgress();
                        int progressInt = (int) progress;
                        if (min > progressInt) 
                            min = progressInt;
                        if (max < progress - progressInt)
                            max = progress - progressInt;                       
                    }
                    for (int i = 0; i < targets.Length; i++)
                    {
                        target = targets[i].GetComponent<TargetPoint>();
                        if (target == null) continue;
                        float progress = target.Enemy.getProgress();
                        int progressInt = (int)progress;
                        if (min == progressInt && max == progress - progressInt)
                        {
                           Debug.DrawLine(gameObject.transform.position, target.transform.position, UnityEngine.Color.blue, 0.5f);
                           return true;
                        }
                    }
                    break;
                case "Mortal":
                    int maxNumberOfTargets = 0;
                    for (int i = 0; i < targets.Length; i++)
                    {
                        target = targets[i].GetComponent<TargetPoint>();
                        if (target == null) continue;
                        //create small colider around target
                        //count colliders
                        //find maxCount
                    }
                    break;
            }
            return true;
        }
        target = null;
        return false;
    }
    
}
