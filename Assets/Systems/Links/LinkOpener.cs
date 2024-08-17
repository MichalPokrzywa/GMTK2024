using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkOpener
{
    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }
}