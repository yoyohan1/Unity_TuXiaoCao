using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yoyohan.YouDaSdkTool;

namespace yoyohan.tuxiaocao
{
    public class TuXiaoCaoMgr
    {
        private static TuXiaoCaoMgr _instance;
        public static TuXiaoCaoMgr instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TuXiaoCaoMgr();
                }
                return _instance;
            }
        }

        private const string requestId = "UNITY_TUXIAOCAO_RES";
        private string packageName = "com.yoyohan.tuxiaocao.MainActivity";


        #region 核心代码

#if UNITY_IOS
        [System.Runtime.InteropServices.DllImport("__Internal")]
        public static extern string getiPhoneType_iOS();
        [System.Runtime.InteropServices.DllImport("__Internal")]
        public static extern void openTuXiaoCao_iOS(string url, string phone, string nickname, string avatar, string openid);
        [System.Runtime.InteropServices.DllImport("__Internal")]
        public static extern void openWebView_iOS(string url, int orientationID);
#endif

        /// <summary> 
        /// 已更新到iPhone 12 返回结果为：iPhone 12 mini、iPhone 12 Pro Max
        /// </summary>
        public string getiPhoneType()
        {
#if UNITY_IOS
            return getiPhoneType_iOS();
#endif
            return "";
        }


        public void openTuXiaoCao(string url, string phone, string nickname, string avatar, string openid)
        {
            //示例代码
            //string url = "https://support.qq.com/product/298495";
            //string phone = "phone";
            //string nickname = "nickname";
            //if (nickname.Contains(phone))
            //{
            //    char[] returnStr = phone.ToCharArray();
            //    for (int i = 0; i < phone.Length; i++)
            //    {
            //        if (i >= 3 && i <= 6)
            //        {
            //            returnStr[i] = '*';
            //        }
            //    }
            //    nickname = nickname.Replace(phone, new string(returnStr));
            //}
            //string avatar = "https://c-ssl.duitang.com/uploads/blog/202012/04/20201204115704_641e3.png";
            //string openid = "openid";

            if (Application.platform == RuntimePlatform.Android)
            {
                YouDaSdkMgr.instance.SendMessageToAndroidByPackage(packageName, "openTuXiaoCao", false, url, phone, nickname, avatar, openid);
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
#if UNITY_IOS
                openTuXiaoCao_iOS(url, phone, nickname, avatar, openid);
#endif
            }
            else
            {
                Application.OpenURL(url);
            }
        }

        public void openWebView(string url, int orientationID = -1)
        {
            if (orientationID == -1)
            {
                orientationID = (int)Screen.orientation;
            }

            List<int> orientationArr = new List<int>() { 1, 2, 3, 4 };
            if (orientationArr.Contains(orientationID) == false)
            {
                orientationID = 1;
            }

            if (Application.platform == RuntimePlatform.Android)
            {
                YouDaSdkMgr.instance.SendMessageToAndroidByPackage(packageName, "openWebView", false, url, orientationID);
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
#if UNITY_IOS
                openWebView_iOS(url, orientationID);
#endif
            }
            else
            {
                YouDaSdkMgr.instance.openUrl(url);
            }
        }
        #endregion


    }
}

