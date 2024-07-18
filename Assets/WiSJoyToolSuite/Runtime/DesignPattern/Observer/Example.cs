using System;

namespace WiSJoy.DesignPattern
{
    // Example message class implementing the resettable interface
    public class Example : IResettable
    {
        public int Score { get; set; }
        public string PlayerId { get; set; }
        public Action Callback { get; set; }

        public void Reset()
        {
            Score = 0;
            PlayerId = null;
        }
    }
}