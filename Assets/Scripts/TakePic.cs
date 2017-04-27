using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using StarseedGeneral.LitJson;


public class TakePic : MonoBehaviour
{

    public ControlPage controlpage;
    public GameObject camReportPosition;
    public GameObject PopupWaiting;

    public void OnEnable()
    {
        listLate = new ClassListSendreportLate();
        string sendLate = PlayerPrefs.GetString("DualCallLate", "");
        if (!string.IsNullOrEmpty(sendLate))
        {
            listLate = JsonMapper.ToObject<ClassListSendreportLate>(sendLate);
        }
    }

    public string ScreenShotName(string name)
    {
        return name + System.DateTime.Today.ToString("dd-MMM-yyyy") + ".png";
    }


    public bool CheckLongtermIdInListsend(int longtermID)
    {
        if (listLate.listReport.FindAll(m => m.FakeACtivitiesID == longtermID).Count > 0)
            return true;
        return false;
    }

    public void CreateSendReportAndSaveLocal()
    {
        controlpage.sendreport = ConvertToRePort(controlpage.maindata, DateTime.Today.ToString(), controlpage.godc.rep);

        string reportbyrep = JsonMapper.ToJson(controlpage.maindata);
        PlayerPrefs.SetString(KeySaving.DataRepNotSent.ToString() + controlpage.godc.rep.RepID.ToString(), reportbyrep);
        controlpage.ViewDataOnly(false);
        controlpage.isNotSend = true;
        controlpage.btnOverview[0].interactable = true;
        Color temp = controlpage.btnOverview[0].image.color;
        temp.a = .3f;
        controlpage.btnOverview[0].image.color = temp;
    }

    public void CreateListSendReport(int[] idsend)
    {
        RenderTexture currentActiveRT = RenderTexture.active;
        RenderTexture.active = screenShot1;
        tx2d[0].ReadPixels(new Rect(0, 0, 1025, 2040), 0, 0);
        var pdf1 = tx2d[0].EncodeToPNG();
        tx2d[1].ReadPixels(new Rect(0, 2029, 1024, 2040), 0, 0); // test crash
        var pdf2 = tx2d[1].EncodeToPNG();
        string sendLate = PlayerPrefs.GetString("DualCallLate", "");
        if (!string.IsNullOrEmpty(sendLate))
        {
            listLate = JsonMapper.ToObject<ClassListSendreportLate>(sendLate);
        }
        //check xem trong này có rep nay ko?


        //check xem rep này đã nhập ngày hôm nay chưa? neu nhap roi ko cho thay doi

        //if (CheckReportInlistLate(controlpage.godc.rep.RepID, DateTime.Today.ToString()))
        //{
        //    //delete neu no ton tai roi ma chua duoc gui di
        //  //  listLate.listReport.RemoveAll(m => m.RepID == l.RepID && m.dateTime == DateTime.Today.ToString());

        //    //chưa remove ben trong
        //}


        l = new SendReportLate();
        l.fakeObjectId = idsend[0];
        l.FakeACtivitiesID = idsend[1];
        if (controlpage.godc.longtermObject != null)
            l.ObjectID = controlpage.godc.longtermObject.ObjectiveID;
        else
        {
            l.ObjectID = 0;
        }
        l.ActivitiesID = idsend[1];
        l.dcreport = PublicClassDualCallToJson(controlpage.maindata, DateTime.Today.ToString(), controlpage.godc.rep);
        l.dualCallReportObject = controlpage.sendreport; //ConvertToRePort(controlpage.maindata);
        l.dcthisDay = controlpage.maindata;
        l.img1 = pdf1;
        l.img2 = pdf2;
        l.dateTime = DateTime.Today.ToString();
        l.Status = 0;
        l.RepID = controlpage.godc.rep.RepID;
        if (CheckReportInlistLate(l.RepID, DateTime.Today.ToString()))
        {
            //delete neu no ton tai roi ma chua duoc gui di
            listLate.listReport.RemoveAll(m => m.RepID == l.RepID && m.dateTime == DateTime.Today.ToString());

            //chưa remove ben trong
        }
        listLate.listReport.Add(l);
        RenderTexture.active = currentActiveRT;
        StartCoroutine(SendReportOffline(listLate.listReport, SendSuccess, SendFailed));

    }


    public bool CheckSendReportToday()
    {
        if (CheckReportInlistLate(controlpage.godc.rep.RepID, DateTime.Today.ToString()))
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    public void CreateListSendReport1(int[] idsend)
    {
      
            RenderTexture currentActiveRT = RenderTexture.active;
            RenderTexture.active = screenShot1;
            tx2d[0].ReadPixels(new Rect(0, 0, 1025, 2040), 0, 0);
            var pdf1 = tx2d[0].EncodeToPNG();
            tx2d[1].ReadPixels(new Rect(0, 2029, 1024, 2040), 0, 0); 
            var pdf2 = tx2d[1].EncodeToPNG();
            string sendLate = PlayerPrefs.GetString("DualCallLate", "");
            if (!string.IsNullOrEmpty(sendLate))
            {
                listLate = JsonMapper.ToObject<ClassListSendreportLate>(sendLate);
            }
            //check xem trong này có rep nay ko?
            l = new SendReportLate();
            l.fakeObjectId = idsend[0];
            l.FakeACtivitiesID = idsend[1];
            if (controlpage.godc.longtermObject != null)
                l.ObjectID = controlpage.godc.longtermObject.ObjectiveID;
            else
            {
                l.ObjectID = 0;
            }
            l.ActivitiesID = idsend[1];
            l.dcreport = PublicClassDualCallToJson(controlpage.maindata, DateTime.Today.ToString(), controlpage.godc.rep);
            l.dualCallReportObject = controlpage.sendreport; //ConvertToRePort(controlpage.maindata);
            l.dcthisDay = controlpage.maindata;
            l.img1 = pdf1;
            l.img2 = pdf2;
            l.dateTime = DateTime.Today.ToString();
            l.Status = 0;
            l.RepID = controlpage.godc.rep.RepID;
            listLate.listReport.Add(l);
            RenderTexture.active = currentActiveRT;
       
      //  StartCoroutine(SendReportOffline(listLate.listReport, SendSuccess, SendFailed));

    }

    public void StartSendReport()
    {
        StartCoroutine(SendReportOffline(listLate.listReport, SendSuccess, SendFailed));
    }



    public bool CheckReportInlistLate(int repID, string dateSend)
    {
        if (listLate.listReport.FindAll(m => m.RepID == repID && m.dateTime == dateSend).Count > 0)
            return true;
        return false;
    }

    bool checkRepGoToday()
    {
        return true;
    }

    int numSuccess = 0;
    private int numfailed;
    private int totalsend;
    private StringBuilder resultsSend;
    IEnumerator SendReportOffline(List<SendReportLate> listReportLate, Action<string, int, int, SendReportLate> success,
        Action<string> faild)
    {
        resultsSend = new StringBuilder();
        rpAftersend = new ReportAfterSend();
        reportAnalytics = new ReportAnalytics();
        isCatchException = false;
        numSuccess = 0;
        numfailed = 0;
        string resultSendreport = "";
        string reportError = "";
        totalsend = listLate.listReport.FindAll(a => a.Status == 0).Count;
        for (int i = 0; i < listLate.listReport.Count; i++)
        {
            if (listLate.listReport[i].Status == 0)
            {
                WWWForm form = new WWWForm();
                form.AddBinaryData("pdf1", listLate.listReport[i].img1, "ScreenShot1.png", "image/png");
                form.AddBinaryData("pdf2", listLate.listReport[i].img2, "ScreenShot2.png", "image/png");
                string datarp = PublicClassDualCallToJson(listLate.listReport[i].dcthisDay,
                    listLate.listReport[i].dateTime, listLate.listReport[i].dualCallReportObject.rep);

                form.AddField("data", datarp);
                WWW httpResponse = new WWW("http://vn1ln01.int.grs.net/api/user/save-report", form);
                // WWW httpResponse = new WWW("http://localhost/Makas/ItemsData.php", form);
                float timer = 0;
                bool failed = false;
                while (!httpResponse.isDone)
                {
                    if (timer > 120) // sua thanh 120
                    {
                        failed = true;
                        if (failed)
                        {
                            resultsSend.AppendLine("Waiting too long, the data will be save in local and send in the next time");
                            faild("Waiting too long, the data will be save in local and send in the next time");
                            httpResponse.Dispose();
                        }
                        break;
                    }
                    timer += Time.deltaTime;
                    yield return null;
                }
                yield return httpResponse;
                if (!string.IsNullOrEmpty(httpResponse.error))
                {
                    reportError += httpResponse.error;
                    resultsSend.AppendLine(httpResponse.error);
                    faild("Waiting too long, the data will be save in local and send in the next time");
                    resultsSend.AppendLine(httpResponse.error);
                }
                else
                {
                    try
                    {
                        RootObjectSendReport result = JsonMapper.ToObject<RootObjectSendReport>(httpResponse.text);
                        Debug.Log(httpResponse.text);
                        if (result.error == 0)
                        {
                            UpdateSuccessSend(i, result);
                            resultsSend.AppendLine(result.msg);
                            success("Success", result.result.ObjectiveID, result.result.EvaluationActivityID,
                                listLate.listReport[i]);

                        }
                        else
                        {
                            resultsSend.AppendLine(result.msg);
                            if (result.error_code == 1)
                            {
                               
                                UpdateToomany(i,result.result_error[0].ObjectiveID,
                                    result.result_error[0].EvaluationActivityID);
                            }
                            if (result.error_code == 2)
                            {
                                UpdateTooBig(i, result.result_error[0].ObjectiveID,
                                    result.result_error[0].EvaluationActivityID);
                            
                            }
                            if (result.error_code == 3)
                            {
                                listLate.listReport[i].Status = 1;
                            }
                            if (result.error_code == 4)
                            {
                                listLate.listReport[i].Status = 1;
                            }
                            if (result.error_code == 5)
                            {
                                listLate.listReport[i].Status = 1;
                            }
                            //const API_CODE_MANY_ACTIVITY = 1;   //too mayny activity
                            //const API_CODE_WRONG_LONG_TERM_OBJECT = 2; //longterm object id too big
                            //const API_CODE_OBJECT_ARCHIVED = 3;    //đã achieve rồi mà đi lại
                            //const API_CODE_MORE_LONG_TERM_OBJECT = 4;// chưa xong object cũ mà đã tạo mới
                            //const API_CODE_MISSING_LONG_TERM_OBJECT = 5; // không có long term object
                            //const API_CODE_NO_ERROR = 6;

                            if (!string.IsNullOrEmpty(result.msg))
                            {
                                faild(result.msg);
                            }
                            else
                            {
                                faild("Failed to send report. Please try again");

                            }
                        }

                    }
                    catch (Exception)
                    {
                        resultsSend.AppendLine(" Can't parse data from server "+ httpResponse.error);
                        faild("Failed to send report. Can't parse data from server");
                        reportError += "Failed to send report. Please try again \n" + "vitri late" + i.ToString();
                       // Debug.Log(reportError + httpResponse.error + httpResponse.text);
                        isCatchException = true;
                        break;
                    }
                }

            }
            yield return new WaitForSeconds(1);
        }

    }

    private void UpdateTooBig(int i, int ObjectId, int EvaluationActivityID)
    {
        LongTermObject lt = INitData.instance.initData.result.longTermObject.Find(
            m => m.ObjectiveID == listLate.listReport[i].fakeObjectId);
        if (lt != null)
            lt.ObjectiveID = ObjectId;

        List<EvaluationActivity> listEva = INitData.instance.initData.result.evaluationActivity
            .FindAll(m => m.EvaluationActivityID == listLate.listReport[i].FakeACtivitiesID);

        foreach (var lvs in listEva)
        {
            lvs.EvaluationActivityID = EvaluationActivityID;
        }

        UpdateListLate(ObjectId, listLate.listReport[i].fakeObjectId);
     
    }

    private void UpdateToomany(int i,  int ObjectId, int EvaluationActivityID)
    {
        LongTermObject lt = INitData.instance.initData.result.longTermObject.Find(
            m => m.ObjectiveID == listLate.listReport[i].fakeObjectId);
        if (lt != null)
            lt.ObjectiveID = ObjectId;

        List<EvaluationActivity> listEva = INitData.instance.initData.result.evaluationActivity
            .FindAll(m => m.EvaluationActivityID == listLate.listReport[i].FakeACtivitiesID);

        foreach (var lvs in listEva)
        {
            lvs.EvaluationActivityID = EvaluationActivityID;
        }

        UpdateListLate(ObjectId, listLate.listReport[i].fakeObjectId);
        listLate.listReport[i].Status = 1;
    }

 

    private void UpdateSuccessSend(int i, RootObjectSendReport result)
    {
        LongTermObject lt = INitData.instance.initData.result.longTermObject.Find(
            m => m.ObjectiveID == listLate.listReport[i].fakeObjectId);
        if (lt != null)
            lt.ObjectiveID = result.result.ObjectiveID;

        List<EvaluationActivity> listEva = INitData.instance.initData.result.evaluationActivity
            .FindAll(m => m.EvaluationActivityID == listLate.listReport[i].FakeACtivitiesID);

        foreach (var lvs in listEva)
        {
            lvs.EvaluationActivityID = result.result.EvaluationActivityID;
        }

        UpdateListLate(result.result.ObjectiveID, listLate.listReport[i].fakeObjectId);
        if (listLate.listReport[i].dateTime == DateTime.Today.ToString())
        {
            controlpage.SetLastReport(listLate.listReport[i].RepID,
                listLate.listReport[i].dualCallReportObject.step2.datefrom,
                listLate.listReport[i].dualCallReportObject.step2.dateto,
                listLate.listReport[i].dualCallReportObject.step2.longterm,
                listLate.listReport[i].dcthisDay.step2.howto);
            controlpage.SaveDataTodayReport(listLate.listReport[i].RepID,
                listLate.listReport[i].dcthisDay);
        }

        listLate.listReport[i].Status = 1;
    }

    private bool isCatchException;

    public void UpdateListLate(int objectiviID, int fakeID)
    {

        List<SendReportLate> lt = listLate.listReport.FindAll(m => m.ObjectID == fakeID);
        foreach (SendReportLate listt in lt)
        {
            listt.ObjectID = objectiviID;
            listt.dualCallReportObject.ObjectiveID = objectiviID;
            listt.dcthisDay.longterm.ObjectiveID = objectiviID;
        }

    }

    private bool isChangeSendNext = false;

    void SendSuccess(string ss, int objectiveID, int ActivityID, SendReportLate reportLate)
    {
        ReportAnalytics ra = new ReportAnalytics();
        ra.ObjectiveID = objectiveID;
        ra.ActivityID = ActivityID;
        ra.sms = "send success";
        ra.dateTime = reportLate.dateTime;
        // Debug.Log(ss + "" + objectiveID + "//acti" + ActivityID);
        numSuccess++;
        //  Debug.Log("failed " + numfailed + "/ success" + numSuccess + " / total" + totalsend);
        if (reportLate.dateTime == DateTime.Today.ToString())
        {
            controlpage.SaveDataTodayReport(reportLate.RepID, reportLate.dcthisDay);
        }
        if (numSuccess + numfailed == totalsend)
        {
            PopupWaiting.SetActive(false);
            string mes = "";
            if (numSuccess > 0)
            {
                mes = "Send succeed " + numSuccess.ToString() + " report. ";
            }
            if (numfailed > 0)
            {
                mes = "Failed to send" + numfailed.ToString() + " report. that report will be sent in the next time. ";
            }

            // controlpage.ShowMesage(mes);
            Debug.Log(resultsSend.ToString() + "ss");
            controlpage.ShowMesage(resultsSend.ToString());
            PopupWaiting.SetActive(false);

            SaveListLate();
            controlpage.SaveAfterReport();

        }
    }

    private ReportAfterSend rpAftersend = new ReportAfterSend();
    ReportAnalytics reportAnalytics = new ReportAnalytics();
    void DisPlayTotalReport(int numSuccess, int numFaild)
    {
        rpAftersend.numSussess = numSuccess;
        rpAftersend.numFaild = numFaild;
    }

    void SaveListLate()
    {
        listLate.listReport.RemoveAll(a => a.Status == 1);
        string saveToDie = JsonMapper.ToJson(listLate);
        PlayerPrefs.SetString("DualCallLate", saveToDie);
    }


    void SendFailed(string ss)
    {
        if (!isCatchException)
        {

            numfailed++;
            //   Debug.Log("failed " + numfailed + "/ success" + numSuccess + " / total" + totalsend);
            if (numSuccess + numfailed == totalsend)
            {
                string mes = "";
                if (numSuccess > 0)
                {
                    mes = "send succeed " + numSuccess.ToString() + ". ";
                }
                if (numfailed > 0)
                {
                    mes += "Failed  to send " + numfailed.ToString() + " report. that report will be sent in the next time. ";
                }
           
                controlpage.ShowMesage(resultsSend.ToString());
                PopupWaiting.SetActive(false);
                SaveListLate();
            }

        }
        else
        {
         //   Debug.Log(resultsSend.ToString() + "ex");
            controlpage.ShowMesage(resultsSend.ToString());
            //  controlpage.ShowMesage("Can't send report");
            PopupWaiting.SetActive(false);
            SaveListLate();
        }

    }


    public RenderTexture screenShot1;
    public Texture2D[] tx2d;
    private string json_bill;
    private string send;

    public string PublicClassDualCallToJson(PublicClassDualCall dc, string dateCreate, Rep rp)
    {
        json_bill = "";
        controlpage.sendreport = ConvertToRePort(dc, dateCreate, rp);
        json_bill = JsonMapper.ToJson(controlpage.sendreport);
        return json_bill;
    }


    public DualCallReport ConvertToRePort(PublicClassDualCall dc, string dateCreate, Rep rp)
    {
        DualCallReport sendreport = new DualCallReport();


        if (dateCreate == null)
        {
            dateCreate = DateTime.Today.ToString();
        }
        controlpage.maindata.dateSendReport = dateCreate;
        sendreport.clientDate = dateCreate;
        sendreport.ObjectiveID = dc.longterm.ObjectiveID;
        sendreport.AM_ID = UserAuthentication.instance.aminfo.AM_ID;
        sendreport.rep = rp;
        sendreport.step1.adoctor = dc.step1.adoctor;
        sendreport.step1.bkadoctor = dc.step1.bkadoctor;
        sendreport.step1.odoctor = dc.step1.odoctor;
        sendreport.step1.period = dc.step1.period;
        sendreport.step2.Achieve = dc.step2.Achieve;
        sendreport.step2.datefrom = dc.step2.datefrom;
        sendreport.step2.dateto = dc.step2.dateto;
        sendreport.step2.longterm = ReplateText(dc.step2.longterm);
        sendreport.step2.howto = ReplateText(dc.step2.howto);
        sendreport.step3.objective = (int)dc.step3.dataChar[0];
        sendreport.step3.CatchyHook = (int)dc.step3.dataChar[1];
        sendreport.step3.KeyMessage = (int)dc.step3.dataChar[2];
        sendreport.step3.EvisualAd = (int)dc.step3.dataChar[3];
        sendreport.step3.DocumentUsed = (int)dc.step3.dataChar[4];
        sendreport.step3.BrandReminder = (int)dc.step3.dataChar[5];
        sendreport.step3.Interaction = (int)dc.step3.dataChar[6];
        sendreport.step3.Comitment = (int)dc.step3.dataChar[7];
        sendreport.step3.QualityOfNote = (int)dc.step3.dataChar[8];
        sendreport.step4.QualityNoteEvaluate = ReplateText(dc.step4.evaluaateQualityofNotes);
        sendreport.step5.Evaluatetheoptimization = ReplateText(dc.step5.evaluaateQualityofVisit);
        sendreport.step6.Evaluateknowledgeandskills = ReplateText(dc.step6.evaluateProductExpertise);
        sendreport.step7.Agreementbetween = ReplateText(dc.step7.evaluateCommentAgreement);

        return sendreport;
    }

    string ReplateText(string txt)
    {

        string lt = txt.Replace("'", " ");
        lt = txt.Replace("\"", " ");
        lt = lt.Replace("\\", "  ");
        return lt;

    }

    public ClassListSendreportLate listLate;
    public bool CheckAddlistReport(List<SendReportLate> listsrp, string dateTime)
    {
        List<SendReportLate> check = listsrp.FindAll(m => m.dateTime == dateTime && m.RepID == controlpage.godc.rep.RepID);// && m.Status == 0);
        if (check.Count != 0)
        {
            return true;
        }
        return false;

    }

    public bool CheckGoDCTodayForRep(int repID)
    {
        string todayDC = PlayerPrefs.GetString(KeySaving.MainData.ToString() + repID.ToString(), "");
        if (!string.IsNullOrEmpty(todayDC))
        {
            PublicClassDualCall abc = JsonMapper.ToObject<PublicClassDualCall>(todayDC);
            if (abc.dateSendReport == DateTime.Today.ToString())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
    public bool CheckRenewListReport()
    {
        if (CheckAddlistReport(listLate.listReport, DateTime.Today.ToString()))
        {
            return true;
        }

        return false;
    }

    SendReportLate l = new SendReportLate();




}
[System.Serializable]
public class DualCallReport
{
    public int AM_ID;
    public Rep rep;
    public int ObjectiveID;
    public string clientDate;
    public Step1Report step1 = new Step1Report();
    public Step2Report step2 = new Step2Report();
    public Step3Report step3 = new Step3Report();
    public Step4Report step4 = new Step4Report();
    public Step5Report step5 = new Step5Report();
    public Step6Report step6 = new Step6Report();
    public Step7Report step7 = new Step7Report();

}



[System.Serializable]
public class Step1Report
{
    public int period = -1;
    public int adoctor;
    public int bkadoctor;
    public int odoctor;
}

[System.Serializable]
public class Step2Report
{
    public string datefrom;
    public string dateto;
    public int Achieve;
    public string longterm = "";
    public string howto = "";

}

[System.Serializable]
public class Step3Report
{
    public int objective;
    public int CatchyHook;
    public int KeyMessage;
    public int EvisualAd;
    public int DocumentUsed;
    public int BrandReminder;
    public int Interaction;
    public int Comitment;
    public int QualityOfNote;
}

[System.Serializable]
public class Step4Report
{
    public string QualityNoteEvaluate;
}

[System.Serializable]
public class Step5Report
{
    public string Evaluatetheoptimization;
}

[System.Serializable]
public class Step6Report
{

    public string Evaluateknowledgeandskills;

}

[System.Serializable]
public class Step7Report
{
    public string Agreementbetween;
}

[System.Serializable]
public class ResultSentReport
{
    public int ObjectiveID { get; set; }
    public int EvaluationActivityID { get; set; }
}

[System.Serializable]
public class RootObjectSendReport
{
    public int success { get; set; }
    public int error { get; set; }
    public int error_code { get; set; }
    public string msg { get; set; }
    public ResultSentReport result { get; set; }
    public List<ResultError> result_error { get; set; }

}
[System.Serializable]
public class ResultError
{
    public int ObjectiveID;
    public int EvaluationActivityID;
    public int Evaluation_IDC;
    public string Comment;
    public int Score;
}
[System.Serializable]
public class SendReportLate
{
    public int Status;
    public string dateTime;
    public string dcreport;
    public DualCallReport dualCallReportObject;
    public PublicClassDualCall dcthisDay;

    [HideInInspector]
    public byte[] img1;
    [HideInInspector]
    public byte[] img2;
    public int ObjectID;
    public int ActivitiesID;
    public int fakeObjectId;
    public int FakeACtivitiesID;
    public int RepID;
}

[System.Serializable]
public class ClassListSendreportLate
{
    public int status;
    public List<SendReportLate> listReport = new List<SendReportLate>();
}

public class ReportAfterSend
{
    public int numSussess;
    public int numFaild;
    public List<ReportAnalytics> reportAnalytics;
}
[System.Serializable]
public class ReportAnalytics
{
    public int ObjectiveID;
    public int ActivityID;
    public string dateTime;
    public string sms;
    public string Rep;
}