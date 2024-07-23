using UnityEditor;
using UnityEngine;

public class UtilityEditor
{
    /// <summary>
    /// Get file path in asset
    /// </summary>
    /// <param name="fileName">example</param>
    /// <param name="extension">.cs</param>
    /// <returns></returns>
    public static string GetFilePathInAsset(string fileName, string extension = ".cs")
    {
        string[] guids = AssetDatabase.FindAssets(fileName);
        if (guids.Length == 0)
        {
            Debug.LogError("File not found");
            return null;
        }
        string path = "";
        foreach (var item in guids)
        {
            string tempPath = AssetDatabase.GUIDToAssetPath(item);
            // search for exact file name
            string tempFileName = System.IO.Path.GetFileName(tempPath);
            Debug.Log(tempFileName);
            if (string.CompareOrdinal(tempFileName, fileName + extension) == 0)
            {
                path = tempPath;
                break;
            }
        }
        return path;
    }
}
public class ClassInfo
{
    public string Name { get; set; }
    public System.Collections.Generic.List<PropertyInfo> Properties { get; set; }
}

public class PropertyInfo
{
    public string Name { get; set; }
    public string Type { get; set; }
}