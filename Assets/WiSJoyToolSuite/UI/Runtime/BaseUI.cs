using UnityEngine;

namespace WiSJoy.UI
{
    public class BaseUI : MonoBehaviour
    {
        public PushInfo UIInfo { get; private set; }
        public virtual void Show(PushInfo uiInfo)
        {
            UIInfo = uiInfo;
        }
        public virtual void Hide()
        {
            UIInfo = null;
        }
    }
}