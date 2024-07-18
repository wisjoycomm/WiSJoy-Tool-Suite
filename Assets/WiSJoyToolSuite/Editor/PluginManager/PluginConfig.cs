using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PluginConfig", menuName = "Configuration/PluginConfig")]
public class PluginConfig : ScriptableObject
{
    public List<PluginData> plugins = new List<PluginData>();
}

[System.Serializable]
public class PluginData
{
    public ePlugin Name;
    public bool IsEnable;
    public string DefineSymbol;
    public PluginData()
    {
        Name = ePlugin.None;
        IsEnable = false;
        DefineSymbol = $"USE_{Name.ToString().ToUpper()}";
    }

    // Change DefineSymbol when Name is changed
    public void ChangeDefineSymbol()
    {
        if (Name == ePlugin.None)
        {
            DefineSymbol = "";
            return;
        }
        DefineSymbol = $"USE_{Name.ToString().ToUpper()}";
    }
}
public enum ePlugin
{
    None,
    Addressables,
    MessagePack,
}
