using System;
using System.Collections.Generic;
using UnityEngine;
using WiSJoy.Observer;
namespace WiSJoy.UI
{

    public class UIManager : MonoBehaviour
    {
        #region Fields
        public UISO SO;
        [Tooltip("Make sure the canvas is child of UIManager")]
        [SerializeField] private Canvas _canvas;

        [SerializeField] private List<eUIID> _uiShow = new List<eUIID>();
        private Dictionary<eUIID, List<BaseUI>> _uiDic = new Dictionary<eUIID, List<BaseUI>>();
        #endregion

        #region Life Cycle
        private void Awake()
        {
            MessageBus.I.Subscribe<PushInfo>(Push, MessageChannel.ui);
            MessageBus.I.Subscribe<PopInfo>(Pop, MessageChannel.ui);
        }

        private void OnDestroy()
        {
            MessageBus.I.Unsubscribe<PushInfo>(Push, MessageChannel.ui);
            MessageBus.I.Unsubscribe<PopInfo>(Pop, MessageChannel.ui);
        }
        #endregion

        #region Event

        private void Push(PushInfo info)
        {
            if (!_uiDic.ContainsKey(info.Key))
            {
                _uiDic.Add(info.Key, new List<BaseUI>());
            }

            if (info.Additive == ePushType.Single)
            {
                foreach (var ui in _uiShow)
                {
                    if (ui != info.Key)
                    {
                        foreach (var item in _uiDic[ui])
                        {
                            item.Hide();
                        }
                    }
                }
                _uiShow.Clear();
                _uiShow.Add(info.Key);
            }
            else if (info.Additive == ePushType.Additive)
            {
                if (!_uiShow.Contains(info.Key))
                {
                    _uiShow.Add(info.Key);
                }
            }
        }

        private void Pop(PopInfo info)
        {
            if (_uiDic.ContainsKey(info.Key))
            {
                foreach (var item in _uiDic[info.Key])
                {
                    item.Hide();
                }
            }
        }
        #endregion
    }

    public class PopInfo
    {
        public eUIID Key;
    }

    public class PushInfo
    {
        public eUIID Key;
        public ePushType Additive;
        public int Priotity;
        public Action<BaseUI> CallBack;
    }

    public enum ePushType
    {
        // Single: Only show one UI, hide others
        Single,
        // Additive: Add a new UI, not hide others
        Additive,
    }

}