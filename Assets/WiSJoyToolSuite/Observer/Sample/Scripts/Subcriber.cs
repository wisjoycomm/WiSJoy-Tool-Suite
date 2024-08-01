using System;
using UnityEngine;

namespace WiSJoy.Observer
{
    public class Subcriber : MonoBehaviour
    {
        private void OnEnable()
        {
            MessageBus.I.Subscribe<Example>(OnExampleMessage, MessageChannel.gameplay);
        }
        private void OnDisable()
        {
            MessageBus.I.Unsubscribe<Example>(OnExampleMessage, MessageChannel.gameplay);
        }

        private void OnExampleMessage(Example message)
        {
            // Debug.Log($"Score: {message.Score}, PlayerId: {message.PlayerId}");
            // message.Callback?.Invoke();
        }
    }
}
