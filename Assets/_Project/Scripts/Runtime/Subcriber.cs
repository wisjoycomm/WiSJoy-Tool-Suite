using System;
using UnityEngine;
using WiSJoy.DesignPattern;

namespace WiSJoy
{
    public class Subcriber : MonoBehaviour
    {
        private void OnEnable()
        {
            MessageBus.I.Subscribe<Example>(OnExampleMessage, MessageChannel.ui);
        }
        private void OnDisable()
        {
            MessageBus.I.Unsubscribe<Example>(OnExampleMessage, MessageChannel.ui);
        }

        private void OnExampleMessage(Example message)
        {
            Debug.Log($"Score: {message.Score}, PlayerId: {message.PlayerId}");
            message.Callback?.Invoke();
        }
    }
}
