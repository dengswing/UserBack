//
//  UploadFile.m
//  crashReport
//
//  Created by shinezone on 16/6/12.
//  Copyright © 2016年 fs. All rights reserved.
//

#import "UploadFile.h"
#import <Foundation/Foundation.h>

NSString* const kFormBoundary = @"+++++formBoundary";
NSString* const fileKey = @"file";
NSString* const mimeType= @"text/plain";

#define YYEncode(str) [str dataUsingEncoding:NSUTF8StringEncoding]

@implementation UploadFile

void uploadDump(NSString *upUrl, NSString *name,NSString *filename,NSString *mimeType,NSData *data,NSDictionary *params)
{
    // 文件上传
    NSURL *url = [NSURL URLWithString:upUrl];
    NSMutableURLRequest *request = [NSMutableURLRequest requestWithURL:url];
    request.HTTPMethod = @"POST";
    
    // 设置请求体
    NSMutableData *body = [NSMutableData data];
    
    /***************文件参数***************/
    // 参数开始的标志
    [body appendData:YYEncode(@"--YY\r\n")];
    // name : 指定参数名(必须跟服务器端保持一致)
    // filename : 文件名
    NSString *disposition = [NSString stringWithFormat:@"Content-Disposition: form-data; name=\"%@\"; filename=\"%@\"\r\n", name, filename];
    [body appendData:YYEncode(disposition)];
    NSString *type = [NSString stringWithFormat:@"Content-Type: %@\r\n", mimeType];
    [body appendData:YYEncode(type)];
    
    [body appendData:YYEncode(@"\r\n")];
    [body appendData:data];
    [body appendData:YYEncode(@"\r\n")];
    
    /***************普通参数***************/
    [params enumerateKeysAndObjectsUsingBlock:^(id key, id obj, BOOL *stop) {
        // 参数开始的标志
        [body appendData:YYEncode(@"--YY\r\n")];
        NSString *disposition = [NSString stringWithFormat:@"Content-Disposition: form-data; name=\"%@\"\r\n", key];
        [body appendData:YYEncode(disposition)];
        
        [body appendData:YYEncode(@"\r\n")];
        [body appendData:YYEncode(obj)];
        [body appendData:YYEncode(@"\r\n")];
    }];
    
    /***************参数结束***************/
    // YY--\r\n
    [body appendData:YYEncode(@"--YY--\r\n")];
    request.HTTPBody = body;
    
    // 设置请求头
    // 请求体的长度
    [request setValue:[NSString stringWithFormat:@"%zd", body.length] forHTTPHeaderField:@"Content-Length"];
    // 声明这个POST请求是个文件上传
    [request setValue:@"multipart/form-data; boundary=YY" forHTTPHeaderField:@"Content-Type"];
    
    // 发送请求
    [NSURLConnection sendAsynchronousRequest:request queue:[NSOperationQueue mainQueue] completionHandler:^(NSURLResponse *response, NSData *data, NSError *connectionError) {
        if (data) {
            NSDictionary *dict = [NSJSONSerialization JSONObjectWithData:data options:NSJSONReadingMutableLeaves error:nil];
            NSLog(@"%@", dict);
        } else {
            NSLog(@"上传失败");
        }
    }];
}

- (void)Upload:(NSString *)upLoadUrl filePath:(NSString *)filePath cpId:(NSString *)cpId timestamp:(NSString *)timestamp sign:(NSString *)sign gameId:(NSString *)gameId szId:(NSString *)szId crashInfo:(NSString *)crashInfo crashModel:(NSString *)crashModel
{
    // Socket 实现断点上传
    
    //apache-tomcat-6.0.41/conf/web.xml 查找 文件的 mimeType
    //    UIImage *image = [UIImage imageNamed:@"test"];
    //    NSData *filedata = UIImagePNGRepresentation(image);
    //    [self upload:@"file" filename:@"test.png" mimeType:@"image/png" data:filedata parmas:@{@"username" : @"123"}];
    
    // 给本地文件发送一个请求
    NSURL *fileurl = [NSURL URLWithString:filePath];
    NSURLRequest *request = [NSURLRequest requestWithURL:fileurl];
    NSURLResponse *repsonse = nil;
    NSData *data = [NSURLConnection sendSynchronousRequest:request returningResponse:&repsonse error:nil];
    
    uploadDump(upLoadUrl, @"file", @"crashReport.dmp", repsonse.MIMEType, data, @{@"cp_id":cpId,
                                                                           @"timestamp":timestamp,
                                                                           @"sign":sign,
                                                                           @"game_id":gameId,
                                                                           @"sz_id":szId,
                                                                           @"crash_info":crashInfo,
                                                                           @"crash_model":crashModel});
    //删除文件
    [[NSFileManager defaultManager] removeItemAtPath:filePath error:nil];
}


@end
