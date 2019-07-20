using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Where all the prefabs and everything and all the materials will be stored.
/// </summary>
public class MaterialList : ScriptableObject
{
    public static Material[] SpawnMaterials;
    public static Material[] KiwiMaterials;

    public Material[] _SpawnMaterials;
    public Material[] _KiwiMaterials;

    [MenuItem("Assets/Create/MaterialList")]
    public static void Create()
    {
        MaterialList list = ScriptableObject.CreateInstance<MaterialList>();

        AssetDatabase.CreateAsset(list, "Assets/Resources/MaterialList.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = list;
    }

    public static MaterialList mlist;

    public static void Load()
    {
        mlist = Resources.Load<MaterialList>("MaterialList.asset");
        SpawnMaterials = mlist._SpawnMaterials;
        KiwiMaterials = mlist._KiwiMaterials;
    }
}
