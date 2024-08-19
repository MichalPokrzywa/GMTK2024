using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingEnemy : MonoBehaviour
{
    [SerializeField] private Collider healingRange;
    private bool cooldown = false;
    [SerializeField] private float range = 1.5f;
    TargetPoint target;

    void Update()
    {
        if (!cooldown)
        {
            StartCoroutine(WaitForHeal());     
            cooldown = true;
        }
    }

    IEnumerator WaitForHeal() {
        yield return new WaitForSeconds(4f);
        Healing();
        cooldown = false;
    }
    private void Healing()
    {

    }

    bool AcquireTarget()
    {
        Collider[] targets = Physics.OverlapSphere(
            transform.localPosition, range
        );
        for ( int i = 0;i<targets.Length;i++)
        {
            target = targets[i].GetComponent<TargetPoint>();
            if( target == null ) continue;
            //target.Enemy.
            return true;
        }
        target = null;
        return false;
    }
}
