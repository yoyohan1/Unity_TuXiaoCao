//
//  TuXiaoCaoViewController.m

#import "TuXiaoCaoViewController.h"
// 如果使用 WKWebview 的话，需要导入 Webkit 的头文件
#import "WebKit/WebKit.h"
#import <sys/utsname.h>
#import "iOSUtil.h"

@interface TuXiaoCaoViewController ()<WKNavigationDelegate>

@end

@implementation TuXiaoCaoViewController

//隐藏状态栏
- (BOOL)prefersStatusBarHidden
{
    return NO;
}

Boolean canAutorotate=YES;
//是否旋转屏幕
- (BOOL)shouldAutorotate{
    return canAutorotate;
}

//初始旋转
-(UIInterfaceOrientation)preferredInterfaceOrientationForPresentation{
    Boolean appIsLandscape=NO;
    NSArray *arr= [[NSBundle mainBundle]objectForInfoDictionaryKey:@"UISupportedInterfaceOrientations"];
    for (int i=0; i<arr.count; i++) {
        if ([arr[i] containsString:@"Landscape"]) {
            appIsLandscape=YES;
        }
        else{
            appIsLandscape=NO;
        }
        NSLog(@"遍历info.plist的UISupportedInterfaceOrientations[0]:%@ 得出appIsLandscape：%@",arr[i],appIsLandscape?@"YES":@"NO");
        break;
    }
    
    if (self.orientationID==1) {
        canAutorotate=appIsLandscape==NO;
        return UIInterfaceOrientationPortrait;
    }
    else if(self.orientationID==2){
        canAutorotate=appIsLandscape==NO;
        return UIInterfaceOrientationPortraitUpsideDown;
    }
    else if(self.orientationID==3){
        canAutorotate=appIsLandscape==YES;
        return UIInterfaceOrientationLandscapeRight;
    }
    else if(self.orientationID==4){
        canAutorotate=appIsLandscape==YES;
        return UIInterfaceOrientationLandscapeLeft;
    }
    else{
        canAutorotate=appIsLandscape==NO;
        return UIInterfaceOrientationPortrait;
    }
}

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view.
}

-(void)backAction:(id)sender
{
    if ([self.webview canGoBack] == YES) { //如果当前H5可以返回
        //则返回上一个H5页面
        [self.webview goBack];
    }else{
        //否则回到原生页面
        [self dismissViewControllerAnimated:NO completion:nil];
    }
}

-(void)closeAction:(id)sender
{
    [self dismissViewControllerAnimated:NO completion:nil];
}

-(void)refershAction:(id)sender
{
    [self.webview reload];
}

-(void)viewDidAppear:(BOOL)animated{
    
    BOOL isIPhoneNotchScreen=[iOSUtil getIsNotch];
    NSLog(@"是否为刘海屏:%@ 刘海屏高度:%f 状态栏高度:%f",isIPhoneNotchScreen?@"YES":@"NO",[iOSUtil getNotchSize],[[UIApplication sharedApplication] statusBarFrame].size.height);
    
    //如果是刘海屏使用getNotchsize 不是刘海屏使用状态栏高度
    CGFloat startY=isIPhoneNotchScreen?[iOSUtil getNotchSize]:[[UIApplication sharedApplication] statusBarFrame].size.height;
   
    
    self.nBarView = [[UIView alloc]initWithFrame:CGRectMake(0, 0, self.view.frame.size.width,startY+36)];
    [self.nBarView setBackgroundColor:[UIColor whiteColor]];
    [self.view addSubview:self.nBarView];
    
    UIButton *button = [UIButton buttonWithType:UIButtonTypeRoundedRect];
    [button setTitle:@"返回" forState:UIControlStateNormal];
    button.backgroundColor = [UIColor whiteColor];
    button.frame = CGRectMake(0, startY, 60, 36);
    [button addTarget:self action:@selector(backAction:) forControlEvents:UIControlEventTouchUpInside];
    [self.nBarView addSubview:button];
    
    UIButton *button3 = [UIButton buttonWithType:UIButtonTypeRoundedRect];
    [button3 setTitle:@"刷新" forState:UIControlStateNormal];
    button3.backgroundColor = [UIColor whiteColor];
    button3.frame = CGRectMake(60, startY, 60, 36);
    [button3 addTarget:self action:@selector(refershAction:) forControlEvents:UIControlEventTouchUpInside];
    [self.nBarView addSubview:button3];
    
    UIButton *button2 = [UIButton buttonWithType:UIButtonTypeRoundedRect];
    [button2 setTitle:@"关闭" forState:UIControlStateNormal];
    button2.backgroundColor = [UIColor whiteColor];
    button2.frame = CGRectMake(120, startY, 60, 36);
    [button2 addTarget:self action:@selector(closeAction:) forControlEvents:UIControlEventTouchUpInside];
    [self.nBarView addSubview:button2];
    
    //进度条初始化
    self.progressView = [[UIProgressView alloc] initWithFrame:CGRectMake(0, startY, [[UIScreen mainScreen] bounds].size.width, 1)];
    self.progressView.backgroundColor = [UIColor whiteColor];
    //设置进度条的高度，下面这句代码表示进度条的宽度变为原来的1倍，高度变为原来的1.5倍.
    self.progressView.transform = CGAffineTransformMakeScale(1.0f, 1.5f);
    [self.view addSubview:self.progressView];
    
    
    //如果是刘海屏使用getNotchsize 不是刘海屏使用状态栏高度
    CGFloat viewHeight=isIPhoneNotchScreen?[iOSUtil getNotchSize]+36:[[UIApplication sharedApplication] statusBarFrame].size.height+36;
    
    //创建WKWebView对象，设置大小为屏幕大小
    self.webview = [[WKWebView alloc] initWithFrame:CGRectMake(0, viewHeight, [[UIScreen mainScreen] bounds].size.width, [[UIScreen mainScreen] bounds].size.height-viewHeight)];
    
    self.webview.navigationDelegate = self;
    
    //当前网页加载的进度，所以监听这个属性
    [self.webview addObserver:self forKeyPath:@"estimatedProgress" options:NSKeyValueObservingOptionNew context:nil];
    
    // 将WebView对象添加到当前页面当中
    [self.view addSubview:self.webview];
    
    
    if ([self._type isEqualToString:@"tuxiaocao"]){
         // 获得 webview url，请注意url单词是product而不是products，products是旧版本的参数，用错地址将不能成功提交
        NSString *appUrl = [self._url stringByAppendingString:@"?d-wx-push=1"];
        
        // 设置请求体
        NSMutableURLRequest *request = [NSMutableURLRequest requestWithURL:[NSURL URLWithString:appUrl]];
        
        // 请求方式为POST请求
        [request setHTTPMethod:@"POST"];
        [request setValue:@"application/x-www-form-urlencoded" forHTTPHeaderField:@"Content-Type"];
        
        NSString *customInfo=[NSString stringWithFormat:@"账号:%@ 机型:%@ 手机版本:iOS%@", self._phone,[iOSUtil getiPhoneType], [[UIDevice currentDevice] systemVersion]];
        NSString *body = [NSString stringWithFormat:@"nickname=%@&avatar=%@&openid=%@&customInfo=%@", self._nickname, self._avatar, self.open_id,customInfo];
        
        NSLog( @"%@", [NSString stringWithFormat:@"打开兔小巢页面 body:%@",body]);
        [request setHTTPBody:[body dataUsingEncoding:NSUTF8StringEncoding]];

        // WebView对象加载请求并且现实内容
        [self.webview loadRequest:request];
    }
    else if ([self._type isEqualToString:@"url"]){
        [self.webview loadRequest:[NSURLRequest requestWithURL:[NSURL URLWithString:self._url]]];
    }
   
}


#pragma mark - WKNavigationDelegate
- (void)webView:(WKWebView *)webView decidePolicyForNavigationAction:(WKNavigationAction *)navigationAction decisionHandler:(void (^)(WKNavigationActionPolicy))decisionHandler {
    WKNavigationActionPolicy actionPolicy = WKNavigationActionPolicyAllow;
    NSString *urlString = navigationAction.request.URL.absoluteString;
    urlString = [urlString stringByReplacingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
    
    //微信
    if ([urlString containsString:@"weixin://"]) {
        actionPolicy = WKNavigationActionPolicyCancel;
        
        //判断是否安装的微信
        BOOL isInstalled = [[UIApplication sharedApplication] canOpenURL:[NSURL URLWithString:@"weixin://"]];
        if (!isInstalled) {
            UIAlertController *alert = [UIAlertController alertControllerWithTitle:@"提示" message:@"未检测到微信客户端，请安装后重试！" preferredStyle:UIAlertControllerStyleAlert];
            [alert addAction:[UIAlertAction actionWithTitle:@"知道了" style:UIAlertActionStyleDefault handler:nil]];
            UIViewController *vc = self;//这里是我工具类中，获取当前试图控制器的方法，可自行替换
            [vc presentViewController:alert animated:YES completion:nil];
            
            //这句是必须加上的，不然会异常
            decisionHandler(actionPolicy);
            return;
        }
        
        //解决wkwebview weixin://无法打开微信客户端的处理
        NSURL *url = [NSURL URLWithString:urlString];
        if ([[UIApplication sharedApplication] respondsToSelector:@selector(openURL:options:completionHandler:)]) {
            if (@available(iOS 10.0,*)) {
                [[UIApplication sharedApplication] openURL:url options:@{UIApplicationOpenURLOptionUniversalLinksOnly: @NO} completionHandler:^(BOOL success) {
                }];
            }
            else {
                [[UIApplication sharedApplication] openURL:url];
            }
        } else {
            [[UIApplication sharedApplication] openURL:webView.URL];
        }
    }
    //拦截加载新页面
    else if (navigationAction.targetFrame == nil) {
        [webView loadRequest:navigationAction.request];
    }
    //拦截打开AppStore
    else if ([[navigationAction.request.URL host] isEqualToString:@"itunes.apple.com"] &&
        [[UIApplication sharedApplication] openURL:navigationAction.request.URL])
    {
        actionPolicy = WKNavigationActionPolicyCancel;
    }
    // else if (navigationAction.navigationType == WKNavigationTypeLinkActivated) {
    //     if ([[UIApplication sharedApplication] canOpenURL:navigationAction.request.URL]) {
    //         [[UIApplication sharedApplication] openURL:navigationAction.request.URL options:@{} completionHandler:nil];
    //     }
    //     decisionHandler(WKNavigationActionPolicyCancel);
    //     return;
    // } 
    
    //这句是必须加上的，不然会异常
    decisionHandler(actionPolicy);
}

- (void)observeValueForKeyPath:(NSString *)keyPath ofObject:(id)object change:(NSDictionary<NSString *,id> *)change context:(void *)context {
    if ([keyPath isEqualToString:@"estimatedProgress"]) {
        self.progressView.progress = self.webview.estimatedProgress;
        if (self.progressView.progress == 1) {
            /*
             *添加一个简单的动画，将progressView的Height变为1.4倍，在开始加载网页的代理中会恢复为1.5倍
             *动画时长0.25s，延时0.3s后开始动画
             *动画结束后将progressView隐藏
             */
            //__weak typeof(self) weakSelf = self;
            [UIView animateWithDuration:0.25f delay:0.3f options:UIViewAnimationOptionCurveEaseOut animations:^{
                self.progressView.transform = CGAffineTransformMakeScale(1.0f, 1.4f);
            } completion:^(BOOL finished) {
                self.progressView.hidden = YES;

            }];
        }
    }else{
        [super observeValueForKeyPath:keyPath ofObject:object change:change context:context];
    }
}

//开始加载
- (void)webView:(WKWebView *)webView didStartProvisionalNavigation:(WKNavigation *)navigation {
    NSLog(@"开始加载网页");
    //开始加载网页时展示出progressView
    self.progressView.hidden = NO;
    //开始加载网页的时候将progressView的Height恢复为1.5倍
    self.progressView.transform = CGAffineTransformMakeScale(1.0f, 1.5f);
    //防止progressView被网页挡住
    [self.view bringSubviewToFront:self.progressView];
}

//加载完成
- (void)webView:(WKWebView *)webView didFinishNavigation:(WKNavigation *)navigation {
    NSLog(@"加载完成");
    //加载完成后隐藏progressView
    //self.progressView.hidden = YES;
}

//加载失败
- (void)webView:(WKWebView *)webView didFailProvisionalNavigation:(WKNavigation *)navigation withError:(NSError *)error {
    NSLog(@"加载失败");
    //加载失败同样需要隐藏progressView
    //self.progressView.hidden = YES;
}

- (void)dealloc {
    [self.webview removeObserver:self forKeyPath:@"estimatedProgress"];
}

@end



extern "C" {
    void openTuXiaoCao_iOS(const char *url, const char *phone, const char *nickname, const char *avatar, const char *openid) {

        NSString *_url = [NSString stringWithUTF8String:url];
        NSString *_phone = [NSString stringWithUTF8String:phone];
        // 用户ID
        NSString *open_id = [NSString stringWithUTF8String:openid];
        // 昵称
        NSString *_nickname = [NSString stringWithUTF8String:nickname];
        // 头像url地址
        NSString *_avatar = [NSString stringWithUTF8String:avatar];

        TuXiaoCaoViewController *tuXiaoCaoViewController=[[TuXiaoCaoViewController alloc] init];
        tuXiaoCaoViewController._type = @"tuxiaocao";
        tuXiaoCaoViewController._url = _url;
        tuXiaoCaoViewController._phone = _phone;
        tuXiaoCaoViewController.open_id = open_id;// 用户ID
        tuXiaoCaoViewController._nickname = _nickname;// 昵称
        tuXiaoCaoViewController._avatar = _avatar;// 头像url地址
        tuXiaoCaoViewController.orientationID=1;//屏幕方向
        
        //防止弹出界面不能占满屏幕
        tuXiaoCaoViewController.modalPresentationStyle = UIModalPresentationFullScreen;
        [UnityGetGLViewController()  presentViewController:tuXiaoCaoViewController animated:false completion:nil];

    }

    void openWebView_iOS(const char *url,int orientationID) {
        NSLog(@"iOS端开始openWebView:%@ orientationID:%d",[NSString stringWithUTF8String:url],orientationID);
        NSString *_url = [NSString stringWithUTF8String:url];

        TuXiaoCaoViewController *tuXiaoCaoViewController=[[TuXiaoCaoViewController alloc] init];
        tuXiaoCaoViewController._type = @"url";
        tuXiaoCaoViewController._url = _url;
        tuXiaoCaoViewController.orientationID = orientationID;
        
        //防止弹出界面不能占满屏幕
        tuXiaoCaoViewController.modalPresentationStyle = UIModalPresentationFullScreen;
        [UnityGetGLViewController()  presentViewController:tuXiaoCaoViewController animated:false completion:nil];

    }
}
