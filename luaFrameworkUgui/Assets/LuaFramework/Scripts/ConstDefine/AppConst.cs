using UnityEngine;

namespace LuaFramework
{
    public class AppConst
    {
        public const bool DebugMode = false;                       //调试模式-用于内部测试
        /// <summary>
        /// 如果想删掉框架自带的例子，那这个例子模式必须要
        /// 关闭，否则会出现一些错误。
        /// </summary>
        public const bool ExampleMode = true;                       //例子模式 

        /// <summary>
        /// 如果开启更新模式，前提必须启动框架自带服务器端。
        /// 否则就需要自己将StreamingAssets里面的所有内容
        /// 复制到自己的Webserver上面，并修改下面的WebUrl。
        /// </summary>
        public const bool UpdateMode = true;                       //更新模式-默认关闭 
        public const bool LuaByteMode = false;                       //Lua字节码模式-默认关闭 
        public const bool LuaBundleMode = true;                    //Lua代码AssetBundle模式

        public const int TimerInterval = 1;
        public const int GameFrameRate = 30;                        //游戏帧频

        public const string AssetBundlePath = "Assets/AssetBundles";  //需要打包的资源路径
        public const string AppName = "LuaFramework";                 //应用程序名称
        public const string LuaTempDir = "Lua/";                      //临时目录
        public const string AppPrefix = AppName + "_";                //应用程序前缀
        public const string ExtName = ".unity3d";                     //素材扩展名      
        public const string WebUrl = "http://172.17.73.107:8080/";    //测试更新地址
        public const string VersionFile = "version.txt";              //版本文件

        public static string UserId = string.Empty;                 //用户ID
        public static int SocketPort = 0;                           //Socket服务器端口
        public static string SocketAddress = string.Empty;          //Socket服务器地址



        public static string FrameworkRoot
        {
            get
            {
                return Application.dataPath + "/" + AppName;
            }
        }

        /// <summary>
        /// 素材目录 
        /// </summary>
        public static string AssetDir
        {
            get
            {
                return string.Format("StreamingAssets/{0}", PlatformFile);
            }
        }

        /// <summary>
        /// 平台文件夹
        /// </summary>
        /// <returns></returns>
        public static string PlatformFile
        {
            get
            {
#if UNITY_EDITOR
                return "Windows";
#elif UNITY_ANDROID
                return "Android";
#elif UNITY_IOS
                return "IOS";
#else
                return "Other";
#endif
            }
        }
    }
}