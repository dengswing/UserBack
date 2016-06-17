#import "CrashReport.h"
#import "client/ios/BreakpadController.h"

#import "UploadFile.h"
#import <CommonCrypto/CommonDigest.h>


@implementation CrashReport

-(void)Init:(NSString *)gameId cpId:(NSString *)cpId cpKey:(NSString *)cpKey szId:(NSString *)szId userId:(NSString *)userId
{
    [[BreakpadController sharedInstance] start : YES];
    [[BreakpadController sharedInstance] setUploadingEnabled : NO];
    
    //NSString *upLoadUrl =@"http://api.shinezone.com/1.0/crash_log/upload_crash_log/";
    
    NSString *upLoadUrl =@"http://api-test.shinezone.com/1.0/crash_log/upload_crash_log/";
    NSString *timestamp= getTime();
    NSString *sValue =cpId;
    sValue = [sValue stringByAppendingString:timestamp];
    sValue = [sValue stringByAppendingString:cpKey];
    NSLog(@"svalue=%@",sValue);
    
    NSString *sign= md5(sValue);
    NSString *crashInfo= @"crashreport";
    NSString *crashModel= @"model";
    
    NSLog(@"init upLoadUrl=%@ |cpId=%@ |timestamp=%@ |sign=%@ |gameId=%@ |szId=%@",upLoadUrl,cpId,timestamp,sign,gameId,szId);
    
    
    NSMutableArray *allDumpFile =dumpFilePath();
    
    int count = allDumpFile.count;//减少调用次数
    for( int i=0; i<count; i++){
        NSString *filePath = [allDumpFile objectAtIndex:i];
        
        [[UploadFile alloc] Upload:upLoadUrl filePath:filePath cpId:cpId timestamp:timestamp sign:sign gameId:gameId szId:szId crashInfo:crashInfo crashModel:crashModel];
    }
}

NSMutableArray* dumpFilePath()
{
    NSString *cachePath =
    [NSSearchPathForDirectoriesInDomains(NSCachesDirectory,
                                         NSUserDomainMask,
                                         YES)
     objectAtIndex:0];
    
    cachePath= [cachePath stringByAppendingString:@"/Breakpad"];
    
    NSFileManager* fm=[NSFileManager defaultManager];
    NSArray *files = [fm subpathsAtPath: cachePath];
    NSLog(@"all file %@",files);
    
    NSMutableArray *dumpFiles = [NSMutableArray arrayWithCapacity:6];
    int count = files.count;//减少调用次数
    for( int i=0; i<count; i++){
        NSString *filePath=[files objectAtIndex:i];
        NSRange range = [filePath rangeOfString:@".dmp"];//判断字符串是否包含
        
        NSString *dumpPath =cachePath;
        dumpPath = [dumpPath stringByAppendingString:@"/"];
        dumpPath = [dumpPath stringByAppendingString:filePath];
        
        if (range.length >0)//包含
        {
            NSLog(@"%i-%@", i, dumpPath);
            [dumpFiles addObject:dumpPath];
        }else
        {  //删除无用文件
            [[NSFileManager defaultManager] removeItemAtPath:dumpPath error:nil];
        }
    }
    
    return dumpFiles;
}

NSString* getTime()
{
    NSDate* dat = [NSDate dateWithTimeIntervalSinceNow:0];
    NSTimeInterval a=[dat timeIntervalSince1970];
    NSString *timeString = [NSString stringWithFormat:@"%.0f", a]; //转为字符型
    
    return timeString;
}

NSString* md5(NSString *inPutText)
{
    const char *cStr = [inPutText UTF8String];
    unsigned char result[CC_MD5_DIGEST_LENGTH];
    CC_MD5(cStr, strlen(cStr), result);
    
    return [[NSString stringWithFormat:@"%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X",
             result[0], result[1], result[2], result[3],
             result[4], result[5], result[6], result[7],
             result[8], result[9], result[10], result[11],
             result[12], result[13], result[14], result[15]
             ] lowercaseString];
}


-(void) crash
{
    volatile int* a = (int*)(NULL); *a = 1;
}


-(void)Stop{
    [[BreakpadController sharedInstance] stop];
}

@end

