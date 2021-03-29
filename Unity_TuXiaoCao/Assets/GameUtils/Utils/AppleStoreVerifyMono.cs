using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace yoyohan
{
    /// <summary>
    /// 描述：
    /// 功能：
    /// 作者：yoyohan
    /// 创建时间：2020-12-08 15:17:56
    /// </summary>
    public class AppleStoreVerifyMono : MonoBehaviour
    {
        public static AppleStoreVerifyMono instance;

        public bool isAppleStoreVerify;

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
#if UNITY_IOS
            HttpGetPostMgr.instance.CreatHttpGetPost("http://quan.suning.com/getSysTime.do")
                .SetCallbackSucceed(OnGetTimeSucceed)
                .SetCallbackFailed(www => isAppleStoreVerify = false)
                .SetMaxRetryTime(1)
                .StartHttpGetPost();
#endif
        }
        void OnGetTimeSucceed(UnityWebRequest www)
        {
            DateTime dateTime= DateTime.Parse("2020-12-08 17:59:00");
            try
            {
                JsonData jsonData = JsonMapper.ToObject(www.downloadHandler.text);
                DateTime dateTimeNow = DateTime.Parse(jsonData.GetValue<string>("sysTime2"));
                if (dateTime.AddDays(2)>=dateTimeNow)
                {
                    isAppleStoreVerify = true;
                    Debug.Log("得到回应" + jsonData.GetValue<string>("sysTime2"));
                }
            }
            catch (Exception ex)
            {

            }        }
    }
}
