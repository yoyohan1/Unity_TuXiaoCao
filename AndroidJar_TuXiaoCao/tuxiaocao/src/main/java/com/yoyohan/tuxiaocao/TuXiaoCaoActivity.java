package com.yoyohan.tuxiaocao;

import android.annotation.SuppressLint;
import android.app.ActionBar;
import android.app.Activity;
import android.content.Intent;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.net.Uri;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.webkit.WebChromeClient;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.ProgressBar;

public class TuXiaoCaoActivity extends Activity {

    private WebView webView;
    private ProgressBar pg1;
    //private ProgressDialog dialog;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        ActionBar actionBar = getActionBar();
        actionBar.setTitle("");
        actionBar.setDisplayShowHomeEnabled(false);
        int color = Color.parseColor("#000000");
        ColorDrawable drawable = new ColorDrawable(color);
        actionBar.setBackgroundDrawable(drawable);

        String type = this.getIntent().getStringExtra("type");
        String url = this.getIntent().getStringExtra("url");

        if (type.equals("tuxiaocao")) {
            url = url + "?d-wx-push=1";
        }

        setContentView(R.layout.activity_tuxiaocao);
        webView = (WebView) findViewById(R.id.tuxiaocao);
        pg1 = (ProgressBar) findViewById(R.id.progressBar1);
        webView.getSettings().setJavaScriptEnabled(true);
        webView.getSettings().setDomStorageEnabled(true);       // 这个要加上

        WebViewClient webViewClient = new WebViewClient() {
            /**
             * 拦截 url 跳转,在里边添加点击链接跳转或者操作
             */
            @Override
            public boolean shouldOverrideUrlLoading(WebView view, String url) {
                super.shouldOverrideUrlLoading(view, url);

                if (url == null) {
                    return false;
                }
                try {
                    if (url.startsWith("weixin://")) {
                        Intent intent = new Intent(Intent.ACTION_VIEW, Uri.parse(url));
                        view.getContext().startActivity(intent);
                        return true;//代表告诉系统已经拦截 不需要再做加载了
                    }
                } catch (Exception e) {
                    return false;
                }
                view.loadUrl(url);
                return true;
            }
        };
        webView.setWebViewClient(webViewClient);

        webView.setWebChromeClient(new WebChromeClient() {
            @Override
            public void onProgressChanged(WebView view, int newProgress) {
                // TODO 自动生成的方法存根

                if (newProgress == 100) {
                    pg1.setAlpha(0);
                    //pg1.setVisibility(View.GONE);//加载完网页进度条消失 这种方式加载时webview会不显示
                    //this.closeDialog();//Dialog这种方式太丑
                } else {
                    pg1.setAlpha(1);
                    //pg1.setVisibility(View.VISIBLE);//开始加载网页时显示进度条
                    //this.openDialog(newProgress);
                    pg1.setProgress(newProgress);//设置进度值
                }
            }
//            private void openDialog(int newProgress) {
//                if(dialog==null){
//                    dialog=new ProgressDialog(TuXiaoCaoActivity.this);//新建进度条
//                    dialog.setTitle("正在加载中");//显示文字
//                    dialog.setProgressStyle(ProgressDialog.STYLE_HORIZONTAL);//水平进度条
//                    dialog.setProgress(newProgress);//获得进度
//                    dialog.show();//显示进度
//                }else{
//                    dialog.setProgress(newProgress);
//                }
//            }
//            private void closeDialog() {
//                if(dialog!=null&&dialog.isShowing()){
//                    dialog.dismiss();
//                    dialog=null;
//                }
//            }
        });

        //兔小巢页面
        if (type.equals("tuxiaocao")) {
            String phone = this.getIntent().getStringExtra("phone");
            String nickname = this.getIntent().getStringExtra("nickname");
            String avatar = this.getIntent().getStringExtra("avatar");
            String openid = this.getIntent().getStringExtra("openid");
            String jixing = android.os.Build.BRAND + "  " + android.os.Build.MODEL;
            String customInfo = "账号:" + phone + " 机型:" + jixing + " 手机版本:Android" + android.os.Build.VERSION.RELEASE;
            String postData = "nickname=" + nickname + "&avatar=" + avatar + "&openid=" + openid + "&customInfo=" + customInfo;

            Log.i("Unity", "打开兔小巢页面 postData:" + postData);
            webView.postUrl(url, postData.getBytes());
        } else if (type.equals("url")) {
            //普通页面
            Log.i("Unity", "打开页面 url:" + url);
            webView.loadUrl(url);
        }
        
    }


    @SuppressLint("ResourceType")
    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // 为ActionBar扩展菜单项
        MenuInflater inflater = getMenuInflater();
        inflater.inflate(R.layout.tuxiaocao_activity_actions, menu);
        return super.onCreateOptionsMenu(menu);
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        int i = item.getItemId();
        if (i == R.id.action_exit) {
            this.finish();
            return true;
        } else if (i == R.id.action_back) {
            if (webView.canGoBack()) {
                webView.goBack();
            } else {
                this.finish();
            }
            return true;
        } else if (i == R.id.action_refersh) {
            webView.reload();
            return true;
        } else {
            return super.onOptionsItemSelected(item);
        }
    }

}