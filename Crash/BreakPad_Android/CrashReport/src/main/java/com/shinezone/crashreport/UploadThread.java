package com.shinezone.crashreport;

import java.io.File;
import java.util.Map;

/**
 * Created by shinezone on 16/6/2.
 */

public class UploadThread // extends  Thread
{
    private String requestUrl;
    private File mFile;
    public String result;
    private Map<String, String> urlParams;

    public UploadThread(File file, String requestUrl,Map<String, String> urlParams)
    {
        mFile = file;
        this.requestUrl=requestUrl;
        this.urlParams = urlParams;

        new Thread(networkTask).start();
    }

    Runnable networkTask = new Runnable() {
        @Override
        public void run() {
            // 在这里进行 http request.网络请求相关操作
            result = UploadUtil.uploadFile(requestUrl , mFile,urlParams );
        }
    };
}