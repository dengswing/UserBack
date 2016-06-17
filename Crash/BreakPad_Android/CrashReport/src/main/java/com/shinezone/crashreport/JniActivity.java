package com.shinezone.crashreport;

import android.content.Context;
import android.os.Environment;
import android.util.Log;

import com.pluu.jni.NativeController;

import java.io.File;
import java.util.ArrayList;

public class JniActivity {

    static {
        System.loadLibrary("test_google_breakpad");
    }

    private static JniActivity instance;
    private JniActivity (){}
    public static JniActivity getInstance() {
        if (instance == null)
        {
            instance = new JniActivity();
         }
        return instance;
    }


    native void initNative(String path);
    native void crashService();

    private String _cpId;
    private String _cpKey;
    private String _szId;
    private String _gameId;
    private String _userId;
    private String _dumpPath;


    public void setCpid(String value) {
        _cpId = value;
    }
    public String getCpid() {
        return _cpId;
    }

    public void setCpKey(String value) {
        _cpKey = value;
    }
    public String getCpKey() {
        return _cpKey;
    }

    public void setSzId(String value) {
        _szId = value;
    }
    public String getSzId() {
        return _szId;
    }

    public void setGameId(String value) {
        _gameId = value;
    }
    public String getGameId() {
        return _gameId;
    }

    public void setUserId(String value) {
        _userId = value;
    }
    public String getUserId() {
        return _userId;
    }

    public void setDumpPath(String value) {
        _dumpPath = value;
    }
    public String getDumpPath() {
        return _dumpPath;
    }

    public void init(String gameID,String cpId,String cpKey,String szId,String userID,String dumpPath, Context context)
    {
        setSzId(szId);
        setCpKey(cpKey);
        setCpid(cpId);
        setGameId(gameID);
        setDumpPath(dumpPath);
        setUserId(userID);

        String path = dumpPath;   // Save Dump Path
        if(path == null)
            path = getDiskCacheDir(context);

        initNative(path);

        CrashHandler crashHandler = CrashHandler.getInstance();
        crashHandler.init(context, path);

        Log.i("CarshReport", "JniActivity init");
        Log.i("CarshReport", path);

        uploadFile(path);
    }

    private void uploadFile(String path)
    {

        ArrayList<String> paths = getFile(path);
        int count = paths.size();

        for (int i = 0; i < count; i++) {
            NativeController.NativeCrashCallback(paths.get(i));
        }
    }

    private ArrayList<String> getFile(String filePath) {
        ArrayList<String> paths = new ArrayList<String>();
        File f = new File(filePath);
        File[] files = f.listFiles();// 列出所有文件
        // 将所有文件存入list中
        if (files != null) {
            int count = files.length;// 文件个数
            Log.i("CarshReport", "file size=" + count);
            for (int i = 0; i < count; i++) {
                File file = files[i];
                paths.add(file.getPath());
            }
        }

        return paths;
    }

    private String getDiskCacheDir(Context context) {
        String cachePath = null;
        if (Environment.MEDIA_MOUNTED.equals(Environment.getExternalStorageState())
                || !Environment.isExternalStorageRemovable()) {
            cachePath = context.getExternalCacheDir().getPath();
        } else {
            cachePath = context.getCacheDir().getPath();
        }
        return cachePath;
    }

    public void crashClick() {
        crashService();
    }

}

