using System.Collections.Generic;
using UnityEngine;

public abstract class ListView<ViewType, ModelType, CollectionType> : MonoBehaviour 
    where ViewType : View<ModelType> 
    where CollectionType : Collection<ModelType>
{
    public CollectionType collection;
    public ViewType viewPrefab;

    private List<ViewType> views = new List<ViewType>();

    private void Start()
    {
        CreateViewElements();
    }

    private void OnEnable()
    {
        RefreshViewElements();
    }

    public void CreateViewElements()
    {
        foreach (var item in collection.elements)
        {
            ViewType view = Instantiate(viewPrefab, transform);
            view.SetViewData(item);

            views.Add(view);
        }
    }

    public void RefreshViewElements()
    {
        if (views.Count == 0) return;

        foreach (var view in views)
        {
            view.SetViewData(view.Model);
        }
    }
}
