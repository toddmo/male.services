package services.male.android;

import android.app.Notification;
import android.app.NotificationManager;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.telephony.SmsMessage;
import android.util.Log;
import android.widget.Toast;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Locale;
import java.util.concurrent.ExecutionException;

import microsoft.aspnet.signalr.client.Action;
import microsoft.aspnet.signalr.client.Credentials;
import microsoft.aspnet.signalr.client.ErrorCallback;
import microsoft.aspnet.signalr.client.SignalRFuture;
import microsoft.aspnet.signalr.client.http.Request;
import microsoft.aspnet.signalr.client.hubs.HubConnection;
import microsoft.aspnet.signalr.client.hubs.SubscriptionHandler1;
import microsoft.aspnet.signalr.client.hubs.HubProxy;
import microsoft.aspnet.signalr.client.transport.ClientTransport;
import microsoft.aspnet.signalr.client.transport.ServerSentEventsTransport;


public class SmsReceiver extends BroadcastReceiver {

    private SharedPreferences preferences;

    @Override
    public void onReceive(Context context, Intent intent) {
        if(intent.getAction().equals("android.provider.Telephony.SMS_RECEIVED")){
            Bundle bundle = intent.getExtras();
            SmsMessage[] msgs = null;
            String msg_from;
            String msg_body;
            if (bundle != null){
                try {
                    Object[] pdus = (Object[]) bundle.get("pdus");
                    msgs = new SmsMessage[pdus.length];
                    for (int i = 0; i < msgs.length; i++) {
                        msgs[i] = SmsMessage.createFromPdu((byte[]) pdus[i]);

                        msg_from = msgs[i].getOriginatingAddress();
                        msg_body = msgs[i].getMessageBody();

                        //Let us also show a notification
                        Notification notification = new Notification.Builder(context)
                                .setContentTitle("SMS Received")
                                .setContentText(msg_body)
                                .setSmallIcon(R.drawable.ic_menu_share)
                                .build();
                        notification.flags |= Notification.FLAG_ONGOING_EVENT;
                        NotificationManager notifier = (NotificationManager) context.getSystemService(Context.NOTIFICATION_SERVICE);
                        int SMS_NOTIFICATION = 2;
                        notifier.notify(SMS_NOTIFICATION, notification);

                        //HubConnection hubConnection = new HubConnection ( "http://localtest:54221/signalr/" );
                        HubConnection hubConnection = new HubConnection("http://male.services/signalr/");
                        //HubConnection hubConnection = new HubConnection ( "http://10.0.2.2:54221/signalr/" );
                        Credentials credentials = request -> request.addHeader("User-Name", "todd");
                        hubConnection.setCredentials(credentials);
                        HubProxy hubProxy = hubConnection.createHubProxy("ChatHub");

                        ClientTransport clientTransport = new ServerSentEventsTransport(hubConnection.getLogger());
                        SignalRFuture<Void> signalRFuture = hubConnection.start(clientTransport);

                        try {
                            signalRFuture.get(); // this works fine
                        } catch (InterruptedException | ExecutionException e) {
                            Log.e("SimpleSignalR", e.toString());
                        }

                        try{
                            hubProxy.invoke("Send", msg_from, msg_body ).get();
                        }
                        catch (Exception e){
                            Log.e("send caught","" + e.getMessage());
                            return  ;
                        }
                    }
                }
                catch(Exception e) {
                    Log.e("sdfsd caught","" + e.getMessage());
                }
            }
        }
    }

    // Method to convert current long type timestamp to human readable format - Easter egg ;)
    public String getReadableDate() {
        long currentTimeMilliseconds= System.currentTimeMillis();
        SimpleDateFormat df = new SimpleDateFormat("MMM dd yyyy HH:mm", Locale.getDefault());
        Date resultDate = new Date(currentTimeMilliseconds);
        return df.format(resultDate);
    }
}

