using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class View<ModelType> : MonoBehaviour
{
    private ModelType model;
    public ModelType Model => model;

    public virtual void SetViewData(ModelType data)
    {
        model = data;
    }
}
