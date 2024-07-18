using UnityEngine;

namespace WiSJoy.DesignPattern
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T I
        {
            get
            {
                // If the instance is null, find the object in the scene
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                }
                // If the instance is still null, create a new object
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    _instance = obj.AddComponent<T>();
                }
                return _instance;
            }
            set { _instance = value; }
        }
        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public static bool IsInstanceValid()
        {
            return _instance != null;
        }
    }

    public class SingletonClass<T> where T : class, new()
    {
        private static T _instance;
        public static T I
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
            set { _instance = value; }
        }
    }
}
