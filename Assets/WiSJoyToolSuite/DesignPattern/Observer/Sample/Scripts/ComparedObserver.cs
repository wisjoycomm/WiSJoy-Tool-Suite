using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WisJoy
{
    public class ComparedObserver : MonoBehaviour
    {
        [SerializeField] int number = 10000;
        [SerializeField] List<ComparedSubcriber> subcribers = new List<ComparedSubcriber>();

        private void Start()
        {
            for (int i = 0; i < number; i++)
            {
                var subcriber = new GameObject().AddComponent<ComparedSubcriber>();
                subcriber.transform.SetParent(transform);
                subcribers.Add(subcriber);
            }
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                var example = new Example
                {
                    Score = 100,
                    PlayerId = "Player1",
                    Callback = null
                };
                foreach (var item in subcribers)
                {
                    item.OnExampleMessage(example);
                }
            }

#else
            if (Input.touchCount > 0)
            {
                var example = new Example
                {
                    Score = 200,
                    PlayerId = "Player2",
                    Callback = null
                };
                foreach (var item in subcribers)
                {
                    item.OnExampleMessage(example);
                }
            }
#endif 
        }
    }
}
