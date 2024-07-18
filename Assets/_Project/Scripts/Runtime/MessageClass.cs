// Example message class implementing the resettable interface
using WiSJoy.DesignPattern;
namespace WiSdom
{
    public class PlayerInfo : IResettable
    {
        public int Score { get; set; }
        public string PlayerId { get; set; }

        public void Reset()
        {
            Score = 0;
            PlayerId = null;
        }
    }
    public class EnermyInfo : IResettable
    {
        public int Health { get; set; }
        public string EnermyId { get; set; }

        public void Reset()
        {
            Health = 0;
            EnermyId = null;
        }
    }
}