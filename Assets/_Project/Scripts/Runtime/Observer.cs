using System;
using UnityEngine;
using WiSJoy.DesignPattern;

namespace WiSJoy
{
    public class Observer : MonoBehaviour
    {

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                MessageBus.I.Notify<Example>(message =>
                {
                    message.Score = 100;
                    message.PlayerId = "Player1";
                    message.Callback = () => Debug.Log("Callback");
                }, MessageChannel.gameplay);
            }
        }
    }
}
