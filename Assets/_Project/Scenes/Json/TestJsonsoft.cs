
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MessagePack;
using MessagePack.Resolvers;
using MessagePack.Unity;
using MessagePack.Unity.Extension;
using TMPro;
using UnityEngine;
using WiSdom.SaveSystem;
namespace WiSdom
{
    public class TestJsonsoft : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI jsonText;
        private bool isLoadorSave = true;
        int run = 0;
        void Update()
        {
            if (Input.GetMouseButtonDown(0) && !isLoadorSave)
            {
                Debug.Log("Load");
                isLoadorSave = true;
                DataManager.I.LoadBasicTypeDataData().Forget();
            }
            else if (Input.GetMouseButtonDown(0) && isLoadorSave)
            {
                Debug.Log("Save");
                isLoadorSave = false;
                Serialize();
                DataManager.I.Data.BasicTypeData.boolList = new List<bool>();
                run++;
                for (int i = 0; i < run; i++)
                {
                    DataManager.I.Data.BasicTypeData.boolList.Add(true);
                }
                DataManager.I.SaveBasicTypeDataData().Forget();
            }
        }

        public void Serialize()
        {
            var bytes = MessagePackSerializer.Serialize(DataManager.I.Data.BasicTypeData);
            jsonText.text = MessagePackSerializer.ConvertToJson(bytes);
        }
    }
}
