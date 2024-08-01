using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WiSJoy.Observer
{
    public class ComparedSubcriber : MonoBehaviour
    {
        public void OnExampleMessage(Example message)
        {
            // Debug.Log($"Score: {message.Score}, PlayerId: {message.PlayerId}");
            // message.Callback?.Invoke();
        }
    }
}
