using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum KeySaving
{
    Islogin,
    ValueAM,
    Login,
    DataInit,

    MainData, //main dualcall class data + repID cho ra ngay hom nay rep da di chua?

    DataRepNotSent,

    CatchToday, // store to check today send or not?
    ResultDateOfNote, //14 days of note
   LastReportToday,

    GoduacallToday,
    ControlLoadata,
}
public enum SceneName
{
    Login,
    Loading,
    Main,
}




public class ResultRep
{
    public int success = 1;
    public int error = 0;
    public string msg = "";
    public List<DataRep> result;

}
public class DataRep
{
    public int repId;
    public string repName;
}
