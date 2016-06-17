//
//  Header.h
//  crashReport
//
//  Created by shinezone on 16/6/12.
//  Copyright © 2016年 fs. All rights reserved.
//

@class UploadFile;

#import <UIKit/UIKit.h>
#import <Foundation/Foundation.h>
#import <UIKit/UIApplication.h>

@interface UploadFile : NSObject

-(void) Upload:(NSString *)upLoadUrl filePath:(NSString *)filePath cpId:(NSString *)cpId timestamp:(NSString *)timestamp sign:(NSString *)sign gameId:(NSString *)gameId szId:(NSString *)szId crashInfo:(NSString *)crashInfo crashModel:(NSString *)crashModel;

@end