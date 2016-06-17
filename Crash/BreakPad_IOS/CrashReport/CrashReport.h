//
//  CrashReport.h
//  CrashReport
//
//  Created by shinezone on 16/5/26.
//  Copyright © 2016年 fs. All rights reserved.
//
@class CrashReport;

#import <UIKit/UIKit.h>
#import <Foundation/Foundation.h>
#import <UIKit/UIApplication.h>

@interface CrashReport : NSObject

@property (retain) NSString *cpId;
@property (retain) NSString *cpKey;
@property (retain) NSString *szId;
@property (retain) NSString *gameId;
@property (retain) NSString *userId;

-(void) Init:(NSString *)gameId cpId:(NSString *)cpId cpKey:(NSString *)cpKey szId:(NSString *)szId userId:(NSString *)userId;

-(void) crash;

-(void) Stop;

@end