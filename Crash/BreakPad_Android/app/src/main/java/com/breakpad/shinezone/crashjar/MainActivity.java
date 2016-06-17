package com.breakpad.shinezone.crashjar;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.CompoundButton;

import com.shinezone.crashreport.JniActivity;
import com.shinezone.crashreport.UploadUtil;

public class MainActivity extends AppCompatActivity {

    JniActivity test;


    private CheckBox cbxDelFile;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);


        String cpId ="5";
        String cpKey ="tLJy&sk3k94Q";
        String szId ="sz_123";
        String gameId="71";
        String userId="11";

        test= JniActivity.getInstance();
        test.init(gameId,cpId,cpKey,szId,userId,null,this);

        cbxDelFile = (CheckBox)findViewById(R.id.cbxDelFile);

        cbxDelFile.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                UploadUtil.bDelFile = isChecked;
            }
        });

        Button logBtn = (Button) findViewById(R.id.btnCrashNative);
        logBtn.setOnClickListener(OnCrashNative);


        Button btnJavaCrash = (Button) findViewById(R.id.btnCrashJava);
        btnJavaCrash.setOnClickListener(OnCrashJava);
    }

    View.OnClickListener OnCrashNative =new View.OnClickListener()
    {
        public void onClick(View v)
        {
          test.crashClick();
            //int k = 10 / 0;
        }
    };

    View.OnClickListener OnCrashJava =new View.OnClickListener()
    {
        public void onClick(View v)
        {

            int k = 10 / 0;
        }
    };
}
