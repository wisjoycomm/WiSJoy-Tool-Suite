using UnityEngine;
namespace WiSJoy.Observer
{
    public class Observer : MonoBehaviour
    {
        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                MessageBus.I.Notify<Example>(message =>
                {
                    message.Score = 100;
                    message.PlayerId = "Player1";
                    message.Callback = null;
                }, MessageChannel.gameplay);
            }

#else
            if (Input.touchCount > 0)
            {
                MessageBus.I.Notify<Example>(message =>
                {
                    message.Score = 200;
                    message.PlayerId = "Player2";
                    message.Callback = null;
                }, MessageChannel.gameplay);
            }
#endif 
        }
    }
}
