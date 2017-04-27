using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using StarseedGeneral.LitJson;
using UnityEngine.UI;

public class SysDataInApp : MonoBehaviour
{
    public ControlLogin contrologin;
    void Start()
    {
        SyncDateOfNotes();
       // SyncRep();
    }

    public void SyncRep()
    {

        StartCoroutine(GetRep(sssSS, sssSS, sssSS));

    }

    public void SyncDateOfNotes()
    {
        StartCoroutine(Load14DateOfNote(sssSS, sssSS, sssSS));


    }
    public void sssSS(string sss)
    {
        Debug.Log("Notes" + sss);
    }

    public void LoadingData(Action<String> OnSuccess, Action<String> OnFailed, Action<string> InternetFailed)
    {
        StartCoroutine(Load14DateOfNote(OnSuccess, OnFailed, InternetFailed));
    }
    private IEnumerator Load14DateOfNote(Action<String> OnSuccess, Action<String> OnFailed, Action<string> InternetFailed)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Error. Check internet connection!");
            InternetFailed("There is no internet connection!");
        }
        WWWForm form = new WWWForm();
        int amid = UserAuthentication.instance.aminfo.AM_ID;
        form.AddField("AM_ID", amid);
        WWW httpResponse = new WWW("http://vn1ln01.int.grs.net/api/user/get-note", form);
        yield return httpResponse;

        Debug.Log(httpResponse.text);
        if (!string.IsNullOrEmpty(httpResponse.error))
        {
            //faild(httpResponse.error);
            OnFailed("httpResponse.error");
        }
        else
        {
            try
            {
                RootObjectDateNotes pb = new RootObjectDateNotes();
                pb = JsonMapper.ToObject<RootObjectDateNotes>(httpResponse.text);
                if (pb.result.Count > 0)
                {
                    INitData.instance.initData.result.daysNotes.Clear();
                    INitData.instance.initData.result.daysNotes.AddRange(pb.result);

                    string jss = JsonMapper.ToJson(INitData.instance.initData); // save new data
                    PlayerPrefs.SetString(KeySaving.DataInit.ToString(), jss); // save new dâta
                    OnSuccess("Syn date of note success");
                }
                else
                {
                    OnFailed("Syn date of note failed");
                }
            }
            catch (Exception)
            {

                OnFailed("Syn date of note failed");
            }
          



        }

    }

    private IEnumerator GetRep(Action<String> OnSuccess, Action<String> OnFailed, Action<string> InternetFailed)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Error. Check internet connection!");
            InternetFailed("There is no internet connection!");
        }
        WWWForm form = new WWWForm();
        int amid = UserAuthentication.instance.aminfo.AM_ID;
        form.AddField("AM_ID", amid);
        WWW httpResponse = new WWW("http://vn1ln01.int.grs.net/api/user/get-rep", form);
        yield return httpResponse;

        Debug.Log(httpResponse.text);
        if (!string.IsNullOrEmpty(httpResponse.error))
        {
            //faild(httpResponse.error);
            OnFailed("httpResponse.error");
        }
        else
        {
            RootObjectGetRep pb = new RootObjectGetRep();
            pb = JsonMapper.ToObject<RootObjectGetRep>(httpResponse.text);
            if (pb.result.Count > 0)
            {
                try
                {
                    INitData.instance.initData.result.rep.Clear();
                    INitData.instance.initData.result.rep.AddRange(pb.result);

                    string jss = JsonMapper.ToJson(INitData.instance.initData); // save new data
                    PlayerPrefs.SetString(KeySaving.DataInit.ToString(), jss); // save new dâta
                    OnSuccess("Syn date of note success");
                }
                catch (Exception)
                {

                    OnFailed("Syn date of note failed");
                }
               
            }
            else
            {
                OnFailed("Syn date of note failed");
            }



        }

    }


}

public class RootObjectDateNotes
{
    public int success { get; set; }
    public int error { get; set; }
    public string msg { get; set; }
    public List<DaysNote> result { get; set; }
}

public class RootObjectGetRep
{
    public int success { get; set; }
    public int error { get; set; }
    public string msg { get; set; }
    public List<Rep> result { get; set; }
}