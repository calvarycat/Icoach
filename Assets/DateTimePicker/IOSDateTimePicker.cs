using UnityEngine;
using System;
using System.Collections;


#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
using System.Runtime.InteropServices;
#endif




public class IOSDateTimePicker : ISN_Singleton<IOSDateTimePicker>
{

#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
	
	[DllImport ("__Internal")]
	private static extern void _ISN_ShowDP(int mode, double unix);
		
#endif

    public Action<DateTime> OnDateChanged = delegate { };
    public Action<DateTime> OnPickerClosed = delegate { };




    //--------------------------------------
    // Public Methods
    //--------------------------------------


    public void Show(IOSDateTimePickerMode mode, double unix = 0)
    {
#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
  _ISN_ShowDP( (int) mode, unix);
#endif
    }



    //--------------------------------------
    // Events
    //--------------------------------------

    private void DateChangedEvent(string time)
    {
        DateTime dt = DateTime.Parse(time);

        if (OnDateChanged != null)
            OnDateChanged(dt);
    }

    private void PickerClosed(string time)
    {
        DateTime dt = DateTime.Parse(time);

        if (OnPickerClosed != null)
            OnPickerClosed(dt);
    }
}
