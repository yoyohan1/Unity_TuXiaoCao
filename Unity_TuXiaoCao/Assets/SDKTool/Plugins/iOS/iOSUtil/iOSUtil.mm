#import <Foundation/Foundation.h>
#import "iOSUtil.h"
#import <sys/utsname.h>
#import <AVFoundation/AVFoundation.h>
 
@implementation iOSUtil

+ (long long)getDiskFreeSize {
    
    unsigned long long diskSize = -1;
    
    if (@available(iOS 11.0, *)) {
        NSURL *fileURL = [[NSURL alloc] initFileURLWithPath:NSTemporaryDirectory()];
        NSDictionary *results = [fileURL resourceValuesForKeys:@[NSURLVolumeAvailableCapacityForImportantUsageKey] error:nil];
        NSLog(@"剩余可用空间:%@",results[NSURLVolumeAvailableCapacityForImportantUsageKey]);
        //ios11 除以1000是正确的
        diskSize=[results[NSURLVolumeAvailableCapacityForImportantUsageKey] unsignedLongLongValue]*1.0/(1000*1000);
    } else {
//        NSDictionary *fsAttr = [[NSFileManager defaultManager] attributesOfFileSystemForPath:NSHomeDirectory() error:nil];
//         diskSize = [[fsAttr objectForKey:NSFileSystemFreeSize] longLongValue]/(1024*1024);
        
        /// 总大小
        float totalsize = 0.0;
        /// 剩余大小
        float freesize = 0.0;
        /// 是否登录
        NSError *error = nil;
        NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
        NSDictionary *dictionary = [[NSFileManager defaultManager] attributesOfFileSystemForPath:[paths lastObject] error: &error];
        if (dictionary)
        {
            NSNumber *_free = [dictionary objectForKey:NSFileSystemFreeSize];
            freesize = [_free unsignedLongLongValue]*1.0/(1000);
            
            NSNumber *_total = [dictionary objectForKey:NSFileSystemSize];
            totalsize = [_total unsignedLongLongValue]*1.0/(1000);
            
            NSLog(@"totalsize = %.2f, freesize = %f",totalsize/1000, freesize/1000);
            diskSize=freesize*1.0/1000;
        } else
        {
            NSLog(@"Error Obtaining System Memory Info: Domain = %@, Code = %ld", [error domain], (long)[error code]);
        }
        
    }
    
    return diskSize;
}

@end

extern "C" {
    
    /*os 的native code 处理的字符串一般是NSString，要作为plugin返回给unity的话必须要转换成char *，并且要分配内存，因为mono会释放这个内存，否则会报错。*/
    char* MakeStringCopy_iOSUtil (const char* string){
        if (string == NULL)
            return NULL;
        char* res = (char*)malloc(strlen(string) + 1);
        strcpy(res, string);
        return res;
    }

    long getDiskFreeSize_iOS(){
    	NSLog(@"iOS端开始getDiskFreeSize_iOS");
        return (long)[iOSUtil getDiskFreeSize];
    }

    void openUrl_iOS(const char *url){
        NSLog(@"iOS端开始原生openUrl:%@",[NSString stringWithUTF8String:url]);
        [[UIApplication sharedApplication]openURL:[NSURL URLWithString:[NSString stringWithUTF8String:url]]];
    }

	void openMuteAudio_iOS(){
		NSLog(@"iOS端开始openMuteAudio_iOS");
        [[AVAudioSession sharedInstance] setCategory:AVAudioSessionCategoryPlayback withOptions:AVAudioSessionCategoryOptionAllowBluetooth error:nil];
        [[AVAudioSession sharedInstance] setActive:YES error:nil];
    }

    void closeMuteAudio_iOS(){
    	NSLog(@"iOS端开始closeMuteAudio_iOS");
        //如果有注册remote得把remote也注销 在应用杀死的情况下停掉播放…
        [[UIApplication sharedApplication] endReceivingRemoteControlEvents];
    }
}

