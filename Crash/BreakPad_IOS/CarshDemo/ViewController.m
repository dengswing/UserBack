//
//  ViewController.m
//  CarshDemo
//
//  Created by shinezone on 16/5/27.
//  Copyright © 2016年 fs. All rights reserved.
//

#import "ViewController.h"
#import "CrashReport.h"

@interface ViewController ()

@end

CrashReport *crash;

@implementation ViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view, typically from a nib.
    
    NSString *cpId =@"5";
    NSString *cpKey =@"tLJy&sk3k94Q";
    NSString *szId =@"sz_123";
    NSString *gameId=@"71";
    NSString *userId=@"11";
    
    crash = [CrashReport alloc];
    crash.userId = @"11";
    [crash Init:gameId cpId:cpId cpKey:cpKey szId:szId userId:userId];
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

- (IBAction)clickCrash:(id)sender {
    int * p = NULL;
    *p = 100;
    
   // [crash crash];
}

@end
