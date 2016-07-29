package com.example.clientprinter;

import android.app.Notification;
import android.app.NotificationManager;
import android.content.Context;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;

import java.io.IOException;
import java.net.InetAddress;
import java.net.Socket;

public class MainActivity extends AppCompatActivity {


    public static final int SERVERPORT = 1994;
    private Button button1;
    private Button button2;
    private EditText etext;
    public static String ip;
    public Notification.Builder builder;
    //TaskStackBuilder stackBuilder = TaskStackBuilder.create(this);

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        button1 = (Button)findViewById(R.id.button);
        button2 = (Button)findViewById(R.id.button2);
        etext = (EditText)findViewById(R.id.editText);
        builder = new Notification.Builder(this);
        builder.setSmallIcon(R.mipmap.ic_launcher);

    }

    protected void button1click(View view) throws IOException {
        ip=etext.getText().toString();
        new Thread(new Runnable() {
            @Override
            public void run() {
                while (true) {
                    try {
                        InetAddress serverAddr = InetAddress.getAllByName(ip)[0];
                        Socket Client = new Socket(serverAddr, SERVERPORT);

                        String Request = "";
                        byte[] Buffer = new byte[1024];
                        Client.getInputStream().read(Buffer, 0, Buffer.length);
                        // Преобразуем эти данные в строку и добавим ее к переменной Request
                        Request = new String(Buffer);

                        String[] astr = Request.split(";");
                        for (int i = 0; i < astr.length; i++) {
                            builder.setContentText(astr[i].split(":")[1]);
                            builder.setContentTitle(astr[i].split(":")[0]);

                            NotificationManager mNotificationManager = (NotificationManager) getSystemService(Context.NOTIFICATION_SERVICE);
                            mNotificationManager.notify(i, builder.build());
                        }
                    } catch (Exception e) {

                    }
                }
            }
        }).start();


    }


}
