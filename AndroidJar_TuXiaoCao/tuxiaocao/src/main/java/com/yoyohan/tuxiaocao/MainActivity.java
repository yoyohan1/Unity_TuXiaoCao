package com.yoyohan.tuxiaocao;

import android.app.Activity;
import android.content.Intent;

import com.unity3d.player.UnityPlayer;

public class MainActivity {

    /*打开兔小巢反馈链接
    * openTuXiaoCao("https://support.qq.com/product/298495", "12345678912", "test", "https://c-ssl.duitang.com/uploads/blog/202012/04/20201204115704_641e3.png", "11");
    * */
    public void openTuXiaoCao(final String url, final String phone, final String nickname, final String avatar, final String openid) {

        Activity activity = UnityPlayer.currentActivity;

        Intent intent = new Intent(activity, TuXiaoCaoActivity.class);
        intent.putExtra("type", "tuxiaocao");
        intent.putExtra("url", url);
        intent.putExtra("phone", phone);
        intent.putExtra("nickname", nickname);
        intent.putExtra("avatar", avatar);
        intent.putExtra("openid", openid);
        activity.startActivity(intent);
    }


    /*打开网址
     * openWebView("https://yl-mobile.niceloo.com/agreement/privacy_yl_a.html");
     * */
    public void openWebView(final String url) {
        Activity activity = UnityPlayer.currentActivity;

        Intent intent = new Intent(activity, TuXiaoCaoActivity.class);
        intent.putExtra("type", "url");
        intent.putExtra("url", url);
        activity.startActivity(intent);
    }
}
