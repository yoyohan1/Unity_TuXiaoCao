package com.yoyohan.tuxiaocao;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.content.pm.ActivityInfo;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;

import com.unity3d.player.UnityPlayer;

public class MainActivity extends Activity {

    /*打开兔小巢反馈链接
     * openTuXiaoCao("https://support.qq.com/product/298495", "12345678912", "test", "https://c-ssl.duitang.com/uploads/blog/202012/04/20201204115704_641e3.png", "11");
     * */
    public void openTuXiaoCao(final String url, final String phone, final String nickname, final String avatar, final String openid) {

        Activity activity = UnityPlayer.currentActivity;

        Intent intent = new Intent(activity, TuXiaoCaoActivityP.class);
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
    public void openWebView(final String url, final int orientationID) {
        Activity activity = UnityPlayer.currentActivity;
        Intent intent;
        if (orientationID == 1 || orientationID == 2) {
            intent = new Intent(activity, TuXiaoCaoActivityP.class);
        } else {
            intent = new Intent(activity, TuXiaoCaoActivityL.class);
        }

        intent.putExtra("type", "url");
        intent.putExtra("url", url);
        activity.startActivity(intent);
    }


    /**
     * 获取本地软件版本号
     */
    public static String getLocalVersionCode() {
        String localVersion = "未知";
        try {
            PackageInfo packageInfo = UnityPlayer.currentActivity.getApplicationContext()
                    .getPackageManager()
                    .getPackageInfo(UnityPlayer.currentActivity.getPackageName(), 0);
            localVersion = packageInfo.versionCode + "";
        } catch (PackageManager.NameNotFoundException e) {
            localVersion = "未知";
        }
        return localVersion;
    }

    /**
     * 获取本地软件版本号名称
     */
    public static String getLocalVersionName() {
        String localVersion = "未知";
        try {
            PackageInfo packageInfo = UnityPlayer.currentActivity.getApplicationContext()
                    .getPackageManager()
                    .getPackageInfo(UnityPlayer.currentActivity.getPackageName(), 0);
            localVersion = packageInfo.versionName;
        } catch (PackageManager.NameNotFoundException e) {
            localVersion = "未知";
        }
        return localVersion;
    }

}
