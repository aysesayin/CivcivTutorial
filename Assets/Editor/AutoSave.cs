using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class AutoSave
{
    private static double nextSaveTime;
    private static double saveInterval = 300; // saniye (örn: 300 = 5 dakika)

    static AutoSave()
    {
        EditorApplication.update += Update;
        nextSaveTime = EditorApplication.timeSinceStartup + saveInterval;
    }

    private static void Update()
    {
        if (EditorApplication.timeSinceStartup >= nextSaveTime)
        {
            SaveIfSceneModified();
            nextSaveTime = EditorApplication.timeSinceStartup + saveInterval;
        }
    }

    private static void SaveIfSceneModified()
    {
        bool saved = false;

        for (int i = 0; i < EditorSceneManager.sceneCount; i++)
        {
            var scene = EditorSceneManager.GetSceneAt(i);
            if (scene.isDirty)
            {
                EditorSceneManager.SaveScene(scene);
                saved = true;
                Debug.Log($"[AutoSave] Scene saved: {scene.name}");
            }
        }

        if (saved)
            AssetDatabase.SaveAssets(); // Varsa asset değişikliklerini de kaydeder
    }
}