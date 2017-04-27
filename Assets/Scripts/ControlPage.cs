using System;
using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using StarseedGeneral.LitJson;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class ControlPage : MonoBehaviour
{

    public string linkVPN = "http://vn1ln01.int.grs.net/api/user/vpn-connection";
    #region Variable

    public GoDualCall godc = new GoDualCall();

    public bool[] clickFromTo = new bool[2];
    public TakePic takeReport;
    [Header("page1")]
    ResultRep resultRep = new ResultRep();
    public Toggle[] toglePeriod;
    public Dropdown p1_Rep;
    public InputField[] doctorsVisited;
    public Text p1_txtTotall;
    public InputField[] txtInputP1;

    [Header("Page2")]
    public InputField[] p2_inputspage2;
    public Button[] p2_inputButton;
    public Button[] p2_btn_datetime;
    public GameObject checkmark;
    public Text[] txtpage2;
    public Text[] viewLastReport;

    public GameObject PopupExpered;

    [Header("Page3")]
    public InputField[] p3_inputspage3;
    public Dropdown[] dropdownPage3;
    public Text[] lbl_Drd_p3;
    private bool[] dropdownCheck = new bool[9];
    [Header("page 4")]
    public InputField evaluaDateOfnotes;
    public Dropdown drQUalityOfNotes;
    public GameObject page4ViewLast;
    public Text lbl_Drd_p4;

    public Transform tranformDateofnote;

    public GameObject dateNotePanel;

    [Header("page 5")]
    public InputField evaluaateQualityofVisit;
    [Header("page 6")]
    public InputField evaluateProductExpertise;
    [Header("page 7")]
    public InputField evaluateCommentAgreement;

    [Header("Over view")]
    public Text[] AmAndRep;
    public Button[] btnOverview;

    public GameObject[] buttonHidereport;

    private int oldpage = 0;
    public Text[] Inputpage1;
    public Text[] Inputpage2;
    public Text[] Inputpage4;
    public Text[] Inputpage5;
    public Text[] Inputpage6;
    public Text[] Inputpage7;


    public Text[] page1;
    public Text[] page2;
    public Text[] page3;
    public Text[] page4;
    public Text[] page5;
    public Text[] page6;
    public Text[] page7;

    [Header("Over view page2")]
    public Image[] charColums;
    public Text[] txtValuecharColums;

    public Image[] imgChar2_colums;
    public Text[] txtChar2_ValuecharColums;

    public Image[] imgChar2_Secondducall_colums;
    public Text[] txtChar2_SecondducallValuecharColums;

    public GameObject[] buttonNavigate;
    public GameObject[] pages;
    public GameObject navigation;

    List<string> options = new List<string>
    {
        "",
    };

    public Dropdown dropMember;
    public Data Datachar;
    private string message;
    public PublicClassDualCall maindata;
    private bool isNew;

    #endregion

    private bool isOpenAPp;
    public DualCallReport sendreport = new DualCallReport();
    public Text[] dateFromtoOverview2;

    private bool isSecondTime;


    #region function time

    public bool isCancle;
    #endregion

    #region Header
    [Header("Panel")]
    public GameObject waitingForChecking;
    #endregion

    void Start()
    {

        isShowZezo = false;
        //  PlayerPrefs.SetString("DualCallLate", "");
        CheckNewDate();
        LoadDataRep();
        string jsons = PlayerPrefs.GetString(KeySaving.MainData.ToString(), "");
        if (!string.IsNullOrEmpty(jsons))
        {
            maindata = JsonMapper.ToObject<PublicClassDualCall>(jsons);
        }
        else
        {
            maindata = new PublicClassDualCall();
        }

        Inputpage1[0].text = "Period on " + System.DateTime.Today.ToString("ddd dd-MMM-yyyy");
        page1[6].text = "Period on " + System.DateTime.Today.ToString("ddd dd-MMM-yyyy");
        DateTime dateFrom = DateTime.Today;
        DateTime dateTo = DateTime.Today;
        Inputpage2[0].text = dateFrom.ToString("ddd dd-MMM-yyyy");
        Inputpage2[1].text = dateTo.ToString("ddd dd-MMM-yyyy");
        p1_Rep.value = maindata.step1.idMember;

    }





    #region LoadData

    private List<Rep> listRep = new List<Rep>();

    public void LoadDataRep()
    {
        listRep = INitData.instance.initData.result.rep;
        options.Clear();
        if (listRep != null)
        {
            options.Add("Choose Member");
            for (int i = 0; i < listRep.Count; i++)
            {
                Rep dt = listRep[i];
                options.Add(dt.RepName);
            }
        }
        dropMember.AddOptions(options);
    }

    private bool isGotoday;

    public void CheckNewDate()
    {
        string today = DateTime.Today.ToString();

        string oldate = PlayerPrefs.GetString(KeySaving.CatchToday.ToString(), "");
        if (today == oldate)
        {
            // ngày hôm nay
        }
        else
        {
            //xoa het data trong display
            PlayerPrefs.SetString(KeySaving.MainData.ToString(), "");
            for (int i = 0; i < INitData.instance.initData.result.rep.Count; i++)
            {
                PlayerPrefs.SetString(KeySaving.MainData.ToString() + INitData.instance.initData.result.rep[i].RepID.ToString(), "");
            }
            PlayerPrefs.SetString(KeySaving.CatchToday.ToString(), DateTime.Today.ToString());

            for (int i = 0; i < INitData.instance.initData.result.rep.Count; i++)
            {
                PlayerPrefs.SetString(KeySaving.DataRepNotSent.ToString() + INitData.instance.initData.result.rep[i].RepID.ToString(), "");
            }
            // xoa het data
            ViewDataOnly(true);
            PlayerPrefs.SetString(KeySaving.LastReportToday.ToString(), "");
        }
    }





    public bool CheckRepTodayGoDC(int _RepID)
    {
        string todayDC = PlayerPrefs.GetString(KeySaving.MainData.ToString() + _RepID.ToString(), "");
        if (!string.IsNullOrEmpty(todayDC))
        {
            maindata = JsonMapper.ToObject<PublicClassDualCall>(todayDC);
            if (maindata.dateSendReport == DateTime.Today.ToString())
            {
                isGotoday = true;
                isSend = true;
                return true;

            }
            else
            {
                string todayNew = PlayerPrefs.GetString(KeySaving.MainData.ToString().ToString(), "");
                maindata = JsonMapper.ToObject<PublicClassDualCall>(todayNew);
                //new PublicClassDualCall();
                isGotoday = false;
                isSend = false;
                return false;
            }
        }
        return false;
    }

    #endregion

    #region Chart bar

    public GameObject[] Point;
    public Image[] Line;
    public GameObject RootChart;
    public GameObject RootPoint;
    public GameObject RootLine;
    public List<GameObject> listPoint;
    public Text lblDualcallDateChart;

    void Drawline(int[] dataValues, GameObject cPoint, Image cline, GameObject cCharRoot,
        GameObject cLineRoot, int idChart, int clineWidth = 5)
    {
        //  Debug.Log("Vo day nhieu lan?");
        listPoint.Clear();
        //cCharRoot



        for (int i = 0; i < dataValues.Length; i++)
        {
            GameObject obj = Instantiate(cPoint);
            Image p = obj.GetComponent<Image>();
            p.transform.parent = cCharRoot.transform;
            p.GetComponent<RectTransform>().anchoredPosition = new Vector2(200 * (i + 1), 100 * dataValues[i]);
            Text t = obj.transform.GetChild(0).GetComponent<Text>();
            t.text = dataValues[i].ToString(); // hien thi score cho chart
            listPoint.Add(p.gameObject);

            //create chart 
            Text txtdatechart = Instantiate(lblDualcallDateChart) as Text;
            if (isGotoday || isNotSendYet)
            {
                txtdatechart.text = "DualCall " + (i + 1).ToString() + " " + maindata.dateDualcallChart[i].ToString();
            }
            else
            {
                txtdatechart.text = "DualCall " + (i + 1).ToString() + " " + dateDualcallChart[i].ToString();
            }

            txtdatechart.transform.parent = cCharRoot.transform;
            txtdatechart.GetComponent<RectTransform>().anchoredPosition = new Vector2(200 * (i + 1), -50);
        }
        for (int i = 1; i < listPoint.Count; i++)
        {
            Image g = Instantiate(cline) as Image;
            //   g.transform.parent = cLineRoot.transform;
            g.transform.parent = cCharRoot.transform;
            Drawline(listPoint[i - 1].transform, listPoint[i].transform, clineWidth, g);
        }
    }

    void Drawline(Transform A, Transform b, int lineWidth, Image lineh)
    {

        Vector3 pointA = Vector3.zero;
        Vector3 pointB = Vector3.zero;
        pointA = A.transform.position;
        pointB = b.transform.position;
        Vector3 differenceVector = pointB - pointA;
        lineh.GetComponent<RectTransform>().sizeDelta = new Vector2(differenceVector.magnitude, lineWidth);
        lineh.GetComponent<RectTransform>().pivot = new Vector2(0, .5f);
        lineh.GetComponent<RectTransform>().position = pointA;
        float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
        lineh.GetComponent<RectTransform>().transform.rotation = Quaternion.Euler(0, 0, angle);

    }

    public void DrawChart(float[] values, Image[] imgColum, Text[] txtValue, float x, float y)
    {
        //  Debug.Log("ho" + imgColum[0].transform.parent.name);
        for (int i = 0; i < values.Length; i++)
        {
            imgColum[i].GetComponent<RectTransform>().sizeDelta = new Vector2(x, values[i] * y);
            // double t = values[i];
            double vl = System.Math.Round((double)values[i], 1, MidpointRounding.AwayFromZero);
            txtValue[i].text = vl.ToString();
        }
    }

    #endregion

    #region LoadData

    void LoadOldData()
    {
        // Debug.Log("Load old data");
        //process togle
        DataPage1();
        DataPage2();
        DataPage3();
        DataPage4();
        DataPage5();
        DataPage6();
        DataPage7();
    }



    private void DataPage7()
    {

        evaluateCommentAgreement.text = maindata.step7.evaluateCommentAgreement;

    }

    private void DataPage6()
    {

        evaluateProductExpertise.text = maindata.step6.evaluateProductExpertise;

    }

    private void DataPage5()
    {

        evaluaateQualityofVisit.text = maindata.step5.evaluaateQualityofVisit;

    }

    private void DataPage4()
    {
        Datachar.data[8] = maindata.step3.dataChar[8];
        evaluaDateOfnotes.text = maindata.step4.evaluaateQualityofNotes;

        drQUalityOfNotes.value = (int)Datachar.data[8];
        //14day of notes
    }


    public void Load14daysofnotes()
    {

        //  INitData.instance.initData.g
        List<DaysNote> dt = godc.daysNote;
        if (dt.Count > 0)
        {
            //tranformDateofnote.DetachChildren();
            foreach (Transform child in tranformDateofnote)
            {
                Destroy(child.gameObject);
            }

            RectTransform rt = tranformDateofnote.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(0, 180 * dt.Count);
            for (int i = 0; i < dt.Count; i++)
            {
                GameObject abs = Instantiate(dateNotePanel);

                abs.transform.GetChild(0).GetComponent<Text>().text = i.ToString() + ".";
                abs.transform.GetChild(1).GetComponent<Text>().text =
                    dt[i].product.ToString();
                abs.transform.GetChild(2).GetComponent<Text>().text =
                    dt[i].doctorsPharmacicists;
                abs.transform.GetChild(3).GetComponent<Text>().text =
                    dt[i].date.Substring(0, 11);
                abs.transform.GetChild(4).GetComponent<Text>().text =
                    dt[i].comment.ToString();
                abs.transform.parent = tranformDateofnote;

            }

        }


    }

    private void DataPage3()
    {
        Datachar.data = maindata.step3.dataChar;

        for (int i = 0; i < 8; i++)
        {

            dropdownPage3[i].value = (int)Datachar.data[i];
        }

    }

    public bool CheckDualCallate(string date)
    {
        if (!string.IsNullOrEmpty(date))
        {
            date = date.Substring(0, 11);

            DateTime today = DateTime.Today;
            string td = DateTime.Today.ToString("dd-MMM-yyyy");
            IFormatProvider culture = new System.Globalization.CultureInfo("fr-FR", true);
            today = DateTime.Parse(td, culture);
            DateTime t0 = DateTime.Parse(date, culture);
            return today > t0 ? true : false;
        }
        return false;

    }



    private bool isDuaCallLate;

    private void DataPage2()
    {
        if (string.IsNullOrEmpty(maindata.step2.datefrom))
        {
            //  DateTime dateFrom = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime dateFrom = DateTime.Today;
            maindata.step2.datefrom = dateFrom.ToString("dd-MMM-yyyy");
            Inputpage2[0].gameObject.SetActive(false);
            clickFromTo[0] = false;
        }
        else
        {
            clickFromTo[0] = true;
        }
        if (string.IsNullOrEmpty(maindata.step2.dateto))
        {
            //  DateTime dateTo = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
            DateTime dateTo = DateTime.Today;
            maindata.step2.dateto = dateTo.ToString("dd-MMM-yyyy");
            Inputpage2[1].gameObject.SetActive(false);
            clickFromTo[1] = false;

        }
        else
        {
            clickFromTo[1] = true;
        }



        Inputpage2[0].text = maindata.step2.datefrom.Substring(0, 11);
        Inputpage2[1].text = maindata.step2.dateto.Substring(0, 11);
        if (!string.IsNullOrEmpty(maindata.step2.longterm))
        {
            // p2_inputspage2[0].interactable = false;
            Inputpage2[0].gameObject.SetActive(true);// dis play date time
            Inputpage2[1].gameObject.SetActive(true);
            clickFromTo[0] = true;
            clickFromTo[1] = true;
            p2_btn_datetime[0].interactable = false;
            p2_btn_datetime[1].interactable = false;
        }
        p2_inputspage2[0].text = maindata.step2.longterm;
        p2_inputspage2[1].text = maindata.step2.howto;

        if (maindata.step2.Achieve == 1)
        {
            checkmark.SetActive(true);
        }
        else
        {
            checkmark.SetActive(false);
        }

    }




    void GoPage2()
    {
        if (maindata.step2.Achieve == 0)
        {
            //kiem tra da hoan thanh dual call truowc han hay sau han
            if (CheckDualCallate(maindata.step2.dateto))
            {
                if (!isDisPlayPopup && !isDisplayExpire)
                    PopupExpered.SetActive(true);
            }
        }
    }

    private void DataPage1()
    {

        AmAndRep[0].text = UserAuthentication.instance.aminfo.AM_Name;
        if (options.Count > 0 && maindata.step1.idMember != -1)
            AmAndRep[1].text = options[maindata.step1.idMember];
        else
        {
            AmAndRep[1].text = "";
        }
        if (maindata.step1.period == 0)
        {
            toglePeriod[0].isOn = true;
        }
        if (maindata.step1.period == 1)
        {
            toglePeriod[1].isOn = true;
        }
        if (maindata.step1.period == 2)
        {
            toglePeriod[2].isOn = true;
        }
        if (maindata.step1.period == -1)
        {
            toglePeriod[3].isOn = true;


        }

        //process dropdown Rep
        if (maindata.step1.adoctor >= 0)
        {
            // checkstep1Doctor[0] = true;
            doctorsVisited[0].text = maindata.step1.adoctor.ToString();
        }

        else
        {
            doctorsVisited[0].text = "";
            //if (isGotoday || isNotSendYet || isShowZezo)
            //{
            //    doctorsVisited[0].text = "0";
            //}

        }
        if (maindata.step1.bkadoctor >= 0)
        {
            // checkstep1Doctor[1] = true;
            doctorsVisited[1].text = maindata.step1.bkadoctor.ToString();

        }

        else
        {
            doctorsVisited[1].text = "";
            //if (isGotoday || isNotSendYet || isShowZezo)
            //{
            //    doctorsVisited[1].text = "0";
            //}
        }
        //checkstep1Doctor[2] = true;
        if (maindata.step1.odoctor >= 0)
        {

            doctorsVisited[2].text = maindata.step1.odoctor.ToString();
        }
        else
        {
            doctorsVisited[2].text = "";
            //if (isGotoday || isNotSendYet || isShowZezo)
            //{
            //    doctorsVisited[2].text = "0";
            //}
        }
        p1_txtTotall.text = maindata.step1.CaculateTotalDoctors().ToString();
        CaculateTotalPage1();
    }

    #endregion


    #region page1 event

    public Text txtTotal;

    //  private bool[] checkstep1Doctor = new bool[3] { false, false, false };

    public void OnEndEditInputA(string value)
    {
        // checkstep1Doctor[0] = true;
        if (string.IsNullOrEmpty(value))
        {
            value = "0";
        }
        maindata.step1.adoctor = int.Parse(value);
        if (maindata.step1.adoctor < 0)
        {
            maindata.step1.adoctor = 0;
            txtInputP1[0].text = "0";
        }
        CaculateTotalPage1();

        SaveDataInput();
    }

    public void SaveDataInput()
    {
        string todaySave = JsonMapper.ToJson(maindata);

        if (!string.IsNullOrEmpty(todaySave))
        {
            PlayerPrefs.SetString(KeySaving.MainData.ToString(), todaySave);
        }
    }

    public void OnEndEditInputB(string value)
    {
        //  checkstep1Doctor[1] = true;
        if (string.IsNullOrEmpty(value))
        {
            value = "0";
        }
        maindata.step1.bkadoctor = int.Parse(value);
        if (maindata.step1.bkadoctor < 0)
        {
            maindata.step1.bkadoctor = 0;
            txtInputP1[1].text = "0";
        }


        CaculateTotalPage1();
        SaveDataInput();
    }

    public void OnEndEditInputC(string value)
    {
        //checkstep1Doctor[2] = true;
        if (string.IsNullOrEmpty(value))
        {
            value = "0";
        }
        maindata.step1.odoctor = int.Parse(value);
        if (maindata.step1.odoctor < 0)
        {
            maindata.step1.odoctor = 0;
            txtInputP1[2].text = "0";
        }

        CaculateTotalPage1();
        SaveDataInput();
    }

    public void CaculateTotalPage1()
    {

        txtTotal.text = maindata.step1.CaculateTotalDoctors().ToString();
        if (maindata.step1.adoctor >= 0)
            page1[0].text = maindata.step1.adoctor.ToString();
        if (maindata.step1.bkadoctor >= 0)
            page1[1].text = maindata.step1.bkadoctor.ToString();
        if (maindata.step1.odoctor >= 0)
            page1[2].text = maindata.step1.odoctor.ToString();
        page1[3].text = maindata.step1.CaculateTotalDoctors().ToString();
    }
    PublicClassDualCall dcGo = new PublicClassDualCall();
    public void LoadOldDataInCaseFinish(int repID)
    {
        isShowZezo = true;
        string t = PlayerPrefs.GetString(KeySaving.MainData.ToString() + repID.ToString(), "");
        maindata = JsonMapper.ToObject<PublicClassDualCall>(t);

    }


    private bool isOpenapp = false;
    private bool isFirst = false;
    private bool isSend;
    private bool isNotSendYet = false;


    private bool isShowZezo;
    public void SelectMember(int id)
    {

        dropMember.options[0].text = "";
        isShowZezo = false;
        page1[5].text = options[dropMember.value]; // for overview page 2
        {
            if (id != 0) // chua send report
            {
                isDisplayExpire = false;
                isNotSendYet = false;
                int idxxx = listRep[id - 1].RepID;

                godc = INitData.instance.GetDualCall(idxxx);
                godc.rep = listRep[id - 1];
                sendreport.rep = godc.rep;
                if (CheckRepTodayGoDC(idxxx))
                {
                    //   Debug.Log("Hom nay di dual call roi");
                    ViewDataOnly(false);
                    isGotoday = true;
                    // maindata không thay đổi lấy từ trong dc save
                    // LoadOldActivity();
                    LoadOldDataInCaseFinish(godc.rep.RepID);



                }
                else
                {
                    isGotoday = false;

                    ViewDataOnly(true);
                    if (!isFirst)
                    {
                        string tds = PlayerPrefs.GetString(KeySaving.MainData.ToString(), "");
                        if (!string.IsNullOrEmpty(tds))
                        {
                            isShowZezo = true;
                            maindata = JsonMapper.ToObject<PublicClassDualCall>(tds);
                        }




                        if (godc.longtermObject != null)
                        {
                            // longterm.ObjectiveID;
                            //      Debug.Log("Có dualcall chưa hoàn thnahf");
                            p2_inputspage2[0].interactable = false;
                            Inputpage2[0].gameObject.SetActive(true); // dis play date time
                            Inputpage2[1].gameObject.SetActive(true);
                            clickFromTo[0] = false;
                            clickFromTo[1] = false;
                            p2_btn_datetime[0].interactable = false;
                            p2_btn_datetime[1].interactable = false;
                            maindata.longterm.ObjectiveID = godc.longtermObject.ObjectiveID;
                            maindata.step2.longterm = godc.longtermObject.computed;
                            maindata.step2.Achieve = godc.longtermObject.StatusID;
                            maindata.step2.datefrom = godc.longtermObject.FromDate;
                            maindata.step2.dateto = godc.longtermObject.ToDate;

                        }
                    }
                    else
                    {
                        maindata = new PublicClassDualCall();
                        if (godc.longtermObject == null)
                        {
                            isShowZezo = false;
                            CreateNewActivity();
                        }
                        else
                        {
                            LoadOldActivity();
                        }

                    }

                    //case load data fill already
                    string dt = PlayerPrefs.GetString(KeySaving.DataRepNotSent.ToString() + godc.rep.RepID.ToString(), "");
                    if (!string.IsNullOrEmpty(dt))
                    {
                        isShowZezo = true;
                        maindata = JsonMapper.ToObject<PublicClassDualCall>(dt);
                        LoadOldActivity();
                        ViewDataOnly(false);
                        btnOverview[0].interactable = true;
                        Color temp = btnOverview[0].image.color;
                        temp.a = .3f;
                        btnOverview[0].image.color = temp;
                        isDisplayExpire = true;

                        isNotSendYet = true;
                    }

                }
            }
            else
            {
                dropMember.options[0].text = "";
                dropMember.captionText.text = "Choose member";
            }

            isFirst = true;
            if (!isGotoday)
            {
                maindata.step1.idMember = dropMember.value;

                SaveDataInput();
                if (dropMember.value == 0)
                {


                    Color temp = dropMember.image.color;
                    temp.a = .3f;
                    dropMember.image.color = temp;

                }
                else
                {
                    Color temp = dropMember.image.color;
                    temp.a = 1f;
                    dropMember.image.color = temp;
                }
            }
            LoadOldData();
        }
    }

    public bool isNotSend = false;


    void SetCheckCondition()
    {
        sendreport.ObjectiveID = 0;
        p2_inputspage2[0].interactable = true;
        p2_inputspage2[0].text = "";
        clickFromTo[0] = true;
        clickFromTo[1] = true;
        p2_btn_datetime[0].interactable = true;
        p2_btn_datetime[1].interactable = true;


    }

    private void DropSelectRep(int id)
    {
        page1[5].text = options[dropMember.value]; // for overview page 2
        if (isOpenapp)
        {
            if (id != 0) // chua send report
            {
                int idxxx = listRep[id - 1].RepID;

                godc = INitData.instance.GetDualCall(idxxx);
                godc.rep = listRep[id - 1];
                sendreport.rep = godc.rep;

                maindata = new PublicClassDualCall();
                if (godc.longtermObject == null)
                {
                    //     Debug.Log("tao moi dual call");
                    godc.longtermObject = new LongTermObject();
                    sendreport.ObjectiveID = 0;
                    p2_inputspage2[0].interactable = true;
                    p2_inputspage2[0].text = "";
                    clickFromTo[0] = true;
                    clickFromTo[1] = true;
                    p2_btn_datetime[0].interactable = true;
                    p2_btn_datetime[1].interactable = true;
                    maindata.longterm = godc.longtermObject;
                }
                else
                {
                    //    Debug.Log("co dua call chua hoan thanh");
                    isDisPlayPopup = false;
                    sendreport.ObjectiveID = godc.longtermObject.ObjectiveID;
                    p2_inputspage2[0].text = godc.longtermObject.computed;
                    p2_inputspage2[0].interactable = false;
                    Inputpage2[0].gameObject.SetActive(true); // dis play date time
                    Inputpage2[1].gameObject.SetActive(true);
                    clickFromTo[0] = false;
                    clickFromTo[1] = false;
                    p2_btn_datetime[0].interactable = false;
                    p2_btn_datetime[1].interactable = false;
                    maindata.step2.longterm = godc.longtermObject.computed;
                    maindata.step2.Achieve = godc.longtermObject.StatusID;
                    maindata.step2.datefrom = godc.longtermObject.FromDate;
                    maindata.step2.dateto = godc.longtermObject.ToDate;
                    // kiem tra xem longterm het han?
                    maindata.longterm = godc.longtermObject;
                }
            }
        }
        else
        {
            //mo lan dau load lai data cu
            if (id != 0) // chua send report
            {
                int idxxx = listRep[id - 1].RepID;
                godc = INitData.instance.GetDualCall(idxxx);
                godc.rep = listRep[id - 1];
                sendreport.rep = godc.rep;
                // maindata = new PublicClassDualCall();
                if (godc.longtermObject == null)
                {
                    CreateNewActivity();
                }
                else
                {
                    LoadOldActivity();
                }
            }
        }
        isOpenapp = true;
        maindata.step1.idMember = dropMember.value;
        LoadOldData();
        SaveDataInput();
    }


    //  Action<bool> action
    private void LoadOldActivity()
    {
        // Debug.Log("co dua call chua hoan thanh");
        isDisPlayPopup = false;
        sendreport.ObjectiveID = godc.longtermObject.ObjectiveID;
        p2_inputspage2[0].text = godc.longtermObject.computed;
        p2_inputspage2[0].interactable = false;
        Inputpage2[0].gameObject.SetActive(true); // dis play date time
        Inputpage2[1].gameObject.SetActive(true);
        clickFromTo[0] = false;
        clickFromTo[1] = false;
        p2_btn_datetime[0].interactable = false;
        p2_btn_datetime[1].interactable = false;
        //    maindata.longterm.ObjectiveID = godc.longtermObject.ObjectiveID;
        maindata.step2.longterm = godc.longtermObject.computed;
        maindata.step2.Achieve = godc.longtermObject.StatusID;
        maindata.step2.datefrom = godc.longtermObject.FromDate;
        maindata.step2.dateto = godc.longtermObject.ToDate;
        // kiem tra xem longterm het han?
        maindata.longterm = godc.longtermObject;
    }

    private void CreateNewActivity()
    {
        godc.longtermObject = maindata.longterm;
        //     Debug.Log("tao moi dual call");
        godc.longtermObject = new LongTermObject();
        sendreport.ObjectiveID = 0;
        p2_inputspage2[0].interactable = true;
        p2_inputspage2[0].text = "";
        clickFromTo[0] = true;
        clickFromTo[1] = true;
        p2_btn_datetime[0].interactable = true;
        p2_btn_datetime[1].interactable = true;

    }

    #endregion

    #region page2 event

    public void OnLongtermEdit(string text)
    {
        maindata.step2.longterm = text;
        SaveDataInput();
    }

    public void OnHowtoEdit(string text)
    {
        maindata.step2.howto = text;
        SaveDataInput();
    }

    public void OnAchieveClick()
    {
        if (checkmark.activeSelf)
        {
            checkmark.SetActive(false);
            maindata.step2.Achieve = 0;
        }
        else
        {
            checkmark.SetActive(true);
            maindata.step2.Achieve = 1;
        }
        SaveDataInput();
    }

    private bool isCreateNew = false;

    public GoDualCall objectExpired;
    public int LongtermID;
    private int RepID;
    public void OncreateNewLongterm()
    {
        isDisPlayPopup = true;
        isCreateNew = true;
        PopupExpered.SetActive(false);
        objectExpired = godc;
        godc.dualCalllists.Clear();
        godc.longtermObject = new LongTermObject();
        godc.infor = UserAuthentication.instance.aminfo;
        godc.rep = objectExpired.rep;
        godc.daysNote = objectExpired.daysNote;
        godc.infor = objectExpired.infor;
        sendreport.ObjectiveID = 0;
        // remove longtermID 
        LongtermID = godc.longtermObject.ObjectiveID;
        LongtermID = godc.longtermObject.RepID;

        p2_inputspage2[0].interactable = true;
        p2_btn_datetime[0].interactable = true;
        p2_btn_datetime[1].interactable = true;
        p2_btn_datetime[0].interactable = true;
        //Debug.Log("tao moi dual call");

        p2_inputspage2[0].text = "";
        clickFromTo[0] = false;
        clickFromTo[1] = false;


        INitData.instance.initData.result.longTermObject.Remove(objectExpired.longtermObject);
        // INitData.instance.initData.result.evaluationActivity.RemoveAll(m => m.RepID == maindata.longterm.RepID);
        INitData.instance.SaveChangeData();
        maindata.longterm = new LongTermObject();

        PublicClassDualCall pc = new PublicClassDualCall();
        pc = maindata;
        maindata = new PublicClassDualCall();
        maindata.step1 = pc.step1;

        LoadOldData();
    }

    private bool isDisPlayPopup;
    public void OnContinues()
    {
        isDisPlayPopup = true;
        PopupExpered.SetActive(false);

    }

    #endregion

    #region page3 event



    public void DropdownObjective(int id)
    {

        Datachar.data[0] = id;
        Datachar.RefreshData();
        SaveDataInput();
    }

    public void DropdownCatchyKook(int id)
    {

        Datachar.data[1] = id;
        Datachar.RefreshData();
        SaveDataInput();
    }

    public void DropdownKeyMessage(int id)
    {

        Datachar.data[2] = id;

        Datachar.RefreshData();
        SaveDataInput();
    }

    public void DropdownEvisualAds(int id)
    {

        Datachar.data[3] = id;

        Datachar.RefreshData();
        SaveDataInput();
    }

    public void DropdownDocumentUsed(int id)
    {

        Datachar.data[4] = id;

        Datachar.RefreshData();
        SaveDataInput();
    }

    public void DropdownBrandREminder(int id)
    {

        Datachar.data[5] = id;

        Datachar.RefreshData();
        SaveDataInput();
    }

    public void DropdownInteraction(int id)
    {
        Datachar.data[6] = id;

        Datachar.RefreshData();
        SaveDataInput();
    }

    public void DropdownComitment(int id)
    {

        Datachar.data[7] = id;

        Datachar.RefreshData();
        SaveDataInput();
    }

    public void OnDropDownPage3Click(int dropdownID)
    {

        Datachar.RefreshData();
        SaveDataInput();
    }


    #endregion

    #region page4 event

    public void OnEvauaQualityOfNoteEdit(string text)
    {
        maindata.step4.evaluaateQualityofNotes = text;
        SaveDataInput();
    }

    private bool isF;

    public void DropdownQualityOfNotes(int id)
    {

        Datachar.data[8] = id;

        SaveDataInput();
    }

    public void OnDropPage4Click()
    {


        SaveDataInput();
    }

    #endregion

    #region page5 event

    public void OnEvaluaateQualityofVisitEdit(string text)
    {
        maindata.step5.evaluaateQualityofVisit = text;
        SaveDataInput();
    }

    #endregion

    #region page6 event

    public void OnEvaluateProductExpertiseEdit(string text)
    {
        maindata.step6.evaluateProductExpertise = text;
        SaveDataInput();
    }

    #endregion

    #region page6 event

    public void OnEvaluateCommentAgreementEdit(string text)
    {
        maindata.step7.evaluateCommentAgreement = text;
        SaveDataInput();
    }

    #endregion


    public void PageClick(int pageID)
    {
        scroll.verticalScrollbar.value = 1;
        scroll.enabled = false;
        bool isGo = false;
        switch (pageID)
        {
            case 0:
            case 1:
                isGo = true;
                break;
            case 2:
                if (CheckStep1())
                {
                    isGo = true;
                    GoPage2();
                }
                break;
            case 3:
                if (CheckStep2())
                {
                    if (!maindata.step2.ValidateDaytime())
                    {
                        message = "Date to must be greater than date from";
                        ShowMesage(message);
                        return;
                    }


                    isGo = true;
                }
                break;
            case 4:
                if (CheckStep3())
                {
                    Load14daysofnotes();
                    if (!maindata.step2.ValidateDaytime())
                    {
                        message = "Date to must be greater than date from";
                        ShowMesage(message);
                        return;
                    }
                    isGo = true;
                }
                break;
            case 5:
                if (CheckStep4())
                {
                    if (!maindata.step2.ValidateDaytime())
                    {
                        message = "Date to must be greater than date from";
                        ShowMesage(message);
                        return;
                    }
                    //check luon step 4
                    if (maindata.step3.dataChar[8] == 0)
                    {
                        isGo = false;
                    }
                    else
                    {
                        isGo = true;
                    }

                }
                break;
            case 6:
                if (CheckStep5())
                {
                    if (!maindata.step2.ValidateDaytime())
                    {
                        message = "Date to must be greater than date from";
                        ShowMesage(message);
                        return;
                    }
                    isGo = true;
                }
                break;
            case 7:
                if (CheckStep6())
                {
                    if (!maindata.step2.ValidateDaytime())
                    {
                        message = "Date to must be greater than date from";
                        ShowMesage(message);
                        return;
                    }
                    isGo = true;
                }
                break;
            case 8:
                isGo = true;
                break;
        }
        if (isGo)
        {

            EnablePage(pageID);
        }
        else
        {
            message = "Please complete all information before going to next step";
            ShowMesage(message);

        }

    }

    bool CheckStep1()
    {
        bool chk = !string.IsNullOrEmpty(txtInputP1[0].text.ToString());
        if (chk)
        {
            chk = !string.IsNullOrEmpty(txtInputP1[1].text.ToString());
        }
        if (chk)
        {
            chk = !string.IsNullOrEmpty(txtInputP1[2].text.ToString());
        }
        return chk && !(maindata.step1.period == -1) && !(maindata.step1.idMember == 0);

    }

    bool CheckStep2()
    {
        bool check = false;
        // check = maindata.step1.CheckData();
        check = CheckStep1();
        if (check == true)
            check = maindata.step2.CheckData();
        if (check == true)
            check = clickFromTo[0] && clickFromTo[1];

        return check;
    }

    bool CheckStep3()
    {
        bool check = false;
        // check = maindata.step1.CheckData();
        check = CheckStep1();
        if (check == true)
            check = maindata.step2.CheckData();
        if (check == true)
            check = maindata.step3.CheckData();
        if (check == true)
            check = clickFromTo[0] && clickFromTo[1];
        return check;

    }

    bool CheckStep4()
    {
        bool check = false;
        //  check = maindata.step1.CheckData();
        check = CheckStep1();
        if (check == true)
            check = maindata.step2.CheckData();
        if (check == true)
            check = maindata.step3.CheckData();
        if (check == true)
            check = maindata.step4.CheckData();
        if (check == true)
            check = clickFromTo[0] && clickFromTo[1];
        return check;

    }

    bool CheckStep5()
    {
        bool check = false;
        //  check = maindata.step1.CheckData();
        check = CheckStep1();
        if (check == true)
            check = maindata.step2.CheckData();
        if (check == true)
            check = maindata.step3.CheckData();
        if (check == true)
            check = maindata.step4.CheckData();
        if (check == true)
            check = maindata.step5.CheckData();
        if (check == true)
            check = clickFromTo[0] && clickFromTo[1];
        return check;

    }

    bool CheckStep6()
    {
        bool check = false;
        //  check = maindata.step1.CheckData();
        check = CheckStep1();
        if (check == true)
            check = maindata.step2.CheckData();
        if (check == true)
            check = maindata.step3.CheckData();
        if (check == true)
            check = maindata.step4.CheckData();
        if (check == true)
            check = maindata.step5.CheckData();
        if (check == true)
            check = maindata.step6.CheckData();
        if (check == true)
            check = clickFromTo[0] && clickFromTo[1];
        return check;

    }

    bool CheckStep7()
    {
        bool check = false;
        // check = maindata.step1.CheckData();
        check = CheckStep1();
        if (check == true)
            check = maindata.step2.CheckData();
        if (check == true)
            check = maindata.step3.CheckData();
        if (check == true)
            check = maindata.step4.CheckData();
        if (check == true)
            check = maindata.step5.CheckData();
        if (check == true)
            check = maindata.step6.CheckData();
        if (check == true)
            check = maindata.step7.CheckData();
        if (check == true)
            check = clickFromTo[0] && clickFromTo[1];
        return check;

    }


    public GameObject showMessagePanel;
    public Text mesageShow;

    public void ShowMesage(string sms)
    {
        mesageShow.text = sms;
        showMessagePanel.SetActive(true);
    }

    public void HideMesage()
    {
        mesageShow.text = "";
        showMessagePanel.SetActive(false);

    }

    public void ChangeScence()
    {
        SceneManager.LoadScene(SceneName.Loading.ToString());
        //    Application.LoadLevel(SceneName.Loading.ToString());
    }
    void EnablePage(int pageID)
    {
        if (INitData.instance.CheckOpenApp())
        {
            ShowMesage("your app open more than one day. this will be reset to the new day");
            INitData.instance.LoginTime = DateTime.Today;
            Invoke("ChangeScence", 4f);
        }
        //Debug.Log(pageID);
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
        }
        pages[pageID].SetActive(true);
        if (pageID == 0 || pageID == 8)
        {
            navigation.SetActive(false);
        }
        else
        {
            navigation.SetActive(true);
        }
        if (pageID == 2)
        {
            page4ViewLast.SetActive(true);

        }
        else
        {
            page4ViewLast.SetActive(false);

        }
        if (pageID == 8)
        {
            DataOveView();
            int s = 0, ratio = 1;
            for (int i = 0; i < Datachar.data.Length; i++)
            {
                s = s + (int)Datachar.data[i];
            }
            if (s > 30)
            {
                ratio = 2;
            }
            if (isGotoday || isNotSendYet)
            {
                if (maindata.firstDualcall[0] > 0)
                {

                    DrawChart(maindata.firstDualcall, charColums, txtValuecharColums, 100, 30 / ratio); //first dual call
                }
                if (maindata.lastDualCall != null)
                {
                    if (maindata.lastDualCall[0] > 0)
                    {

                        DrawChart(maindata.lastDualCall, imgChar2_Secondducall_colums, txtChar2_SecondducallValuecharColums, 100, 30 / ratio);
                    }
                }


                DrawChart(maindata.average, imgChar2_colums, txtChar2_ValuecharColums, 100, 50); //chart everage score

                Drawline(maindata.Line1, Point[0], Line[0], RootChart, RootLine, 10); //interaction chart
                Drawline(maindata.Line2, Point[1], Line[1], RootChart, RootLine, 10); //commitmentChart
                Drawline(maindata.Line3, Point[2], Line[2], RootChart, RootLine, 10); //qualityofnotes chart 
            }
            else
            {

                GetChartAverage();

                GetFirstDualCallChart();


                if (fistDualCallChart[0] > 0)
                {
                    maindata.firstDualcall = fistDualCallChart;
                    DrawChart(fistDualCallChart, charColums, txtValuecharColums, 100, 30 / ratio); //first dual call
                }

                if (lastDualCallChart[0] > 0)
                {
                    maindata.lastDualCall = Datachar.data;
                    DrawChart(Datachar.data, imgChar2_Secondducall_colums, txtChar2_SecondducallValuecharColums, 100, 30 / ratio);
                }


                maindata.average = chartEverage;

                DrawChart(chartEverage, imgChar2_colums, txtChar2_ValuecharColums, 100, 50); //chart everage score
                caculateDataChartLine();//chart line
            }


            SaveDataInput();

            // btnSetting.SetActive(false);
        }
        //else
        //{
        //    btnSetting.SetActive(true);
        //}
    }

    public GameObject btnSetting;



    #region DataOverView

    void DataOveView()
    {
        foreach (Transform child in RootChart.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in RootLine.transform)
        {
            Destroy(child.gameObject);
        }

        Page1();
        Page2();
        Page3();
        Page4();
        Page5();
        Page6();
        Page7();
        PageOverview2();
        DisPlayOverviewPage2(); // for second image to take report

    }

    public GameObject[] enablePeriod;

    private void Page1()
    {
        switch (maindata.step1.period)
        {
            case 0:
                SetPeriod(0);
                break;
            case 1:
                SetPeriod(1);
                break;
            case 2:
                SetPeriod(2);
                break;
        }
        CaculateTotalPage1();


    }

    public void SetPeriod(int id)
    {
        for (int i = 0; i < enablePeriod.Length; i++)
        {
            enablePeriod[0].SetActive(false);
        }
        enablePeriod[id].SetActive(true);
    }

    public Text overViewLable;
    private void PageOverview2()
    {
        dateFromtoOverview2[0].text = "From " + maindata.step2.datefrom;
        dateFromtoOverview2[1].text = "To " + DateTime.Today.ToString("dd-MMM-yyyy"); // to

       // overViewLable.text = "Overview \n PERFORMANCE BY MR / " + UserAuthentication.instance.aminfo.TerritoryName;
        overViewLable.text = "Overview \n PERFORMANCE BY MR / " + godc.rep.TerritoryName;
        //UserAuthentication.instance.aminfo.TerritoryName;


    }

    private void Page7()
    {
        page7[0].text = maindata.step7.evaluateCommentAgreement;
    }

    private void Page6()
    {
        page6[0].text = maindata.step6.evaluateProductExpertise;
    }

    private void Page5()
    {
        page5[0].text = maindata.step5.evaluaateQualityofVisit;
    }

    private void Page4()
    {

        page4[0].text = maindata.step4.evaluaateQualityofNotes;

    }

    private void Page3()
    {
        for (int i = 0; i < Datachar.data.Length; i++)
        {
            page3[i].text = Datachar.data[i].ToString();
        }
        RefreshData();
    }

    private void Page2()
    {
        page2[0].text = "From " + maindata.step2.datefrom;
        page2[1].text = "To " + maindata.step2.dateto; // to
        page2[2].text = maindata.step2.longterm;
        page2[3].text = maindata.step2.howto;
    }

    #endregion



    #region overView



    /// <summary>
    /// char for over view
    /// </summary>
    public GameObject[] overviewPage;

    public void OnButtonPageOverViewClick(int id)
    {
        //Debug.Log(id);
        if (id == 0)
        {
            overviewPage[0].SetActive(true);
            overviewPage[1].SetActive(false);
            DisPlayOverviewPage2(); //show report page2 to take image

        }
        else
        {
            overviewPage[1].SetActive(true);
            overviewPage[0].SetActive(false);
            overviewPage[1].GetComponent<RectTransform>().offsetMin =
                new Vector2(overviewPage[1].GetComponent<RectTransform>().offsetMin.x, 0);
            overviewPage[1].GetComponent<RectTransform>().offsetMax =
                new Vector2(overviewPage[1].GetComponent<RectTransform>().offsetMax.x, 0);

        }

    }

    public GameObject ConfirmSend;
    public Transform ChartRoot;
    //  public bool isViewOnly;

    public void RefreshData()
    {

        for (int i = 0; i < Datachar.data.Length; i++)
        {
            ChartRoot.GetChild(i).localScale = new Vector3(1, Datachar.data[i], 1);

            if (Datachar.data[i] > 0)
            {

                ChartRoot.GetChild(i).GetComponent<RectTransform>().anchoredPosition =
                    new Vector2(ChartRoot.GetChild(i).GetComponent<RectTransform>().anchoredPosition.x,
                        20 * (Datachar.data[i]) - 20);
            }

        }
    }

    public void Exit()
    {
        ConfirmExit.SetActive(true);

    }

    public void SaveAndSend()
    {
        CheckVPNConnection();
    }

    public void ReadyForSend()
    {
        panelCheckVPN.SetActive(false);
        if (!ConfirmSend.activeSelf)
        {
            if (CheckStep7())
            {
                if (!takeReport.CheckGoDCTodayForRep(godc.rep.RepID))
                {
                    ConfirmSend.SetActive(true);
                }
                else
                {
                    //da di roi
                    preventClick.SetActive(false);
                    ConfirmSend.SetActive(false);
                    ShowMesage("Today you already go dualcall for this REP");
                }
            }
            else
            {
                message = "Please complete all information before going to next step";
                ShowMesage(message);
            }
        }
    }

    public void DisPlayOverviewPage2()
    {

        overviewPage[1].SetActive(true);
        overviewPage[1].GetComponent<RectTransform>().offsetMin =
            new Vector2(overviewPage[1].GetComponent<RectTransform>().offsetMin.x, -4068);
        overviewPage[1].GetComponent<RectTransform>().offsetMax =
            new Vector2(overviewPage[1].GetComponent<RectTransform>().offsetMax.x, -4068);
    }

    public GameObject PopupWaiting;
    public void SendYes()
    {
        //if (!CheckGoToday())

        PopupWaiting.SetActive(true);
        ConfirmSend.SetActive(false);
        Invoke("Sendx", .2f);
        preventClick.SetActive(true);


    }




    private bool Check = false;
    IEnumerator checkInternetConnection(Action<bool> action)
    {

        Check = true;
        WWW www = new WWW(linkVPN);
        yield return www;
        if (www.error != null)
        {
            Check = false;
            if (waitingForChecking.activeSelf)
                waitingForChecking.SetActive(false);
            action(false);
        }
        else
        {
            Check = false;
            if (waitingForChecking.activeSelf)
                waitingForChecking.SetActive(false);
            action(true);
        }
    }


    public void CheckVPNConnection()
    {

        if (!waitingForChecking.activeSelf)
        {
            waitingForChecking.SetActive(true);
            StartCoroutine(checkInternetConnection(Connection));
        }

    }

    public GameObject panelCheckVPN;
    void Connection(bool isConnect)
    {

        if (!isConnect)
        {
            if (!panelCheckVPN.activeSelf)
            {
                panelCheckVPN.SetActive(true);
            }
        }
        else
        {
            ReadyForSend();
        }

    }

    public void NOVPN()
    {
        if (panelCheckVPN.activeSelf)
        {
            panelCheckVPN.SetActive(false);
        }

    }

    public GameObject preventClick;

    void Sendx()
    {

       // takeReport.CreateSendReportAndSaveLocal();
        //  CreateFakeData();


        CreateFakeData1();


    }

    public void CreateFakeData1()
    {
        if (!takeReport.CheckSendReportToday())
        {
            takeReport.CreateSendReportAndSaveLocal();
            int[] fakeID = new int[2];
            int generateObjectID = UnityEngine.Random.Range(900000000, 999999999);
            while (!CheckObjectiveIdInData(generateObjectID))
            {
                generateObjectID = UnityEngine.Random.Range(900000000, 999999999);
            }
            int generateActivityID = UnityEngine.Random.Range(900000000, 999999999);
            while (!CheckACtivityIDInData(generateActivityID))
            {
                generateActivityID = UnityEngine.Random.Range(900000000, 999999999);
            }
            fakeID[0] = generateObjectID;
            fakeID[1] = generateActivityID;


            if (sendreport.ObjectiveID == 0)
            {
                if (maindata.step2.Achieve == 0)
                {
                    LongTermObject ob = CreateLongterm(generateObjectID);
                    List<EvaluationActivity> activitiesList = CreateActivityDetail(sendreport.rep.RepID, generateObjectID,
                        generateActivityID);
                    INitData.instance.initData.result.evaluationActivity.AddRange(activitiesList);
                    INitData.instance.initData.result.longTermObject.Add(ob);
                }
            }
            else
            {

                fakeID[0] = sendreport.ObjectiveID;

                if (maindata.step2.Achieve == 1)
                {
                    //hoan thanh update va xoa
                    INitData.instance.initData.result.longTermObject.Remove(
                        INitData.instance.initData.result.longTermObject.Find(m => m.RepID == godc.rep.RepID));
                    INitData.instance.initData.result.evaluationActivity.RemoveAll(a => a.RepID == godc.rep.RepID);
                }
                else
                {
                    // Debug.Log("update longterm and add new dualcall");
                    List<EvaluationActivity> activitiesList = CreateActivityDetail(godc.rep.RepID, godc.longtermObject.ObjectiveID, generateActivityID);
                    INitData.instance.initData.result.evaluationActivity.AddRange(activitiesList);
                }
            }
            INitData.instance.SaveChangeData();
            takeReport.CreateListSendReport1(fakeID);
        }
        takeReport.StartSendReport();

    }



    int[] CreateFakeData()
    {


        int[] fakeID = new int[2];
        int generateObjectID = UnityEngine.Random.Range(900000000, 999999999);
        while (!CheckObjectiveIdInData(generateObjectID))
        {
            generateObjectID = UnityEngine.Random.Range(900000000, 999999999);
        }
        int generateActivityID = UnityEngine.Random.Range(900000000, 999999999);
        while (!CheckACtivityIDInData(generateActivityID))
        {
            generateActivityID = UnityEngine.Random.Range(900000000, 999999999);
        }
        fakeID[0] = generateObjectID;
        fakeID[1] = generateActivityID;


        if (sendreport.ObjectiveID == 0)
        {
            if (maindata.step2.Achieve == 0)
            {
                LongTermObject ob = CreateLongterm(generateObjectID);
                List<EvaluationActivity> activitiesList = CreateActivityDetail(sendreport.rep.RepID, generateObjectID,
                    generateActivityID);
                INitData.instance.initData.result.evaluationActivity.AddRange(activitiesList);
                INitData.instance.initData.result.longTermObject.Add(ob);
            }
        }
        else
        {

            fakeID[0] = sendreport.ObjectiveID;

            if (maindata.step2.Achieve == 1)
            {
                //hoan thanh update va xoa
                INitData.instance.initData.result.longTermObject.Remove(
                    INitData.instance.initData.result.longTermObject.Find(m => m.RepID == godc.rep.RepID));
                INitData.instance.initData.result.evaluationActivity.RemoveAll(a => a.RepID == godc.rep.RepID);
            }
            else
            {
                // Debug.Log("update longterm and add new dualcall");
                List<EvaluationActivity> activitiesList = CreateActivityDetail(godc.rep.RepID, godc.longtermObject.ObjectiveID, generateActivityID);
                INitData.instance.initData.result.evaluationActivity.AddRange(activitiesList);
            }
        }
        INitData.instance.SaveChangeData();
        takeReport.CreateListSendReport(fakeID);
        return fakeID;
    }

    bool CheckObjectiveIdInData(int objectiD)
    {
        return true;
    }
    bool CheckACtivityIDInData(int activityID)
    {
        return true;
    }


    public void HideGameObject(bool isActive)
    {
        buttonHidereport[0].SetActive(isActive);
        buttonHidereport[1].SetActive(isActive);
    }


    public void SaveAfterReport()
    {
        // view last report
        INitData.instance.SaveChangeData();
        //   SaveDataTodayReport(godc.rep.RepID);
        SaveDataInput();
        isGotoday = true;
        ViewDataOnly(false);
    }

    public void SaveDataTodayReport(int _RepID, PublicClassDualCall duc = null)
    {
        if (duc == null)
        {
            duc = maindata;
        }
        string todaySave = JsonMapper.ToJson(duc);

        if (!string.IsNullOrEmpty(todaySave))
        {
            PlayerPrefs.SetString(KeySaving.MainData.ToString() + _RepID.ToString(), todaySave);
        }
    }


    public void SaveDataTodayReportOfRep(int _RepID, PublicClassDualCall duc = null)
    {
        if (duc == null)
        {
            duc = maindata;
        }
        string todaySave = JsonMapper.ToJson(maindata);

        if (!string.IsNullOrEmpty(todaySave))
        {
            PlayerPrefs.SetString(KeySaving.MainData.ToString() + _RepID.ToString(), todaySave);
        }
    }

    public void SetLastReport(int repID, string _fromday, string _today, string _longterm, string _howto)
    {
        LastReport lrp = new LastReport();
        lrp.RepID = repID;//sendreport.rep.RepID;
        lrp.fromDate = _fromday;//sendreport.step2.datefrom;
        lrp.toDate = _today;// sendreport.step2.dateto;
        lrp.objective = _longterm;// sendreport.step2.longterm;
        lrp.comment = _howto;//sendreport.step2.howto;
        string lt = JsonMapper.ToJson(lrp);

        string savetd = PlayerPrefs.GetString(godc.rep.RepID.ToString(), "");
        PlayerPrefs.SetString(KeySaving.LastReportToday.ToString(), savetd);

        PlayerPrefs.SetString(godc.rep.RepID.ToString(), lt);


    }


    #region Create data save

    public LongTermObject CreateLongterm(int idObject)
    {
        LongTermObject ob = new LongTermObject();

        ob.ObjectiveID = idObject;
        ob.AM_ID = godc.rep.AM_ID;
        ob.RepID = godc.rep.RepID;
        ob.computed = maindata.step2.longterm;
        ob.CreateDate = DateTime.Today.ToString();
        ob.UpdateDate = DateTime.Today.ToString();
        ob.FromDate = maindata.step2.datefrom;
        ob.ToDate = maindata.step2.dateto;
        ob.StatusID = maindata.step2.Achieve;
        return ob;
    }

    public List<EvaluationActivity> lt = new List<EvaluationActivity>();

    public List<EvaluationActivity> CreateActivityDetail(int repID, int ObjectiveID, int activityID)
    {

        // EvaluationActivity howto = CreateHowTo(repID, ObjectiveID, activityID);
        //EvaluationActivity objective = CreateObjectiveID(repID, ObjectiveID, activityID);
        lt[0] = CreateHowTo(repID, ObjectiveID, activityID);
        lt[1] = CreateObjectiveID(repID, ObjectiveID, activityID);
        lt[2] = CreateeVisualAid(repID, ObjectiveID, activityID);
        lt[3] = CreateInteraction(repID, ObjectiveID, activityID);
        lt[4] = CreateCatchyHook(repID, ObjectiveID, activityID);
        lt[5] = CreateDocumentused(repID, ObjectiveID, activityID);
        lt[6] = CreateCommitment(repID, ObjectiveID, activityID);
        lt[7] = CreateKeymessages(repID, ObjectiveID, activityID);
        lt[8] = CreateBrandreminder(repID, ObjectiveID, activityID);
        lt[9] = CreateQualityofNotes(repID, ObjectiveID, activityID);
        lt[10] = CreateQualityofNoteEvaluate(repID, ObjectiveID, activityID);
        lt[11] = CreateEvaluatetheoptimization(repID, ObjectiveID, activityID);
        lt[12] = CreateEvaluateknowledgeandskillslinktoproducts(repID, ObjectiveID, activityID);
        lt[13] = CreateAgreementbetween(repID, ObjectiveID, activityID);
        return lt;
    }

    private EvaluationActivity CreateHowTo(int repID, int ObjectiveID, int activityID)
    {
        EvaluationActivity objective = new EvaluationActivity();
        objective.Name = "How to achieve this long term objective?";
        objective.Score = 0;
        objective.EditDate = DateTime.Today.ToString();
        objective.RepID = repID;
        objective.TargetA = sendreport.step1.adoctor;
        objective.TargetB = sendreport.step1.bkadoctor;
        objective.TargetO = sendreport.step1.odoctor;
        objective.ObjectiveID = ObjectiveID;
        objective.EvaluationActivityID = activityID;
        objective.comment = sendreport.step2.howto;
        return objective;
    }

    private EvaluationActivity CreateObjectiveID(int repID, int ObjectiveID, int activityID)
    {
        EvaluationActivity objective = new EvaluationActivity();
        objective.Name = "Objective";
        objective.Score = sendreport.step3.objective;
        objective.EditDate = DateTime.Today.ToString();
        objective.RepID = repID;
        objective.TargetA = sendreport.step1.adoctor;
        objective.TargetB = sendreport.step1.bkadoctor;
        objective.TargetO = sendreport.step1.odoctor;
        objective.ObjectiveID = ObjectiveID;
        objective.EvaluationActivityID = activityID;
        objective.comment = "";
        return objective;
    }

    private EvaluationActivity CreateeVisualAid(int repID, int ObjectiveID, int activityID)
    {
        EvaluationActivity objective = new EvaluationActivity();
        objective.Name = "eVisualAid";
        objective.Score = sendreport.step3.EvisualAd;
        objective.EditDate = DateTime.Today.ToString();
        objective.RepID = repID;
        objective.TargetA = sendreport.step1.adoctor;
        objective.TargetB = sendreport.step1.bkadoctor;
        objective.TargetO = sendreport.step1.odoctor;
        objective.ObjectiveID = ObjectiveID;
        objective.EvaluationActivityID = activityID;
        objective.comment = "";
        return objective;
    }

    private EvaluationActivity CreateInteraction(int repID, int ObjectiveID, int activityID)
    {
        EvaluationActivity objective = new EvaluationActivity();
        objective.Name = "Interaction";
        objective.Score = sendreport.step3.Interaction;
        objective.EditDate = DateTime.Today.ToString();
        objective.RepID = repID;
        objective.TargetA = sendreport.step1.adoctor;
        objective.TargetB = sendreport.step1.bkadoctor;
        objective.TargetO = sendreport.step1.odoctor;
        objective.ObjectiveID = ObjectiveID;
        objective.EvaluationActivityID = activityID;
        objective.comment = "";
        return objective;
    }

    private EvaluationActivity CreateCatchyHook(int repID, int ObjectiveID, int activityID)
    {
        EvaluationActivity objective = new EvaluationActivity();
        objective.Name = "Catchy Hook";
        objective.Score = sendreport.step3.CatchyHook;
        objective.EditDate = DateTime.Today.ToString();
        objective.RepID = repID;
        objective.TargetA = sendreport.step1.adoctor;
        objective.TargetB = sendreport.step1.bkadoctor;
        objective.TargetO = sendreport.step1.odoctor;
        objective.ObjectiveID = ObjectiveID;
        objective.EvaluationActivityID = activityID;
        objective.comment = "";
        return objective;
    }

    private EvaluationActivity CreateDocumentused(int repID, int ObjectiveID, int activityID)
    {
        EvaluationActivity objective = new EvaluationActivity();
        objective.Name = "Document used";
        objective.Score = sendreport.step3.DocumentUsed;
        objective.EditDate = DateTime.Today.ToString();
        objective.RepID = repID;
        objective.TargetA = sendreport.step1.adoctor;
        objective.TargetB = sendreport.step1.bkadoctor;
        objective.TargetO = sendreport.step1.odoctor;
        objective.ObjectiveID = ObjectiveID;
        objective.EvaluationActivityID = activityID;
        objective.comment = "";
        return objective;
    }

    private EvaluationActivity CreateCommitment(int repID, int ObjectiveID, int activityID)
    {
        EvaluationActivity objective = new EvaluationActivity();
        objective.Name = "Commitment";
        objective.Score = sendreport.step3.Comitment;
        objective.EditDate = DateTime.Today.ToString();
        objective.RepID = repID;
        objective.TargetA = sendreport.step1.adoctor;
        objective.TargetB = sendreport.step1.bkadoctor;
        objective.TargetO = sendreport.step1.odoctor;
        objective.ObjectiveID = ObjectiveID;
        objective.EvaluationActivityID = activityID;
        objective.comment = "";
        return objective;
    }

    private EvaluationActivity CreateKeymessages(int repID, int ObjectiveID, int activityID)
    {
        EvaluationActivity objective = new EvaluationActivity();
        objective.Name = "Key messages";
        objective.Score = sendreport.step3.KeyMessage;
        objective.EditDate = DateTime.Today.ToString();
        objective.RepID = repID;
        objective.TargetA = sendreport.step1.adoctor;
        objective.TargetB = sendreport.step1.bkadoctor;
        objective.TargetO = sendreport.step1.odoctor;
        objective.ObjectiveID = ObjectiveID;
        objective.EvaluationActivityID = activityID;
        objective.comment = "";
        return objective;
    }

    private EvaluationActivity CreateBrandreminder(int repID, int ObjectiveID, int activityID)
    {
        EvaluationActivity objective = new EvaluationActivity();
        objective.Name = "Brand reminder";
        objective.Score = sendreport.step3.BrandReminder;
        objective.EditDate = DateTime.Today.ToString();
        objective.RepID = repID;
        objective.TargetA = sendreport.step1.adoctor;
        objective.TargetB = sendreport.step1.bkadoctor;
        objective.TargetO = sendreport.step1.odoctor;
        objective.ObjectiveID = ObjectiveID;
        objective.EvaluationActivityID = activityID;
        objective.comment = "";
        return objective;
    }

    private EvaluationActivity CreateQualityofNotes(int repID, int ObjectiveID, int activityID)
    {
        EvaluationActivity objective = new EvaluationActivity();
        objective.Name = "Quality of Notes";
        objective.Score = sendreport.step3.QualityOfNote;
        objective.EditDate = DateTime.Today.ToString();
        objective.RepID = repID;
        objective.TargetA = sendreport.step1.adoctor;
        objective.TargetB = sendreport.step1.bkadoctor;
        objective.TargetO = sendreport.step1.odoctor;
        objective.ObjectiveID = ObjectiveID;
        objective.EvaluationActivityID = activityID;
        objective.comment = "";
        return objective;
    }

    private EvaluationActivity CreateQualityofNoteEvaluate(int repID, int ObjectiveID, int activityID)
    {
        EvaluationActivity objective = new EvaluationActivity();
        objective.Name = "Quality of Note: Evaluate the quality of notes during the day and since the last period";
        objective.Score = 0;
        objective.EditDate = DateTime.Today.ToString();
        objective.RepID = repID;
        objective.TargetA = sendreport.step1.adoctor;
        objective.TargetB = sendreport.step1.bkadoctor;
        objective.TargetO = sendreport.step1.odoctor;
        objective.ObjectiveID = ObjectiveID;
        objective.EvaluationActivityID = activityID;
        objective.comment = sendreport.step4.QualityNoteEvaluate;
        return objective;
    }

    private EvaluationActivity CreateEvaluatetheoptimization(int repID, int ObjectiveID, int activityID)
    {
        EvaluationActivity objective = new EvaluationActivity();
        objective.Name = "Evaluate the optimization of FIC, QIC, RC & achievement of 3 products calls";
        objective.Score = 0;
        objective.EditDate = DateTime.Today.ToString();
        objective.RepID = repID;
        objective.TargetA = sendreport.step1.adoctor;
        objective.TargetB = sendreport.step1.bkadoctor;
        objective.TargetO = sendreport.step1.odoctor;
        objective.ObjectiveID = ObjectiveID;
        objective.EvaluationActivityID = activityID;
        objective.comment = sendreport.step5.Evaluatetheoptimization;
        return objective;
    }

    private EvaluationActivity CreateEvaluateknowledgeandskillslinktoproducts(int repID, int ObjectiveID, int activityID)
    {
        EvaluationActivity objective = new EvaluationActivity();
        objective.Name = "Evaluate knowledge and skills link to products";
        objective.Score = 0;
        objective.EditDate = DateTime.Today.ToString();
        objective.RepID = repID;
        objective.TargetA = sendreport.step1.adoctor;
        objective.TargetB = sendreport.step1.bkadoctor;
        objective.TargetO = sendreport.step1.odoctor;
        objective.ObjectiveID = ObjectiveID;
        objective.EvaluationActivityID = activityID;
        objective.comment = sendreport.step6.Evaluateknowledgeandskills;
        return objective;
    }

    private EvaluationActivity CreateAgreementbetween(int repID, int ObjectiveID, int activityID)
    {
        EvaluationActivity objective = new EvaluationActivity();
        objective.Name =
            "Agreement between MR/KAS and AM on strengths and area of improvement, objectives of next calls...";
        objective.Score = 0;
        objective.EditDate = DateTime.Today.ToString();
        objective.RepID = repID;
        objective.TargetA = sendreport.step1.adoctor;
        objective.TargetB = sendreport.step1.bkadoctor;
        objective.TargetO = sendreport.step1.odoctor;
        objective.ObjectiveID = ObjectiveID;
        objective.EvaluationActivityID = activityID;
        objective.comment = sendreport.step7.Agreementbetween;
        return objective;
    }

    #endregion
    public void OnSendFaild(string mes)
    {
        //   Debug.Log(mes);
        mes = "Send failed, nextime the report will be automatically send again";
        //  CreateFakeData();
        HideGameObject(true);
        PopupWaiting.SetActive(false);
        preventClick.SetActive(false);
        ShowMesage(mes);
    }



    public void ViewDataOnly(bool isDisable)
    {
        //page 1
        for (int i = 0; i < toglePeriod.Length; i++)
        {
            toglePeriod[i].interactable = isDisable;
        }
        //  p1_Rep.interactable = isDisable;
        for (int i = 0; i < doctorsVisited.Length; i++)
        {
            doctorsVisited[i].interactable = isDisable;
        }
        //page2
        for (int i = 0; i < p2_inputspage2.Length; i++)
        {
            p2_inputspage2[i].interactable = isDisable;
            p2_inputButton[i].interactable = isDisable;
        }
        for (int i = 0; i < p2_btn_datetime.Length; i++)
        {
            p2_btn_datetime[i].interactable = isDisable;
            if (!isDisable)
            {

                Color temp = p2_btn_datetime[i].image.color;
                temp.a = .3f;
                p2_btn_datetime[i].image.color = temp;
            }
            //  p2_btn_datetime[i].GetComponent<Image>().color= Color.black;
        }
        //page3
        for (int i = 0; i < p3_inputspage3.Length; i++)
        {
            p3_inputspage3[i].interactable = isDisable;
        }
        for (int i = 0; i < dropdownPage3.Length; i++)
        {
            dropdownPage3[i].interactable = isDisable;
        }
        //page4

        evaluaDateOfnotes.interactable = isDisable;
        drQUalityOfNotes.interactable = isDisable;
        evaluateCommentAgreement.interactable = isDisable;
        evaluaateQualityofVisit.interactable = isDisable;
        evaluateProductExpertise.interactable = isDisable;
        //overview



        // isDisable = true; // nho xoa dong nay

        for (int i = 0; i < btnOverview.Length; i++)
        {

            btnOverview[i].interactable = isDisable;
            if (!isDisable)
            {

                Color temp = btnOverview[i].image.color;
                temp.a = 1f;
                btnOverview[i].image.color = temp;
            }
            else
            {
                Color temp = btnOverview[i].image.color;
                temp.a = 0f;
                btnOverview[i].image.color = temp;
            }



        }
    }


    private bool isDisplayExpire = false;

    public GameObject ConfirmExit;

    public void SendNo()
    {
        ConfirmSend.SetActive(false);

    }

    public void ExitYes()
    {
        ConfirmExit.SetActive(false);
        //check save data if not save
        PlayerPrefs.SetString(KeySaving.MainData.ToString(), "");
        //     Application.LoadLevel(SceneName.Main.ToString());
        SceneManager.LoadScene(SceneName.Main.ToString());
    }

    public void ExitNo()
    {
        ConfirmExit.SetActive(false);
    }


    #endregion


    #region page1 overview

    private int period;

    public void OnTogleFulldayChange(bool ischange)
    {
        if (ischange)
        {
            //  if (!isresest)
            maindata.step1.period = 0;
            SaveDataInput();
        }
    }

    public void OnTogleAMdayChange(bool ischange)
    {
        if (ischange)
        {
            maindata.step1.period = 1;
            SaveDataInput();
        }
    }

    public void OnToglePMdayChange(bool ischange)
    {
        if (ischange)
        {
            maindata.step1.period = 2;
            SaveDataInput();
        }
    }

    public void OnTogleDefault(bool ischange)
    {
        if (ischange)
        {

            maindata.step1.period = -1;
            SaveDataInput();
        }
    }

    #endregion

    #region page2

    public ScrollRect scroll;

    public void OnViewLastReport()
    {
        ViewLastReport();
        scroll.enabled = true;
        scroll.verticalScrollbar.value = .5f;
    }

    public GameObject LineAll;
    public GameObject ChartLine;

    void DrawChartOverviewPage2Line()
    {

        foreach (Transform child in RootChart.transform)
        {
            DestroyImmediate(child.gameObject);
        }
        foreach (Transform child in RootLine.transform)
        {
            DestroyImmediate(child.gameObject);
        }
        maindata.Line1 = interactionChart;
        maindata.Line2 = commitmentChart;
        maindata.Line3 = qualityOfNotesChart;

        Drawline(interactionChart, Point[0], Line[0], RootChart, RootLine, 10); //interaction chart
        Drawline(commitmentChart, Point[1], Line[1], RootChart, RootLine, 10); //commitmentChart
        Drawline(qualityOfNotesChart, Point[2], Line[2], RootChart, RootLine, 10); //qualityofnotes chart
    }

    public OneTimeDualCall CreateTodayDuacall()
    {
        OneTimeDualCall todayDuacall = new OneTimeDualCall();
        todayDuacall.EvaluationActivity = new List<EvaluationActivity>();
        for (int i = 0; i < 14; i++)
        {
            EvaluationActivity evaluate = new EvaluationActivity();
            evaluate.RepID = godc.rep.RepID;
            evaluate.Score = 0;
            evaluate.EditDate = DateTime.Today.ToString("MMM dd yyyy");
            evaluate.TargetA = maindata.step1.adoctor;
            evaluate.TargetB = maindata.step1.bkadoctor;
            evaluate.TargetO = maindata.step1.odoctor;

            todayDuacall.EvaluationActivity.Add(evaluate);
        }

        todayDuacall.EvaluationActivity[1].Score = (int)maindata.step3.dataChar[0];
        todayDuacall.EvaluationActivity[4].Score = (int)maindata.step3.dataChar[1];//evisualad
        todayDuacall.EvaluationActivity[7].Score = (int)maindata.step3.dataChar[2];
        todayDuacall.EvaluationActivity[2].Score = (int)maindata.step3.dataChar[3];
        todayDuacall.EvaluationActivity[5].Score = (int)maindata.step3.dataChar[4];
        todayDuacall.EvaluationActivity[8].Score = (int)maindata.step3.dataChar[5];
        todayDuacall.EvaluationActivity[3].Score = (int)maindata.step3.dataChar[6];
        todayDuacall.EvaluationActivity[6].Score = (int)maindata.step3.dataChar[7];
        todayDuacall.EvaluationActivity[9].Score = (int)maindata.step3.dataChar[8];
        return todayDuacall;

    }
    public List<OneTimeDualCall> Get9DualCall()
    {
        List<OneTimeDualCall> lt = new List<OneTimeDualCall>();


        int getfrom = godc.dualCalllists.Count - 8;
        int getto = 8;
        if (getfrom > 0)
        {
            getfrom = godc.dualCalllists.Count - 8;
            getto = 8;
        }
        else
        {
            getfrom = 0;
            getto = godc.dualCalllists.Count;
        }
        lt.AddRange(godc.dualCalllists.GetRange(getfrom, getto));
        if (!isGotoday)
        {
            OneTimeDualCall todayDuacall = CreateTodayDuacall();
            lt.Add(todayDuacall);
        }

        return lt;

    }

    private int[] interactionChart;
    private int[] commitmentChart;
    private int[] qualityOfNotesChart;
    public string[] dateDualcallChart;


    public void caculateDataChartLine()
    {
        List<OneTimeDualCall> lt = Get9DualCall();

        int charNum = lt.Count;
        interactionChart = new int[charNum];
        commitmentChart = new int[charNum];
        qualityOfNotesChart = new int[charNum];
        dateDualcallChart = new string[charNum];
        for (int i = 0; i < lt.Count; i++)
        {
            interactionChart[i] = lt[i].EvaluationActivity[3].Score;
            commitmentChart[i] = lt[i].EvaluationActivity[6].Score;
            qualityOfNotesChart[i] = lt[i].EvaluationActivity[9].Score;
            dateDualcallChart[i] = lt[i].EvaluationActivity[3].EditDate;
        }
        maindata.dateDualcallChart = dateDualcallChart;
        DrawChartOverviewPage2Line();
    }


    public OneTimeDualCall GetViewLastReport()
    {

        OneTimeDualCall lt = new OneTimeDualCall();
        int getfrom = godc.dualCalllists.Count;

        if (getfrom > 0)
        {
            getfrom = godc.dualCalllists.Count - 1;
            lt = godc.dualCalllists[getfrom];
            return lt;
        }
        return null;

    }


    public void ViewLastReport()
    {
        string todayLastReport = PlayerPrefs.GetString(KeySaving.LastReportToday.ToString(), "");
        string lastRP = "";
        if (!string.IsNullOrEmpty(todayLastReport))
        {
            lastRP = todayLastReport;
        }
        else
        {
            lastRP = PlayerPrefs.GetString(godc.rep.RepID.ToString());
        }

        if (!string.IsNullOrEmpty(lastRP))
        {

            LastReport lrp = JsonMapper.ToObject<LastReport>(lastRP);
            viewLastReport[0].text = lrp.fromDate;
            viewLastReport[1].text = lrp.toDate;
            viewLastReport[2].text = lrp.objective;
            viewLastReport[3].text = lrp.comment;

        }
        else
        {
            viewLastReport[0].text = "";
            viewLastReport[1].text = "";
            viewLastReport[2].text = "";
            viewLastReport[3].text = "";
        }


    }

    private float[] chartEverage = new float[9];
    public void GetChartAverage()
    {
        chartEverage = new float[9];
        List<OneTimeDualCall> lt = new List<OneTimeDualCall>();

        lt = godc.dualCalllists;//caculate number of everage


        OneTimeDualCall todayDuacall = new OneTimeDualCall();
        todayDuacall.EvaluationActivity = new List<EvaluationActivity>();
        for (int i = 0; i < 14; i++)
        {
            EvaluationActivity evaluate = new EvaluationActivity();
            evaluate.RepID = godc.rep.RepID;
            evaluate.Score = 0;
            evaluate.EditDate = DateTime.Today.ToString();
            evaluate.TargetA = maindata.step1.adoctor;
            evaluate.TargetB = maindata.step1.bkadoctor;
            evaluate.TargetO = maindata.step1.odoctor;
            evaluate.EditDate = DateTime.Today.ToString();
            evaluate.comment = "";
            todayDuacall.EvaluationActivity.Add(evaluate);
        }

        todayDuacall.EvaluationActivity[1].Score = (int)maindata.step3.dataChar[0];
        todayDuacall.EvaluationActivity[2].Score = (int)maindata.step3.dataChar[3];
        todayDuacall.EvaluationActivity[3].Score = (int)maindata.step3.dataChar[6];
        todayDuacall.EvaluationActivity[4].Score = (int)maindata.step3.dataChar[1];
        todayDuacall.EvaluationActivity[5].Score = (int)maindata.step3.dataChar[4];
        todayDuacall.EvaluationActivity[6].Score = (int)maindata.step3.dataChar[7];
        todayDuacall.EvaluationActivity[7].Score = (int)maindata.step3.dataChar[2];
        todayDuacall.EvaluationActivity[8].Score = (int)maindata.step3.dataChar[5];
        todayDuacall.EvaluationActivity[9].Score = (int)maindata.step3.dataChar[8];
        lt.Add(todayDuacall);
        int total = lt.Count;
        for (int i = 0; i < total; i++)
        {
            chartEverage[0] += lt[i].EvaluationActivity[1].Score; //objective 
            chartEverage[1] += lt[i].EvaluationActivity[4].Score;//evisualaid  //catchy hook
            chartEverage[2] += lt[i].EvaluationActivity[7].Score;//interaction //key mesage
            chartEverage[3] += lt[i].EvaluationActivity[2].Score;//catchyhook  //evisualads
            chartEverage[4] += lt[i].EvaluationActivity[5].Score;//documentused  //document use
            chartEverage[5] += lt[i].EvaluationActivity[8].Score;//commitment    //brand remider
            chartEverage[6] += lt[i].EvaluationActivity[3].Score;//keys message //interaction
            chartEverage[7] += lt[i].EvaluationActivity[6].Score;//brand remider  //commitment
            chartEverage[8] += lt[i].EvaluationActivity[9].Score;//quality of note  //quality
        }
        for (int i = 0; i < chartEverage.Length; i++)
        {
            chartEverage[i] = (float)chartEverage[i] / (total);
        }
        if (!isGotoday || !isNotSendYet)
            maindata.average = chartEverage;
        lt.Remove(todayDuacall);

    }


    private float[] fistDualCallChart = new float[9];
    private float[] lastDualCallChart = new float[9];
    public void GetFirstDualCallChart()
    {
        if (godc.dualCalllists.Count > 0)
        {
            fistDualCallChart = new float[9];
            OneTimeDualCall lt = godc.dualCalllists.First();//caculate number of everage

            if (lt != null)
            {

                fistDualCallChart[0] += lt.EvaluationActivity[1].Score; //objective
                fistDualCallChart[1] += lt.EvaluationActivity[2].Score;//evisualaid
                fistDualCallChart[2] += lt.EvaluationActivity[3].Score;//interaction
                fistDualCallChart[3] += lt.EvaluationActivity[4].Score;//catchyhook
                fistDualCallChart[4] += lt.EvaluationActivity[5].Score;//documentused
                fistDualCallChart[5] += lt.EvaluationActivity[6].Score;//commitment
                fistDualCallChart[6] += lt.EvaluationActivity[7].Score;//keys message
                fistDualCallChart[7] += lt.EvaluationActivity[8].Score;//brand remider
                fistDualCallChart[8] += lt.EvaluationActivity[9].Score;//quality of note
                lastDualCallChart = Datachar.data;
            }


        }
        else
        {
            // maindata.firstDualcall = Datachar.data;
            fistDualCallChart = Datachar.data;
        }


    }
    #endregion



}




[System.Serializable]
public class SaveDataFake
{
    public int status;
    public int repID;
    public DateTime dateCreateFake;
    public LongTermObject longterm;
    public List<EvaluationActivity> listActive;
}


