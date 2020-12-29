using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

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

        yoyohan.YouDaSdkTool.YouDaSdkMgr.instance.openTuXiaoCao(url, phone, nickname, avatar, openid);
    }

    public void OpenWebView()
    {
        //示例代码
        string url = "https://www.baidu.com";
        yoyohan.YouDaSdkTool.YouDaSdkMgr.instance.openWebView(url);
    }
}
