package com.shinezone.crashreport;

/**
 * Created by hc on 2016/5/30.
 */

import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;
import java.net.URLConnection;
import java.util.UUID;

import android.util.Log;
import com.alibaba.fastjson.JSON;
import com.alibaba.fastjson.JSONObject;
import java.util.Map;


public class UploadUtil
{
    public static boolean bDelFile=false;

    private static final String TAG = "uploadFile";
    private static final int TIME_OUT = 10*1000;   //超时时间
    private static final String CHARSET = "utf-8"; //设置编码


    public static String uploadFile(String url, File file,Map<String, String> urlParams)
    {
        String result = uploadFile(file, url,urlParams);
        return result;
    }


    public static String uploadFile(File file,String uploadUrl,Map<String, String> textParams)
    {
        String filename = file.getPath();
        String result="";
        //String uploadUrl = "http://172.17.69.40:8080/upload/UploadServlet";
        String end = "\r\n";
        String twoHyphens = "--";
        //String boundary = "******";

        final String BOUNDARY =  UUID.randomUUID().toString();
        final String PREFIX = "--";
        final String LINE_END = "\r\n";

        Log.i("CrashReport",uploadUrl);
        Log.i("CrashReport",filename);

        String Params = "Content-Disposition: form-data; name=\"file\"; filename=\""
                + filename.substring(filename.lastIndexOf("/") + 1)
                + "\""
                + end;

        Log.i("CrashReport",Params);

        //uploadFile(filename,uploadUrl,filename.substring(filename.lastIndexOf("/") + 1),urlParams);

        try
        {
            URL url = new URL(uploadUrl);

            HttpURLConnection httpURLConnection = (HttpURLConnection) url
                    .openConnection();
            httpURLConnection.setDoInput(true);
            httpURLConnection.setDoOutput(true);
            httpURLConnection.setUseCaches(false);
            httpURLConnection.setRequestMethod("POST");
            httpURLConnection.setRequestProperty("Connection", "Keep-Alive");
            httpURLConnection.setRequestProperty("Charset", "UTF-8");
            httpURLConnection.setRequestProperty("Content-Type",
                    "multipart/form-data;boundary=" + BOUNDARY);

            // 拼接文本类型的参数
            StringBuilder textSb = new StringBuilder();
            if (textParams != null) {
                for (Map.Entry<String, String> entry : textParams.entrySet()) {
                    textSb.append(PREFIX).append(BOUNDARY).append(LINE_END);
                    //textSb.append(BOUNDARY).append(LINE_END);
                    textSb.append("Content-Disposition: form-data; name=\"" + entry.getKey() + "\"" + LINE_END);
//                    textSb.append("Content-Type: text/plain; charset=" + CHARSET + LINE_END);
//                    textSb.append("Content-Transfer-Encoding: 8bit" + LINE_END);
                    textSb.append(LINE_END);
                    textSb.append(entry.getValue());
                    //textSb.append(LINE_END);
                }
            }

            OutputStream outputStream = httpURLConnection.getOutputStream();
            DataOutputStream dos = new DataOutputStream(outputStream);

            dos.writeBytes(textSb.toString());

            dos.writeBytes( twoHyphens + BOUNDARY + end);
            dos.writeBytes(Params);
            dos.writeBytes(end);

            FileInputStream fis = new FileInputStream(filename);
            byte[] buffer = new byte[1024]; // 8k
            int count = 0;
            while ((count = fis.read(buffer)) != -1)
            {
                dos.write(buffer, 0, count);

            }
            fis.close();

            dos.writeBytes(end);
            dos.writeBytes(twoHyphens+ BOUNDARY + twoHyphens + end);
            dos.flush();

//            Log.i("CrashReport", outputStream.toString());
            InputStream is = httpURLConnection.getInputStream();
            InputStreamReader isr = new InputStreamReader(is, "utf-8");
            BufferedReader br = new BufferedReader(isr);
            result = br.readLine();

            //Test
            //Toast.makeText(this, result, Toast.LENGTH_LONG).show();
            dos.close();
            is.close();

            Log.i("CrashReport", "upload sucess");

            if(bDelFile)
            {
                deleteFile(file);
            }


            return result;
        }
        catch (Exception e)
        {
            Log.i("CrashReport", "upload errror");
            e.printStackTrace();
        }
        return result;
    }

    public static void deleteFile(File file) {
        if (file.exists()) { // 判断文件是否存在
            if (file.isFile()) { // 判断是否是文件
                file.delete(); // delete()方法 你应该知道 是删除的意思;
            } else if (file.isDirectory()) { // 否则如果它是一个目录
                File files[] = file.listFiles(); // 声明目录下所有的文件 files[];
                for (int i = 0; i < files.length; i++) { // 遍历目录下所有的文件
                    deleteFile(files[i]); // 把每个文件 用这个方法进行迭代
                }
            }
            file.delete();
        } else {
            Log.i(TAG,"文件不存在！");
        }
    }
}
