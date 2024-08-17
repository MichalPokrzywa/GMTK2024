using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection<ModelType> : ScriptableObject, IEnumerable, IEnumerable<ModelType>
{
    public List<ModelType> elements;

    public IEnumerator GetEnumerator()
    {
        return elements.GetEnumerator();
    }

    IEnumerator<ModelType> IEnumerable<ModelType>.GetEnumerator()
    {
        return elements.GetEnumerator();
    }
}
