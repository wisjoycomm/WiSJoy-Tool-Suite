using UnityEngine;

namespace WiSJoy.UI
{
    [CreateAssetMenu(fileName = "UISO", menuName = "WiSJoy/UI/UISO")]
    public class UISO : ScriptableObject
    {
        public UISOEntry[] UIEntries;

        public void Load(UIAssets uiAssets)
        {
            for (int i = 0; i < UIEntries.Length; i++)
            {
                for (int j = 0; j < uiAssets.UIEntries.Length; j++)
                {
                    if (uiAssets.UIEntries[j].Key == UIEntries[i].Key.ToString())
                    {
                        UIEntries[i].UI = uiAssets.UIEntries[j].UI;
                        break;
                    }
                }
            }
        }
    }
    [System.Serializable]
    public class UISOEntry
    {
        public UIID Key;
        public BaseUI UI;
    }
}