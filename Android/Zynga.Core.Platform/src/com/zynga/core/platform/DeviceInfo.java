package com.zynga.core.platform;

import android.util.DisplayMetrics;
import android.util.Log;
import android.view.Display;
import android.os.Build;
import android.os.Debug;
import android.app.ActivityManager;
import android.content.Context;
import android.content.SharedPreferences;
import android.provider.Settings;
import android.telephony.TelephonyManager;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.provider.Settings.Secure;
import android.support.v4.app.NotificationManagerCompat;
import android.os.StatFs;
import android.os.Environment;
import com.google.android.gms.ads.identifier.AdvertisingIdClient;
import com.google.android.gms.ads.identifier.AdvertisingIdClient.Info;
import com.google.android.gms.common.GooglePlayServicesNotAvailableException;
import com.google.android.gms.common.GooglePlayServicesRepairableException;
import java.io.IOException;
import java.lang.Runtime;
import java.util.Locale;
import java.util.TimeZone;
import java.util.Set;
import com.unity3d.player.UnityPlayer;

/**
 * Class to provide all device info specific methods on Android device in Unity application
 */
public class DeviceInfo {

    private final static double TABLET_SCREEN_SIZE = 6.0;
    private final static double MEGABYTE = 1048576.0;
    
    private final static String unknownValue = "Unknown";
    
    // This is required to simulate exceptions in Android Java code
    private static boolean simulateExceptions = false;
    
    public static void SimulateExceptions(boolean on) {
        simulateExceptions = on;
    }
    
    private static void SimulateExceptionWithUnitTests() throws NoSuchMethodException {
        if (simulateExceptions)
        {
            throw new java.lang.NoSuchMethodException("Simulated exception");
        }
    }
    
    private static String GetExceptionMessage(Throwable e)
    {
        // Method Throwable.getMessage() can return null. See Java documentation.
        return e != null && e.getMessage() != null ? e.getMessage() : "";
    }
    
    // Indicates if the application can receive remote notifications.
    public static DeviceInfoResult Zynga_Core_Platform_AreRemoteNotificationsEnabled() {
        try
        {
            SimulateExceptionWithUnitTests();
            Set<String> enabledListeners = NotificationManagerCompat.from(UnityPlayer.currentActivity).getEnabledListenerPackages(UnityPlayer.currentActivity);
            return new DeviceInfoResult(enabledListeners.contains(UnityPlayer.currentActivity.getPackageName()), null);
        }
        catch (Throwable e)
        {
            Log.e("AreRemoteNotificationsEnabled", GetExceptionMessage(e));
            return new DeviceInfoResult(false, e);
        }
    }
    
    // Indicates if this application notifications are displayed to the user.
    public static DeviceInfoResult Zynga_Core_Platform_AreNotificationsDisplayed() {
        try
        {
            SimulateExceptionWithUnitTests();
            // android support v4 24.0.0 is required
            return new DeviceInfoResult(Boolean.toString(NotificationManagerCompat.from(UnityPlayer.currentActivity).areNotificationsEnabled()), null);
        }
        catch (NoSuchMethodError e)
        {
            // for android support v4 23.x.x or less (method NotificationManagerCompat.areNotificationsEnabled() is absent)
            return new DeviceInfoResult(unknownValue, null);
        }
        catch(NoClassDefFoundError error) 
        {
            // If appcompat library is missing, can fail to resolve android/support/v4/app/NotificationManagerCompat
            return new DeviceInfoResult(unknownValue, null);
        }
        catch (Throwable e)
        {
            Log.e("AreNotificationsDisplayed", GetExceptionMessage(e));
            return new DeviceInfoResult(unknownValue, e);
        }
    }
    
    // Gets the carrier name.
    public static DeviceInfoResult Zynga_Core_Platform_CarrierName() {
        try
        {
            SimulateExceptionWithUnitTests();
            TelephonyManager telephonyManager = (TelephonyManager)UnityPlayer.currentActivity.getSystemService(Context.TELEPHONY_SERVICE);
            return new DeviceInfoResult(telephonyManager.getNetworkOperatorName(), null);
        }
        catch (Throwable e)
        {
            Log.e("CarrierName", GetExceptionMessage(e));
            return new DeviceInfoResult(unknownValue, e);
        }
    }
    
    // Gets the connection type.
    public static DeviceInfoResult Zynga_Core_Platform_ConnectionType() {
        String connectionType;
        try
        {
            SimulateExceptionWithUnitTests();
            ConnectivityManager connectivityManager = (ConnectivityManager)UnityPlayer.currentActivity.getSystemService(Context.CONNECTIVITY_SERVICE);
            NetworkInfo networkInfo = connectivityManager.getActiveNetworkInfo();
        
            if (networkInfo == null || !networkInfo.isConnected())
            {
                connectionType = "NotReachable";
            }
            else if (networkInfo.getType() == ConnectivityManager.TYPE_WIFI)
            {
                connectionType = "WiFi";
            }
            else if (networkInfo.getType() == ConnectivityManager.TYPE_MOBILE)
            {
                switch (networkInfo.getSubtype())
                {
                    case TelephonyManager.NETWORK_TYPE_GPRS:
                        connectionType = "GPRS";
                        break;
                    case TelephonyManager.NETWORK_TYPE_EDGE:
                        connectionType = "EDGE";
                        break;
                    case TelephonyManager.NETWORK_TYPE_CDMA: // cdmaOne (IS95A or IS95B), Android specific
                        connectionType = "CDMA";
                        break;
                    case TelephonyManager.NETWORK_TYPE_HSDPA:
                        connectionType = "HSDPA";
                        break;
                    case TelephonyManager.NETWORK_TYPE_HSPA: // HSPA, Android specific
                        connectionType = "HSPA";
                        break;
                    case TelephonyManager.NETWORK_TYPE_HSUPA:
                        connectionType = "HSUPA";
                        break;
                    case TelephonyManager.NETWORK_TYPE_HSPAP: // HSPA+, Android specific
                        connectionType = "HSPAP";
                        break;
                    case TelephonyManager.NETWORK_TYPE_UMTS:
                        connectionType = "UMTS";
                        break;
                    case TelephonyManager.NETWORK_TYPE_1xRTT:
                        connectionType = "CDMA1X";
                        break;
                    case TelephonyManager.NETWORK_TYPE_EVDO_0:
                        connectionType = "CDMAEVDOREV0";
                        break;
                    case TelephonyManager.NETWORK_TYPE_EVDO_A:
                        connectionType = "CDMAEVDOREVA";
                        break;
                    case TelephonyManager.NETWORK_TYPE_EVDO_B:
                        connectionType = "CDMAEVDOREVB";
                        break;
                    case TelephonyManager.NETWORK_TYPE_EHRPD:
                        connectionType = "EHRPD";
                        break;
                    case TelephonyManager.NETWORK_TYPE_IDEN: // iDEN, Android specific
                        connectionType = "IDEN";
                        break;
                    case TelephonyManager.NETWORK_TYPE_LTE:
                        connectionType = "LTE";
                        break;
                    case TelephonyManager.NETWORK_TYPE_UNKNOWN:
                    default:
                        connectionType = unknownValue;
                        break;
                }
            }
            else
            {
                connectionType = unknownValue;
            }
            return new DeviceInfoResult(connectionType, null);
        }
        catch (Throwable e)
        {
            Log.e("ConnectionType", GetExceptionMessage(e));
            return new DeviceInfoResult(unknownValue, e);
        }
    }
    
    // Check if ad tracking is enabled by the user
    public static DeviceInfoResult Zynga_Core_Platform_IsAdTrackingEnabled() {
        try
        {
            SimulateExceptionWithUnitTests();
            AdvertisingIdClient.Info advertisingInfo = AdvertisingIdClient.getAdvertisingIdInfo(UnityPlayer.currentActivity);
            return new DeviceInfoResult(advertisingInfo != null ? !advertisingInfo.isLimitAdTrackingEnabled() : false, null);
        }
        catch (Throwable e)
        {
            Log.e("IsAdTrackingEnabled", GetExceptionMessage(e));
            return new DeviceInfoResult(false, e);
        }
    }
    
    // Get the Google advertising of the device
    public static DeviceInfoResult Zynga_Core_Platform_GoogleAdvertisingId() {
        try
        {
            SimulateExceptionWithUnitTests();
            AdvertisingIdClient.Info advertisingInfo = AdvertisingIdClient.getAdvertisingIdInfo(UnityPlayer.currentActivity);
            return new DeviceInfoResult(advertisingInfo != null ? advertisingInfo.getId() : "", null);
        }
        catch (Throwable e)
        {
            Log.e("GoogleAdvertisingId", GetExceptionMessage(e));
            return new DeviceInfoResult("", e);
        }
    }

    // Get the AndroidId of the device
    public static DeviceInfoResult Zynga_Core_Platform_AndroidId() {
        try
        {
            SimulateExceptionWithUnitTests();
            return new DeviceInfoResult(Secure.getString(UnityPlayer.currentActivity.getContentResolver(), Secure.ANDROID_ID), null);
        }
        catch (Throwable e)
        {
            Log.e("AndroidId", GetExceptionMessage(e));
            return new DeviceInfoResult("", e);
        }
    }
    
    // Gets the current default locale, in "en-US" form.
    public static String Zynga_Core_Platform_LocaleCode() {
        return Locale.getDefault().toString();
    }

    // Gets current time zone
    public static String Zynga_Core_Platform_TimeZone()
    {
        return TimeZone.getDefault().getID();
    }
    
    // Determines if we are running on a tablet based on screen size
    public static DeviceInfoResult Zynga_Core_Platform_IsTablet() {
        try
        {
            SimulateExceptionWithUnitTests();
            DisplayMetrics displayMetrics = new DisplayMetrics();
            Display display = UnityPlayer.currentActivity.getWindowManager().getDefaultDisplay();
            display.getMetrics(displayMetrics);
            double x = Math.pow(displayMetrics.widthPixels / displayMetrics.xdpi, 2);
            double y = Math.pow(displayMetrics.heightPixels / displayMetrics.ydpi, 2);
            double screenInches = Math.sqrt(x + y);
            return new DeviceInfoResult(screenInches > TABLET_SCREEN_SIZE, null);
        }
        catch (Throwable e)
        {
            Log.e("IsTablet", GetExceptionMessage(e));
            return new DeviceInfoResult(false, e);
        }
    }
    
    // Get the operating system type and version
    public static String Zynga_Core_Platform_OperatingSystem() {
        return "Android OS " + Build.VERSION.RELEASE;
    }

    // Get the manufacturer of the device
    public static String Zynga_Core_Platform_Manufacturer() {
        return Build.MANUFACTURER;
    }

    // Get the model of the device
    public static String Zynga_Core_Platform_DeviceModel() {
        String manufacturer = Build.MANUFACTURER;
        String model = Build.MODEL;
        if (model.startsWith(manufacturer))
        {
            return model;
        }
        return manufacturer + " " + model;
    }

    // Get the device name
    public static DeviceInfoResult Zynga_Core_Platform_DeviceName() {
        String deviceName;
        try
        {
            SimulateExceptionWithUnitTests();
            deviceName = Settings.System.getString(UnityPlayer.currentActivity.getContentResolver(), "device_name");
            if (deviceName == null)
            {
                deviceName = Settings.Secure.getString(UnityPlayer.currentActivity.getContentResolver(), "bluetooth_name");
            }
            if (deviceName == null)
            {
                deviceName = unknownValue;
            }
            return new DeviceInfoResult(deviceName, null);
        }
        catch (Throwable e)
        {
            Log.e("DeviceName", GetExceptionMessage(e));
            return new DeviceInfoResult(unknownValue, e);
        }
    }
    
    // Get the memory size of the device
    public static DeviceInfoResult Zynga_Core_Platform_DeviceMemoryMB() {
        try
        {
            SimulateExceptionWithUnitTests();
            ActivityManager.MemoryInfo memoryInfo = new ActivityManager.MemoryInfo();
            ActivityManager activityManager = (ActivityManager)UnityPlayer.currentActivity.getSystemService(Context.ACTIVITY_SERVICE);
            activityManager.getMemoryInfo(memoryInfo);
            return new DeviceInfoResult(memoryInfo.totalMem / MEGABYTE, null);
        }
        catch (Throwable e)
        {
            Log.e("DeviceMemoryMB", GetExceptionMessage(e));
            return new DeviceInfoResult(0.0, e);
        }
    }
    
    public static DeviceInfoResult Zynga_Core_Platform_CurrentMemoryMB() {
        try
        {
            SimulateExceptionWithUnitTests();
            return new DeviceInfoResult(Debug.getNativeHeapAllocatedSize() / MEGABYTE, null);
        }
        catch (Throwable e)
        {
            Log.e("CurrentMemoryMB", GetExceptionMessage(e));
            return new DeviceInfoResult(0.0, e);
        }
    }

    public static DeviceInfoResult Zynga_Core_Platform_RemainingMemoryMB() {
        try
        {
            SimulateExceptionWithUnitTests();
            StatFs statFs = new StatFs(Environment.getRootDirectory().getAbsolutePath());   
            long   total  = ((long)statFs.getBlockCount() * (long)statFs.getBlockSize());
            return new DeviceInfoResult(total / MEGABYTE, null);
        }
        catch (Throwable e)
        {
            Log.e("RemainingMemoryMB", GetExceptionMessage(e));
            return new DeviceInfoResult(0.0, e);
        }
    }

    public static DeviceInfoResult Zynga_Core_Platform_ScreenHeight() {
        try
        {
            SimulateExceptionWithUnitTests();
            DisplayMetrics displayMetrics = new DisplayMetrics();
            Display display = UnityPlayer.currentActivity.getWindowManager().getDefaultDisplay();
            display.getMetrics(displayMetrics);

            int height = displayMetrics.heightPixels;
            return new DeviceInfoResult(height, null);
        }
        catch (Throwable e)
        {
            Log.e("ScreenHeight", GetExceptionMessage(e));
            return new DeviceInfoResult(0.0, e);
        }
    }

    public static DeviceInfoResult Zynga_Core_Platform_ScreenWidth() {
        try
        {
            SimulateExceptionWithUnitTests();
            DisplayMetrics displayMetrics = new DisplayMetrics();
            Display display = UnityPlayer.currentActivity.getWindowManager().getDefaultDisplay();
            display.getMetrics(displayMetrics);

            int width = displayMetrics.widthPixels;
            return new DeviceInfoResult(width, null);
        }
        catch (Throwable e)
        {
            Log.e("ScreenWidth", GetExceptionMessage(e));
            return new DeviceInfoResult(0.0, e);
        }
    }

    public static DeviceInfoResult Zynga_Core_Platform_ScreenDpi() {
        try
        {
            SimulateExceptionWithUnitTests();
            DisplayMetrics displayMetrics = new DisplayMetrics();
            Display display = UnityPlayer.currentActivity.getWindowManager().getDefaultDisplay();
            display.getMetrics(displayMetrics);  
            
            double dpi = displayMetrics.density;
            return new DeviceInfoResult(dpi, null);
        }
        catch (Throwable e)
        {
            Log.e("ScreenDpi", GetExceptionMessage(e));
            return new DeviceInfoResult(0.0, e);
        }
    }
}

