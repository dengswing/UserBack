namespace com.shinezone.network
{
    /// <summary>
    /// 单例类实例化
    /// </summary>
    public class SingleInstance<T> where T : new ()
    {
        private static T sInstance;
        private static object sLock = new object();
        public static T Instance
        {
            get
            {
                lock (sLock)
                {
                    if (sInstance == null)
                    {
                        sInstance = new T();
                    }

                    return sInstance;
                }
            }
        }
    }
}