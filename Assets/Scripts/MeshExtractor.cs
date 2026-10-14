#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class MeshExtractor
{
    [MenuItem("Tools/Extract Mesh from Selected")]
    static void ExtractMesh()
    {
        GameObject selected = Selection.activeGameObject;
        if (selected == null)
        {
            Debug.LogError("Выберите объект в сцене или префабе.");
            return;
        }

        MeshFilter meshFilter = selected.GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.sharedMesh == null)
        {
            Debug.LogError("На объекте нет MeshFilter или меша.");
            return;
        }

        Mesh originalMesh = meshFilter.sharedMesh;

        Mesh newMesh = Object.Instantiate(originalMesh);
        newMesh.name = originalMesh.name + "_Extracted";

        string path = "Assets/Meshes/Extracted/" + newMesh.name + ".asset";
        AssetDatabase.CreateAsset(newMesh, path);
        AssetDatabase.SaveAssets();

        meshFilter.sharedMesh = newMesh;

        MeshCollider meshCollider = selected.GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            meshCollider.sharedMesh = newMesh;
        }

        Debug.Log($"Меш успешно извлечён и назначен: {path}");
    }
}
#endif