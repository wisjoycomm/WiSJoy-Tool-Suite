using System.IO;
using System.Text;
using Cysharp.Threading.Tasks;
using MessagePack;
using UnityEngine;
using WiSdom;

public static class SaveManager
{
        public static async UniTask SaveData<T>(T data, string fileBase)
        {
                try
                {
                        string jsonPath = Path.Combine(Application.persistentDataPath, fileBase + ".json");
#if USE_JSON
                        // Save as JSON
#if USE_MESSAGEPACK
                        string jsonData = MessagePackSerializer.SerializeToJson(data);
#else
                        string jsonData = JsonUtility.ToJson(data, true); // Add prettyPrint for readability
#endif
                        await File.WriteAllTextAsync(jsonPath, jsonData); // Use async method
#else
                        // Save as binary
                        string binaryPath = Path.Combine(Application.persistentDataPath, Utility.DecodeBase64ToString(fileBase) + ".bin"); // Use ".bytes" extension
#if USE_MESSAGEPACK
                        byte[] serializedData = MessagePackSerializer.Serialize(data);
#else
                        byte[] serializedData = Utility.ConvertStringToByte(JsonUtility.ToJson(data));
#endif
                        await File.WriteAllBytesAsync(binaryPath, serializedData); // Use async method
#endif
                }
                catch (IOException e)
                {
                        Debug.LogError(e.Message); // Log the exception message
                }
        }

        public static async UniTask<T> LoadData<T>(string fileBase)
        {
                try
                {
#if USE_JSON
                        // Load from JSON
                        string jsonPath = Path.Combine(Application.persistentDataPath, fileBase + ".json");
                        string jsonData = await File.ReadAllTextAsync(jsonPath); // Use async method
#if USE_MESSAGEPACK
                        // Deserialize using MessagePack
                        return MessagePackSerializer.Deserialize<T>(MessagePackSerializer.ConvertFromJson(jsonData));
#else
                        // Deserialize using Unity's JsonUtility
                        return JsonUtility.FromJson<T>(jsonData);
#endif
#else
                        // Load from binary
                        string binaryPath = Path.Combine(Application.persistentDataPath, Utility.DecodeBase64ToString(fileBase) + ".bin"); // Use ".bytes" extension
                        byte[] serializedData = await File.ReadAllBytesAsync(binaryPath); // Use async method

#if USE_MESSAGEPACK
                        // Deserialize using MessagePack
                        return MessagePackSerializer.Deserialize<T>(serializedData);
#else
                        // Deserialize using Unity's JsonUtility
                        string jsonData = Utility.ConvertByteToString(serializedData);
                        return JsonUtility.FromJson<T>(jsonData);
#endif
#endif
                }
                catch
                {
                        return default; // Return default value
                }
        }

        public static void ClearData(string fileBase)
        {
#if USE_JSON
                string path = Path.Combine(Application.persistentDataPath, fileBase + ".json");
#else
                string path = Path.Combine(Application.persistentDataPath, Utility.DecodeBase64ToString(fileBase) + ".bin"); // Use ".bytes" extension
#endif
                if (File.Exists(path)) // Check if the file exists before deleting
                {
                        File.Delete(path);
                }
        }
}