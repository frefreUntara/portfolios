using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ResourcesRefresher : Editor
{
    static public void ResourcesLoader()
    {
        AssetDatabase.Refresh();
    }
}
