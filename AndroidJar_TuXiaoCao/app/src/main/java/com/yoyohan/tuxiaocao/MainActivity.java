package com.yoyohan.tuxiaocao;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        //关联两个button和TextView变量
        Button mAddOne = (Button) findViewById(R.id.button);
        mAddOne.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                openTuXiaoCao("https://support.qq.com/product/298495", "12345678912", "test", "https://c-ssl.duitang.com/uploads/blog/202012/04/20201204115704_641e3.png", "11");
            }
        });

        Button mAddtwo = (Button) findViewById(R.id.button1);
        mAddtwo.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                openWebView("https://yl-mobile.niceloo.com/agreement/privacy_yl_a.html");
            }
        });
    }

    /*打开兔小巢反馈链接*/
    public void openTuXiaoCao(final String url, final String phone, final String nickname, final String avatar, final String openid) {

        Intent intent = new Intent(this, TuXiaoCaoActivity.class);//UnityPlayer.currentActivity.getApplicationContext()
        intent.putExtra("type", "tuxiaocao");
        intent.putExtra("url", url);
        intent.putExtra("phone", phone);
        intent.putExtra("nickname", nickname);
        intent.putExtra("avatar", avatar);
        intent.putExtra("openid", openid);
        this.startActivity(intent);

    }


    /*打开网址
     * openWebView("https://yl-mobile.niceloo.com/agreement/privacy_yl_a.html");
     * */
    public void openWebView(final String url) {
        Intent intent = new Intent(this, TuXiaoCaoActivity.class);
        intent.putExtra("type", "url");
        intent.putExtra("url", url);
        this.startActivity(intent);
    }
}
