//
//  TuXiaoCaoViewController.h


#import <UIKit/UIKit.h>
#import "WebKit/WebKit.h"

@interface TuXiaoCaoViewController : UIViewController

@property (nonatomic) NSString * _url;
@property (nonatomic) NSString * _phone;
@property (nonatomic) NSString * open_id;
@property (nonatomic) NSString * _nickname;
@property (nonatomic) NSString * _avatar;
@property (nonatomic,strong) UIView *nBarView;
@property (nonatomic,strong) WKWebView *webview;
@property (nonatomic, strong) UIProgressView *progressView;

@end
