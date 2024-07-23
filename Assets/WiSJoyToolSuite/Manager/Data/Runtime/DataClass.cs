using System;
namespace WiSJoy.Manage.Data
{
    [Serializable]
    public class VersionData
    {
        public string Ver = "1.0.0";
        public int VerCode = 1;
    }
    [Serializable]
    public class PlayerData
    {
        public string Name = "Player";
    }

    [Serializable]
    public class DeviceInfo
    {
        public string DeviceName = "Device";
        public string DeviceModel = "Model";
        public string DeviceType = "Type";
        public string DeviceSystem = "System";
        public string DeviceSystemVersion = "Version";
        public string DeviceUniqueIdentifier = "UniqueIdentifier";
    }
}