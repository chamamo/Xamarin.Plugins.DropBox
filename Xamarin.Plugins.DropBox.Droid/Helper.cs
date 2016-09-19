using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Lang.Reflect;
using Java.Util;
using Object = System.Object;

namespace Xamarin.Plugins.DropBox
{
    public static class Helper
    {
        public static Activity GetCurrentActivity()
        {
            Class activityThreadClass = Class.ForName("android.app.ActivityThread");
            var activityThread = activityThreadClass.GetMethod("currentActivityThread").Invoke(null);
            Field activitiesField = activityThreadClass.GetDeclaredField("mActivities");
            activitiesField.Accessible = true;
            var o = activitiesField.Get(activityThread);
            var activities = (IMap)o;
            foreach (Java.Lang.Object activityRecord in activities.Values())
            {
                Class activityRecordClass = activityRecord.Class;
                Field pausedField = activityRecordClass.GetDeclaredField("paused");
                pausedField.Accessible = true;
                if (!pausedField.GetBoolean(activityRecord))
                {
                    Field activityField = activityRecordClass.GetDeclaredField("activity");
                    activityField.Accessible = true;
                    Activity activity = (Activity)activityField.Get(activityRecord);
                    return activity;
                }
            }
            return null;
        }
    }
}