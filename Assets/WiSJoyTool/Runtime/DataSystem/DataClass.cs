using System;
using System.Collections.Generic;
using MessagePack;
using UnityEngine;
namespace WiSdom.SaveSystem.Data
{
    [MessagePackObject]
    [Serializable]
    public class VersionData
    {
        [Key(0)]
        public string Ver = "1.0.0";
        [Key(1)]
        public int VerCode = 1;
    }
    [MessagePackObject]
    [Serializable]
    public class PlayerData
    {
        [Key(0)]
        public string Name = "Player";
    }

    [MessagePackObject]
    [Serializable]
    public class DeviceInfo
    {
        [Key(0)]
        public string DeviceName = "Device";
        [Key(1)]
        public string DeviceModel = "Model";
        [Key(2)]
        public string DeviceType = "Type";
        [Key(3)]
        public string DeviceSystem = "System";
        [Key(4)]
        public string DeviceSystemVersion = "Version";
        [Key(5)]
        public string DeviceUniqueIdentifier = "UniqueIdentifier";
    }
    [MessagePackObject]
    [Serializable]
    public class BasicTypeData
    {
        [Key(0)]
        public Vector2 Vector2 = Vector2.zero;
        [Key(1)]
        public Vector3 Vector3 = Vector3.zero;
        [Key(2)]
        public Vector4 Vector4 = Vector4.zero;
        [Key(3)]
        public Quaternion Quaternion = Quaternion.identity;
        [Key(4)]
        public Color Color = Color.white;
        [Key(5)]
        public Color32 Color32 = new Color32(255, 255, 255, 255);
        [Key(6)]
        public int[] ints = new int[] { 1, 2, 3, 4, 5 };
        [Key(7)]
        public float[] floats = new float[] { 1.1f, 2.2f, 3.3f, 4.4f, 5.5f };
        [Key(8)]
        public string[] strings = new string[] { "1", "2", "3", "4", "5" };
        [Key(9)]
        public bool[] bools = new bool[] { true, false, true, false, true };
        [Key(10)]
        public byte[] bytes = new byte[] { 1, 2, 3, 4, 5 };
        [Key(11)]
        public short[] shorts = new short[] { 1, 2, 3, 4, 5 };
        [Key(12)]
        public long[] longs = new long[] { 1, 2, 3, 4, 5 };
        [Key(13)]
        public double[] doubles = new double[] { 1.1, 2.2, 3.3, 4.4, 5.5 };
        [Key(14)]
        public decimal[] decimals = new decimal[] { 1.1m, 2.2m, 3.3m, 4.4m, 5.5m };
        [Key(15)]
        public List<int> intList = new List<int>() { 1, 2, 3, 4, 5 };
        [Key(16)]
        public List<float> floatList = new List<float>() { 1.1f, 2.2f, 3.3f, 4.4f, 5.5f };
        [Key(17)]
        public List<string> stringList = new List<string>() { "1", "2", "3", "4", "5" };
        [Key(18)]
        public List<bool> boolList = new List<bool>() { true, false, true, false, true };
        [Key(19)]
        public List<byte> byteList = new List<byte>() { 1, 2, 3, 4, 5 };
        [Key(20)]
        public List<short> shortList = new List<short>() { 1, 2, 3, 4, 5 };
        [Key(21)]
        public List<long> longList = new List<long>() { 1, 2, 3, 4, 5 };
        [Key(22)]
        public List<double> doubleList = new List<double>() { 1.1, 2.2, 3.3, 4.4, 5.5 };
        [Key(23)]
        public List<decimal> decimalList = new List<decimal>() { 1.1m, 2.2m, 3.3m, 4.4m, 5.5m };
        [Key(24)]
        public Dictionary<string, int> intDictionary = new Dictionary<string, int>() { { "1", 1 }, { "2", 2 }, { "3", 3 }, { "4", 4 }, { "5", 5 } };
        [Key(25)]
        public Dictionary<string, float> floatDictionary = new Dictionary<string, float>() { { "1", 1.1f }, { "2", 2.2f }, { "3", 3.3f }, { "4", 4.4f }, { "5", 5.5f } };
        [Key(26)]
        public Dictionary<string, string> stringDictionary = new Dictionary<string, string>() { { "1", "1" }, { "2", "2" }, { "3", "3" }, { "4", "4" }, { "5", "5" } };
        [Key(27)]
        public HashSet<int> intHashSet = new HashSet<int>() { 1, 2, 3, 4, 5 };
        [Key(28)]
        public HashSet<float> floatHashSet = new HashSet<float>() { 1.1f, 2.2f, 3.3f, 4.4f, 5.5f };
        [Key(29)]
        public HashSet<string> stringHashSet = new HashSet<string>() { "1", "2", "3", "4", "5" };
    }

}