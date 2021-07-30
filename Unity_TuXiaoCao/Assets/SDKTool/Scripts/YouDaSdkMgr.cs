using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace yoyohan.YouDaSdkTool
{
    public class YouDaSdkMgr
    {
        private static readonly YouDaSdkMgr _instance = new YouDaSdkMgr();
        public static YouDaSdkMgr instance { get { return _instance; } }

        public Action<ResponceMessage> OnGetSDKResponce;


        #region 核心代码
        private AndroidJavaClass unityClass;
        private AndroidJavaObject currActivity;
        private Dictionary<string, AndroidJavaObject> dicPackages = new Dictionary<string, AndroidJavaObject>();

        private YouDaSdkMgr()
        {
            //1.生成接收SDK消息的Mono
            if (YouDaSdkMono.instance == null)
            {
                GameObject youDaSdkGo = new GameObject("YouDaSdk");
                youDaSdkGo.AddComponent<YouDaSdkMono>();
            }

            //2.初始化AndroidJavaClass
            if (Application.platform == RuntimePlatform.Android)
            {
                unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                currActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
            }
            Debug.Log("YouDaSdkMgr初始化完成！");
        }

        public void SendMessageToAndroid(string methodName, bool isStaticMethod = false, params object[] para)
        {
            Debug.Log("向SDk发送" + methodName + "请求！para:" + para.Length);

            if (Application.platform != RuntimePlatform.Android) return;

            if (isStaticMethod)
            {
                currActivity.CallStatic(methodName, para);
            }
            else
            {
                currActivity.Call(methodName, para);
            }
        }

        public Tvalue SendMessageToAndroid<Tvalue>(string methodName, bool isStaticMethod = false, params object[] para)
        {
            Debug.Log("向SDk发送" + methodName + "请求！para:" + para.Length);

            if (Application.platform != RuntimePlatform.Android) return default(Tvalue);

            if (isStaticMethod)
            {
                return currActivity.CallStatic<Tvalue>(methodName, para);
            }
            else
            {
                return currActivity.Call<Tvalue>(methodName, para);
            }
        }

        /// <summary>
        /// 调用指定包名、指定类
        /// </summary>
        public void SendMessageToAndroidByPackage(string packageName, string methodName, bool isStaticMethod = false, params object[] para)
        {
            Debug.Log("向SDk发送" + methodName + "请求！ para:" + string.Join(" para:", this.ConvertObjectArrayToStringArray(para)));

            if (Application.platform != RuntimePlatform.Android) return;

            AndroidJavaObject activity;
            if (dicPackages.ContainsKey(packageName))
            {
                activity = dicPackages[packageName];
            }
            else
            {
                activity = new AndroidJavaObject(packageName);
                dicPackages.Add(packageName, activity);
            }

            if (isStaticMethod)
            {
                activity.CallStatic(methodName, para);
            }
            else
            {
                activity.Call(methodName, para);
            }
        }


        /// <summary>
        /// 调用指定包名、指定类
        /// </summary>
        public Tvalue SendMessageToAndroidByPackage<Tvalue>(string packageName, string methodName, bool isStaticMethod = false, params object[] para)
        {
            Debug.Log("向SDk发送" + methodName + "请求！ para:" + string.Join(" para:", this.ConvertObjectArrayToStringArray(para)));

            if (Application.platform != RuntimePlatform.Android) return default(Tvalue);

            AndroidJavaObject activity;
            if (dicPackages.ContainsKey(packageName))
            {
                activity = dicPackages[packageName];
            }
            else
            {
                activity = new AndroidJavaObject(packageName);
                dicPackages.Add(packageName, activity);
            }

            if (isStaticMethod)
            {
                return activity.CallStatic<Tvalue>(methodName, para);
            }
            else
            {
                return activity.Call<Tvalue>(methodName, para);
            }

        }
        #endregion


        #region 安装APK的代码 
        //GitHub地址： https://github.com/yoyohan1/AndroidJar_MyInstallApkLibrary

        private bool isInitMyInstallApkLibrary = false;
        private AndroidJavaObject myInstallApkLibrary;

        private void InitMyInstallApkLibrary()
        {
            AndroidJavaClass otherClass = new AndroidJavaClass("com.example.myinstallapk.MainActivity");
            myInstallApkLibrary = otherClass.CallStatic<AndroidJavaObject>("getInstance");

            myInstallApkLibrary.Call("InitContext", currActivity);

            isInitMyInstallApkLibrary = true;
        }

        public void MyInstallApkLibrary(string path)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (isInitMyInstallApkLibrary == false)
                InitMyInstallApkLibrary();

            myInstallApkLibrary.Call("installApk", path);
#endif
        }
        #endregion


        #region 拷贝StreamingAssets的代码
        //GitHub地址： https://github.com/yoyohan1/AndroidJar_testmediastore

        /// <summary>
        /// 拷贝StreamingAssets文件夹下的文件 到别的目录
        /// </summary>
        /// <param name="fromDir">StreamingAssets文件夹下的目录，
        /// 方式1：A/B路径
        /// 方式2：使用""代表asset下所有文件
        /// 方式3：指定文件名也可以，例如ResCache.zip</param>
        /// <param name="toDir">目标路径 安卓的完整路径，
        /// 方式1：A/B路径
        /// 方式2：拷贝某个文件时，需要填入完整路径+"/"+文件名+后缀名</param>
        /// <param name="isOveride">是否覆盖文件 </param>
        public void CopyAssetToSDCard(string fromDir, string toDir, bool isOveride)
        {
            this.SendMessageToAndroidByPackage("com.example.unitytestmediastore.MainActivity", "CopyAssetToSDCard", false, fromDir, toDir, isOveride);
        }

        /// <summary>
        /// 拷贝StreamingAssets示例注册方法
        /// Global.YouDaSdkMgr.OnGetSDKResponce += OnCopyAssetToSDCard;
        /// Global.YouDaSdkMgr.OnGetSDKResponce -= OnCopyAssetToSDCard;
        /// </summary>
        void OnCopyAssetToSDCard(ResponceMessage responceMessage)
        {

        }
        #endregion


        #region 获取设备存储空间
#if UNITY_IOS
        [System.Runtime.InteropServices.DllImport("__Internal")]
        public static extern long getDiskFreeSize_iOS();
#endif
        public long getDiskFreeSize()
        {
            long size = 0;
#if UNITY_EDITOR
            size = -1;
#elif UNITY_ANDROID
            size = SendMessageToAndroid<long>("GetFreeDiskSpace"); 
#elif UNITY_IOS
            size = getDiskFreeSize_iOS();
#endif
            return size;
        }
        #endregion


        #region 苹果openUrl的代码
        //Ios阉割掉libiphone中的UIWebView openURL不能使用的替代工具
        //代码在iOSUtil.mm

#if UNITY_IOS
        [System.Runtime.InteropServices.DllImport("__Internal")]
        public static extern long openUrl_iOS(string url);
#endif

        public void openUrl(string url)
        {
#if UNITY_IOS
            openUrl_iOS(url);
#else
            Application.OpenURL(url);
#endif
        }
        #endregion


        #region 苹果静音模式播放声音
#if UNITY_IOS
        [System.Runtime.InteropServices.DllImport("__Internal")]
        public static extern long openMuteAudio_iOS();
        [System.Runtime.InteropServices.DllImport("__Internal")]
        public static extern long closeMuteAudio_iOS();
#endif

        public void openMuteAudio()
        {
#if UNITY_IOS
            openMuteAudio_iOS();
#endif
        }
        public void closeMuteAudio()
        {
#if UNITY_IOS
            closeMuteAudio_iOS();
#endif
        }
        #endregion


        #region 阿里云推送主动获取通知和消息的代码
        //#if UNITY_IOS
        //        [System.Runtime.InteropServices.DllImport("__Internal")]
        //        public static extern string getLisNoSendMsg();
        //        [System.Runtime.InteropServices.DllImport("__Internal")]
        //        public static extern string getNoSendNotification();
        //        [System.Runtime.InteropServices.DllImport("__Internal")]
        //        public static extern string getDeviceID();//获取iOS设备的deviceId
        //#endif
        //        public string mGetLisNoSendMsg()
        //        {
        //#if UNITY_IOS
        //            return getLisNoSendMsg();
        //#elif UNITY_ANDROID
        //            return YouDaSdkMgr.instance.SendMessageToAndroid<string>("getLisNoSendMsg");
        //#else
        //            return "";
        //#endif
        //        }

        //        public string mGetNoSendNotification()
        //        {
        //#if UNITY_IOS
        //            return getNoSendNotification();
        //#elif UNITY_ANDROID
        //            return YouDaSdkMgr.instance.SendMessageToAndroid<string>("getNoSendNotification");
        //#else
        //            return "";
        //#endif
        //        }

        //        public string mGetDeviceID()
        //        {
        //#if UNITY_IOS&&!UNITY_EDITOR
        //            return getDeviceID();
        //#else
        //            return "null";
        //#endif
        //        }

        #endregion



        /// <summary>
        /// 退出游戏
        /// </summary>
        public void ExitGame()
        {
            this.SendMessageToAndroid("ExitGame");
#if UNITY_IOS
            //TODO..
#endif
        }


        /// <summary>
        /// 例1无返回值：检查支付订单,漏单处理
        /// </summary>
        public void CheckOrderList()
        {
            Debug.LogError("开始漏单处理CheckOrderList！");
            this.SendMessageToAndroid("CheckOrderList");
        }





        /// <summary>
        /// 转换object数组为string数组，方便打印
        /// </summary>
        private String[] ConvertObjectArrayToStringArray(object[] para)
        {
            string[] pataStr = new string[para.Length];
            for (int i = 0; i < para.Length; i++)
            {
                if (para[i] == null)
                {
                    pataStr[i] = "null";
                }
                else
                {
                    pataStr[i] = para[i].ToString();
                }
            }

            return pataStr;
        }

    }


    public enum REQID
    {
        GET_PHOTO = 0,//unitytestmediastore.aar发送 获取媒体库中的所有图片
        GET_VIDEO = 1,//unitytestmediastore.aar发送 获取媒体库中的所有视频
        CopyAssetToSDCard = 2,//unitytestmediastore.aar发送 拷贝安卓assets下的资源
        Open_Notification = 3,//xiaofangapp-mainactivity.aar发送 推送的通知被打开 传递给Unity端的消息
        Receive_Message = 4,//xiaofangapp-mainactivity.aar发送 接收到推送的消息
    }

}