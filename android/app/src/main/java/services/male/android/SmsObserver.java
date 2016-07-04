package services.male.android;

import android.content.ContentResolver;
import android.database.ContentObserver;
import android.database.Cursor;
import android.net.Uri;
import android.os.Handler;

/**
 * Created by tmorrow on 5/31/2016.
 */
public class SmsObserver extends ContentObserver {

    public SmsObserver(Handler handler) {
        super(handler);
    }

    @Override
    public void onChange(boolean selfChange) {
        super.onChange(selfChange);
        // save the message to the SD card here
        Uri uriSMSURI = Uri.parse("content://sms/out");
        ContentResolver contentResolver = this.getApplicationContext().getContentResolver();
        Cursor cur = this.getContentResolver().query(uriSMSURI, null, null, null, null);
        // this will make it point to the first record, which is the last SMS sent
        cur.moveToNext();
        String content = cur.getString(cur.getColumnIndex("body"));
// use cur.getColumnNames() to get a list of all available columns...
// each field that compounds a SMS is represented by a column (phone number, status, etc.)
// then just save all data you want to the SDcard :)
    }
}