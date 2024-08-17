using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public List<Effect> effects;
    private List<GameObject> usedEffects = new List<GameObject>();

    public void ActivateEffect(EffectType effect, Vector3 position)
    {
        GameObject effectObject = null;

        foreach (var item in effects)
        {
            if (item.type == effect)
            {
                effectObject = Instantiate(item.effectPrefab);
                usedEffects.Add(effectObject);
                break;
            }
        }

        if (effectObject != null)
        {
            effectObject.transform.position = position;
        }
    }

    public void ClearUsedEffects()
    {
        if (usedEffects.Count == 0) return;

        for (int i = 0; i < usedEffects.Count; i++)
        {
            Destroy(usedEffects[i]);
        }

        usedEffects.Clear();
    }    
}


public enum EffectType
{
    Respawn = 0,
    Death = 1,
}
