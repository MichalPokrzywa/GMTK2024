using System.Collections.Generic;
using UnityEngine;

public class ScrollContentAdjuster : MonoBehaviour
{
    public List<RectTransform> contents;

    private void OnDisable()
    {
        foreach (var content in contents)
        {
            content.anchoredPosition = Vector2.zero;
        }
    }
}
