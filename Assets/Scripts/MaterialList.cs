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
    public static Material[] ConstructionKiwiMaterials;

    public Material[] _SpawnMaterials;
    public Material[] _ConstructionKiwiMaterials;

    public static GameObject ConstructionKiwiPrefab;
    public GameObject _ConstructionKiwiPrefab;

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
        mlist = Resources.Load<MaterialList>("MaterialList");
        SpawnMaterials = mlist._SpawnMaterials;
        ConstructionKiwiMaterials = mlist._ConstructionKiwiMaterials;
        ConstructionKiwiPrefab = mlist._ConstructionKiwiPrefab;
    }
}
