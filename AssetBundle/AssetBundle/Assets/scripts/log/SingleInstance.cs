using UnityEngine;

namespace Utilities
{
    public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T sInstance;

        private static bool sIsQuitting = false;

        private static object sLock = new object();

        public static T Instance
        {
            get
            {
                if (sIsQuitting && Application.isPlaying)
                {
                    Debug.LogWarning("application is quitting");
                    return null;
                }

                lock (sLock)
                {
                    if (sInstance == null)
                    {
                        sInstance = (T)FindObjectOfType(typeof(T));

                        if (FindObjectsOfType(typeof(T)).Length > 1)
                        {
                            return sInstance;
                        }

                        if (sInstance == null)
                        {
                            GameObject singleton = new GameObject();
                            sInstance = singleton.AddComponent<T>();
                            singleton.name = "(singleton) " + typeof(T).ToString();
                            DontDestroyOnLoad(singleton);
                        }
                    }

                    return sInstance;
                }
            }
        }

        public void OnDestroy()
        {
            if (Application.isPlaying)
            {
                Debug.LogWarningFormat("singleton [{0}] destroy", this.ToString());
                sIsQuitting = true;
            }
        }
    }
}
