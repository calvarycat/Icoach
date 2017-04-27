using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using StarseedGeneral.LitJson;


public class INitData : MonoBehaviour
{
    public static INitData instance;
    string linkInitata = "http://vn1ln01.int.grs.net/api/user/init-data";
    string LinkGetActivity = "http://vn1ln01.int.grs.net/api/user/get-longterm-activity";

    public RootObjectInit initData;
    public bool isHaveData;


    public bool isLoadingdata = false;
    void Awake()
    {
        LoginTime= DateTime.Today;
        if (!instance)
            instance = this;
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this.gameObject);
        string dt = PlayerPrefs.GetString(KeySaving.DataInit.ToString(), "");
        if (!string.IsNullOrEmpty(dt))
        {
            isHaveData = true;
            initData = JsonMapper.ToObject<RootObjectInit>(dt);
            initData.result.evaluationActivity = initData.result.evaluationActivity.OrderBy(o => o.EvaluationActivityID).ToList();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            StartCoroutine(GetReport());
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            CheckOpenApp();
        }
    }

    public IEnumerator GetReport()
    {
        WWW httpResponse = new WWW("http://localhost/Makas/ItemsData.php");
        yield return httpResponse;
     //   Debug.Log(httpResponse.text);

    }


    public void SaveChangeData()
    {
        string jss = JsonMapper.ToJson(initData);
        PlayerPrefs.SetString(KeySaving.DataInit.ToString(), jss);

    }
    public void LoadingData(Action<String> OnSuccess, Action<String> OnFailed, Action<string> InternetFailed)
    {
        GetDataFake();
        StartCoroutine(InitFirstTimeData(OnSuccess, OnFailed, InternetFailed));
    }

    public List<LongTermObject> objFake;
    public List<EvaluationActivity> activeFake;
    public void GetDataFake()
    {
        objFake = new List<LongTermObject>();
        activeFake = new List<EvaluationActivity>();
        objFake = initData.result.longTermObject.FindAll(m => m.ObjectiveID > 9000000);
        activeFake = initData.result.evaluationActivity.FindAll(m => m.EvaluationActivityID > 9000000);
        //= 
    }

    public void SendDataToSever()
    {
        for (int i = 0; i < initData.result.rep.Count; i++)
        {
            PlayerPrefs.SetString(initData.result.rep[i].RepID.ToString() + "DualCallLate", "");
         
        }
    }

    IEnumerator SendReportList(List<SendReportLate> listReportLate)
    {
        string resultSendreport = "";
        string reportError = "";
        for (int i = 0; i < listReportLate.Count; i++)
        {
            if (listReportLate[i].Status == 0)
            {
                WWWForm form = new WWWForm();
                form.AddBinaryData("pdf1", listReportLate[i].img1, "ScreenShot1.png", "image/png");
                form.AddBinaryData("pdf2", listReportLate[i].img2, "ScreenShot2.png", "image/png");
                // convert here
                
                form.AddField("data", listReportLate[i].dcreport);
                WWW httpResponse = new WWW("http://vn1ln01.int.grs.net/api/user/save-report", form);
                yield return httpResponse;
                if (!string.IsNullOrEmpty(httpResponse.error))
                {
                    reportError += httpResponse.error;
                }
                else
                {
                    try
                    {
                        string t = httpResponse.text;
                        RootObjectSendReport result = JsonMapper.ToObject<RootObjectSendReport>(t);
                        if (result.error == 0)
                        {
                            listReportLate[i].Status = 1;
                        }
                        else
                        {
                            //Debug.Log("Failed to send report");
                        }

                    }
                    catch (Exception)
                    {

                        reportError += i + " Failed to send report. Please try again\n";
                   //     Debug.Log(reportError + httpResponse.error + httpResponse.text);
                    }
                }

            }
            yield return new WaitForSeconds(5);
        }
    }

   
    public DateTime LoginTime;
    public bool CheckOpenApp()
    {
        System.TimeSpan diff1 = DateTime.Today.Subtract(LoginTime);
        int day = diff1.Days;
        if (day > 0)
            return true;
        return false;
    }
    private IEnumerator InitFirstTimeData(Action<String> OnSuccess, Action<String> OnFailed, Action<string> InternetFailed)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
         //   Debug.Log("Error. Check internet connection!");
            InternetFailed("There is no internet connection!");
        }
        WWWForm form = new WWWForm();
        int amid = UserAuthentication.instance.aminfo.AM_ID;
        form.AddField("AM_ID", amid);
        WWW httpResponse = new WWW(linkInitata, form);
        yield return httpResponse;
        if (!string.IsNullOrEmpty(httpResponse.error))
        {
            OnFailed("There are some errors occurred while loading data");
        }
        else
        {
            try
            {
                initData = JsonMapper.ToObject<RootObjectInit>(httpResponse.text);
                LoadViewLastReport();
                initData.result.evaluationActivity = initData.result.evaluationActivity.OrderBy(o => o.EvaluationActivityID).ToList();

                initData.result.longTermObject.AddRange(objFake);
                initData.result.evaluationActivity.AddRange(activeFake);
                string t = JsonMapper.ToJson(initData);
                PlayerPrefs.SetString(KeySaving.DataInit.ToString(), t);
                OnSuccess("Loading data success");


            }
            catch (Exception)
            {

                OnFailed("There are some errors occurred while loading data");
            }


        }

    }

    public List<EvaluationActivity> test;
    public void LoadViewLastReport()
    {

        foreach (LastReport lrp in initData.result.lastReport)
        {
            string stlastReport = JsonMapper.ToJson(lrp);
            if (!string.IsNullOrEmpty(stlastReport))
            {
                PlayerPrefs.SetString(lrp.RepID.ToString(), stlastReport);// PlayerPrefs.SetString(repid;
            }

        }

    }


    public List<SendReportLate> listReportLate;
    public void LoadLateDualCall()
    {
        if (initData.result.lastReport != null)
        {
            foreach (LastReport lrp in initData.result.lastReport)
            {
                string stlastReport = JsonMapper.ToJson(lrp);
                if (!string.IsNullOrEmpty(stlastReport))
                {
                    PlayerPrefs.SetString(lrp.RepID.ToString(), stlastReport);// PlayerPrefs.SetString(repid;                    PlayerPrefs.SetString(lrp.RepID.ToString(), stlastReport);// PlayerPrefs.SetString(repid;
                }
            }
        }
    }



    public Rep GetRepByID(int repid)
    {
        return initData.result.rep.Find(m => m.RepID == repid);
    }
    public List<DaysNote> GetDateNotes(int repid)
    {
        return initData.result.daysNotes.FindAll(m => m.RepID == repid);
    }

    public LongTermObject GetLongtermObject(int repid)
    {
        return initData.result.longTermObject.Find(m => m.RepID == repid);
    }

    public List<EvaluationActivity> GetEvaluationActivity(int repid)
    {
        return initData.result.evaluationActivity.FindAll(m => m.RepID == repid);
    }

    void FailedGetActive(string sms)
    {
        Debug.Log(sms);
    }


    public GoDualCall GetDualCall(int repID)
    {
        GoDualCall g = new GoDualCall();
        g.longtermObject = GetLongtermObject(repID);
        g.rep = GetRepByID(repID);
        g.daysNote = GetDateNotes(repID);
        g.dualCalllists = GetListDualCalltime(GetEvaluationActivity(repID));
        return g;
    }



    public List<OneTimeDualCall> GetListDualCalltime(List<EvaluationActivity> listduacal)
    {
        int d = 0;
        List<OneTimeDualCall> lt = new List<OneTimeDualCall>();
        OneTimeDualCall oneTimedc = new OneTimeDualCall();
        int num = listduacal.Count / 14;
        // oneTimedc.EvaluationActivity.AddRange(listduacal.GetRange(0,14));
        int u = 0;

        for (int j = 0; j < num; j++)
        {

            oneTimedc = new OneTimeDualCall();
            oneTimedc.EvaluationActivity = new List<EvaluationActivity>();
            oneTimedc.EvaluationActivity.AddRange(listduacal.GetRange(u, 14));
            lt.Add(oneTimedc);
            u = u + 14;

        }



        return lt;
    }
}
[System.Serializable]
public class GoDualCall
{
    public AMInfor infor = new AMInfor();
    public Rep rep = new Rep();
    public List<DaysNote> daysNote;
    public LongTermObject longtermObject = new LongTermObject();
    // public List<EvaluationActivity> evaluationActivity;
    public List<OneTimeDualCall> dualCalllists;

}
[System.Serializable]
public class OneTimeDualCall
{
    public List<EvaluationActivity> EvaluationActivity = new List<EvaluationActivity>();// có 14 cái
}
[System.Serializable]
public class Rep
{
    public int AM_ID;
    public string AM_Name;
    public int RegionID;
    public string RegionName;
    public int SectorID;
    public string SectorName;
    public int TerritoryID;
    public string TerritoryName;
    public int RepID;
    public string RepName;
    public int TeamTypeID;
    public string TeamTypeName;
    public int TeamID;
    public string TeamName;
}
[System.Serializable]
public class DaysNote
{
    public string product;
    public string doctorsPharmacicists;
    public string date;
    public string comment;
    public int RepID;
}
[System.Serializable]
public class LongTermObject
{
    public int ObjectiveID;
    public int AM_ID;
    public int RepID;
    public string computed;
    public string FromDate;
    public string ToDate;
    public string CreateDate;
    public string UpdateDate;
    public int StatusID;
    public int FakeObjectiveID ;
    public int SentStatus ;
}
[System.Serializable]
public class EvaluationActivity
{
    public string Name;
    public int Score;
    public string EditDate;
    public int RepID;
    public int TargetA;
    public int TargetB;
    public int TargetO;
    public int ObjectiveID;
    public int EvaluationActivityID = 0;
    public string comment;
    public int FakeEvaluationActivityID;
}
[System.Serializable]
public class ResultInit
{
    public List<Rep> rep;
    public List<DaysNote> daysNotes;
    public List<LongTermObject> longTermObject;
    public List<EvaluationActivity> evaluationActivity;
    public List<LastReport> lastReport;
}
[System.Serializable]
public class LastReport
{
    public string objective { get; set; }
    public string comment { get; set; }
    public int RepID { get; set; }
    public string fromDate { get; set; }
    public string toDate { get; set; }
}
[System.Serializable]
public class RootObjectInit
{
    public int success;
    public int error;
    public string msg;
    public ResultInit result;
}

public class ListResultActivity
{
    public List<EvaluationActivity> EvaluationActivity { get; set; }
}

public class RootObjectGetActivityByID
{
    public int success { get; set; }
    public int error { get; set; }
    public string msg { get; set; }
    public ListResultActivity result { get; set; }
}