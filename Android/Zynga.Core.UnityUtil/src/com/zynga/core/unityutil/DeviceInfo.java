package com.zynga.core.unityutil;

import java.io.IOException;
import android.content.Context;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.telephony.TelephonyManager;

import com.unity3d.player.UnityPlayer;
import com.google.android.gms.ads.identifier.AdvertisingIdClient;
import com.google.android.gms.ads.identifier.AdvertisingIdClient.Info;
import com.google.android.gms.common.GooglePlayServicesNotAvailableException;
import com.google.android.gms.common.GooglePlayServicesRepairableException;

public class DeviceInfo {
    private Context context;

    public DeviceInfo() {
        this.context = UnityPlayer.currentActivity;
    }

    public String getGoogleAdvertisingId() {
        String advertisingId = null;
        try {
            Info adInfo = AdvertisingIdClient.getAdvertisingIdInfo(context);
            advertisingId = adInfo.getId();
            return advertisingId;
        } catch (IOException e) {
            return "Exception: " + e;
        } catch (GooglePlayServicesNotAvailableException e) {
            return "Exception: " + e;
        } catch (IllegalStateException e) {
            return "Exception: " + e;
        } catch (GooglePlayServicesRepairableException e) {
            return "Exception: " + e;
        } catch (Exception e) {
            return "Exception: " + e;
        }
    }

    public String getConnectionType() {

        try {
            String NotReachable = "NotReachable";

            ConnectivityManager connectivityManager = (ConnectivityManager) context
                    .getSystemService(context.CONNECTIVITY_SERVICE);
            if (connectivityManager == null) {
                return NotReachable;
            }
            NetworkInfo networkInfo = connectivityManager
                    .getActiveNetworkInfo();
            if (networkInfo == null) {
                return NotReachable;
            }
            boolean isConnected = networkInfo.isConnected();
            if (!isConnected)
                return NotReachable;

            int type = networkInfo.getType();
            if (type == ConnectivityManager.TYPE_WIFI) {
                return "Wifi";
            }
            if (type == ConnectivityManager.TYPE_MOBILE) {
                int subType = networkInfo.getSubtype();
                switch (subType) {
                case TelephonyManager.NETWORK_TYPE_1xRTT:
                    return "1xRTT";
                case TelephonyManager.NETWORK_TYPE_CDMA:
                    return "CDMA";
                case TelephonyManager.NETWORK_TYPE_EDGE:
                    return "EDGE";
                case TelephonyManager.NETWORK_TYPE_EHRPD:
                    return "EHRPD";
                case TelephonyManager.NETWORK_TYPE_EVDO_0:
                    return "CDMAEVDOREV0";
                case TelephonyManager.NETWORK_TYPE_EVDO_A:
                    return "CDMAEVDOVA";
                case TelephonyManager.NETWORK_TYPE_EVDO_B:
                    return "CDMAEVDOVB";
                case TelephonyManager.NETWORK_TYPE_GPRS:
                    return "GPRS";
                case TelephonyManager.NETWORK_TYPE_HSDPA:
                    return "HSDPA";
                case TelephonyManager.NETWORK_TYPE_HSPA:
                    return "HSPA";
                case TelephonyManager.NETWORK_TYPE_HSPAP:
                    return "HSPAP";
                case TelephonyManager.NETWORK_TYPE_HSUPA:
                    return "HSUPA";
                case TelephonyManager.NETWORK_TYPE_IDEN:
                    return "IDEN";
                case TelephonyManager.NETWORK_TYPE_LTE:
                    return "LTE";
                case TelephonyManager.NETWORK_TYPE_UMTS:
                    return "UMTS";
                case TelephonyManager.NETWORK_TYPE_UNKNOWN:
                default:
                    return "Unknown-subType=" + subType;

                }
            } else { // Neither Wifi nor Mobile
                return "Unknown-Type=" + type;
            }
        } catch (Exception e) {
            return "Exception: " + e;
        }

    }
}