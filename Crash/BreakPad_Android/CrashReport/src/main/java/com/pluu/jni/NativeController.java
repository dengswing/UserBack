package com.pluu.jni;

import android.util.Log;

import com.shinezone.crashreport.JniActivity;
import com.shinezone.crashreport.UploadThread;
import java.sql.Timestamp;

import java.io.File;
import java.security.MessageDigest;
import java.io.UnsupportedEncodingException;
import java.security.NoSuchAlgorithmException;
import java.util.Collection;
import java.util.Map;
import java.util.Set;
import java.util.HashMap;

/**
 * Created by PLUUSYSTEM on 2015-06-11.
 */
public class NativeController {
    public static int NativeCrashCallback(String fileName) {
        Log.i("CarshReport", "NativeController called");

        //正式服
        String uploadUrl = "http://api.shinezone.com/1.0/crash_log/upload_crash_log/";
        //测试服
        //String uploadUrl = "http://api-test.shinezone.com/1.0/crash_log/upload_crash_log/";

        //String uploadUrl = "http://172.17.69.40:8080/upload/UploadServlet";
      //  String uploadUrl = "http://172.17.72.37:8000/crash_log/upload_crash_log";

        JniActivity jni = JniActivity.getInstance();
        String cpId =jni.getCpid();
        String cpKey =jni.getCpKey();
        String szId =jni.getSzId();
        String gameId=jni.getGameId();
        String info="crashreport";
        String model="model";

        Timestamp now = new Timestamp(System.currentTimeMillis());//获取系统当前时间
        long timestamp = now.getTime()/1000;
        String sign =md5(cpId+timestamp+cpKey);

        Map<String, String> urlParams = new HashMap<String, String>();
        urlParams.put("cp_id",cpId);
        urlParams.put("timestamp",String.valueOf(timestamp));
        urlParams.put("sign",sign);
        urlParams.put("game_id", gameId);
        urlParams.put("sz_id",szId);
        urlParams.put("crash_info",info);
        urlParams.put("crash_model",model);
/*
                ="; cp_id=\"" + cpId+"\"";
        urlParams = urlParams + "; timestamp=\""+timestamp+"\"";
        urlParams = urlParams + "; sign=\""+sign+"\"";
        urlParams = urlParams + "; game_id=\""+gameId+"\"";
        urlParams = urlParams + "; sz_id=\""+szId+"\"";
        urlParams = urlParams + "; crash_info=\""+info+"\"";
        urlParams = urlParams + "; crash_model=\""+model+"\""; */

        Log.i("CarshReport", "file="+ fileName);
        Log.i("CarshReport", "params="+urlParams);

        File dir = new File(fileName);
        UploadThread load = new UploadThread(dir,uploadUrl,urlParams);

        return 0;
    }

    public static String md5(String string) {
        byte[] hash;
        try {
            hash = MessageDigest.getInstance("MD5").digest(string.getBytes("UTF-8"));
        } catch (NoSuchAlgorithmException e) {
            throw new RuntimeException("Huh, MD5 should be supported?", e);
        } catch (UnsupportedEncodingException e) {
            throw new RuntimeException("Huh, UTF-8 should be supported?", e);
        }

        StringBuilder hex = new StringBuilder(hash.length * 2);
        for (byte b : hash) {
            if ((b & 0xFF) < 0x10) hex.append("0");
            hex.append(Integer.toHexString(b & 0xFF));
        }
        return hex.toString();
    }
}