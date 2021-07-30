using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yoyohan.tuxiaocao.demo
{
    public class SampleScene : MonoBehaviour
    {
        public void OpenTuXiaoCao()
        {
            //示例代码
            string url = "https://support.qq.com/product/298495";
            string phone = "phone";
            string nickname = "nickname";
            if (nickname.Contains(phone))
            {
                char[] returnStr = phone.ToCharArray();
                for (int i = 0; i < phone.Length; i++)
                {
                    if (i >= 3 && i <= 6)
                    {
                        returnStr[i] = '*';
                    }
                }
                nickname = nickname.Replace(phone, new string(returnStr));
            }
            string avatar = "https://c-ssl.duitang.com/uploads/blog/202012/04/20201204115704_641e3.png";
            string openid = "openid";

            TuXiaoCaoMgr.instance.openTuXiaoCao(url, phone, nickname, avatar, openid);
        }


        public void OpenBaiDu(int orientationID)
        {
            //示例代码
            string url = "https://gitee.com/";
            TuXiaoCaoMgr.instance.openWebView(url, orientationID);
        }

    }

}