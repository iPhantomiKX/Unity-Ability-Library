using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class RightClickCreateSO
{
    [MenuItem("Assets/Create/Asset from ScriptableObject", priority = -100)]
    private static void Create()
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        path = path.Substring(0, path.LastIndexOf("/"));
        Type type = ((MonoScript)Selection.activeObject).GetClass();
        ScriptableObject asset = ScriptableObject.CreateInstance(type);
        AssetDatabase.CreateAsset(asset, path + "/" + type.Name.ToString() + ".asset");
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/Create/Asset from ScriptableObject", true)]
    private static bool CreateValidation()
    {
        if (Selection.activeObject == null)
            return false;
        if (Selection.activeObject.GetType() != typeof(MonoScript))
            return false;

        System.Type type = ((MonoScript)Selection.activeObject).GetClass();
        if (type == null)
            return false;
        if (!typeof(ScriptableObject).IsAssignableFrom(type))
            return false;
        if (type.IsAbstract)
            return false;
        return true;
    }
}
