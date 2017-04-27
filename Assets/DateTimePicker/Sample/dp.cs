using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class dp : MonoBehaviour
{
    public ControlPage controlpage;
    public Text[] dateFromto;
    Text txtDate;
    private static DateTime selectedDate;

    private delegate void AndroidDateChange(DateTime time);

    private static AndroidDateChange OnAndroidDateChange;

    void Start()
    {
#if UNITY_IOS
        IOSDateTimePicker.instance.OnDateChanged += OnDateChanged;

#elif UNITY_ANDROID
        OnAndroidDateChange = OnDateChanged;
#endif
        selectedDate = DateTime.Now;

    }

    public void OpenDatePicker(Text value)
    {
        txtDate = value;
        if (value.name == "txtDatFrom")
        {
            controlpage.clickFromTo[0] = true;
        }
        if (value.name == "txtTo")
        {
            controlpage.clickFromTo[1] = true;
        }
        value.gameObject.SetActive(true);

        IFormatProvider culture = new System.Globalization.CultureInfo("fr-FR", true);
        from = DateTime.Parse(value.text, culture);
        selectedDate = from;
#if UNITY_IOS
        IOSDateTimePicker.instance.Show(IOSDateTimePickerMode.Date, DateTimeToUnixTimestamp(selectedDate));

#elif UNITY_ANDROID
        var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            new AndroidJavaObject("android.app.DatePickerDialog", activity, new DateCallback(), selectedDate.Year , selectedDate.Month , selectedDate.Day).Call("show");
        }));
#endif
    }


    public void OnpickerCloed(DateTime datetime)
    {

        Debug.Log("Onpicker close");


    }

    public double DateTimeToUnixTimestamp(DateTime dateTime)
    {
        return (TimeZoneInfo.ConvertTimeToUtc(dateTime) -
               new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;
    }
    private DateTime from, to;

    public void OnDateChanged(System.DateTime time)
    {
      //  if (txtDate.name == "txtDatFrom")
           // time = new DateTime(time.Year, time.Month, 1);
       // if (txtDate.name == "txtTo")
       //     time = new DateTime(time.Year, time.Month,DateTime.DaysInMonth(time.Year,time.Month));
        selectedDate = time;
        txtDate.text = time.ToString("dd-MMM-yyyy");
        SetDate();
    }

    public void SetDate()
    {

        controlpage.maindata.step2.datefrom = dateFromto[0].text;
        controlpage.maindata.step2.dateto = dateFromto[1].text;
        //validate ngay day cung duoc
    }

#if UNITY_ANDROID
    class DateCallback : AndroidJavaProxy
    {
        public DateCallback()
            : base("android.app.DatePickerDialog$OnDateSetListener")
        {
        }
        void onDateSet(AndroidJavaObject view, int year, int monthOfYear, int dayOfMonth)
        {
            selectedDate = new DateTime(year, monthOfYear, dayOfMonth);
            if (OnAndroidDateChange != null)
            {
                OnAndroidDateChange(selectedDate);
            }
        }
    }
#endif
}
