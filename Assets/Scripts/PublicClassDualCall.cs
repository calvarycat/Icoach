using UnityEngine;
using System.Collections;
using System;
[System.Serializable]
public class PublicClassDualCall
{
   public LongTermObject longterm = new LongTermObject();
    public string dateSendReport;
    public Step1 step1 = new Step1();
    public Step2 step2 = new Step2();
    public Step3 step3 = new Step3();
    public Step4 step4 = new Step4();
    public Step5 step5 = new Step5();
    public Step6 step6 = new Step6();
    public Step7 step7 = new Step7();
    
    public float[] average ;
    public float[] firstDualcall ;
    public float[] lastDualCall;
    public int[] Line1 ;
    public int[] Line2 ;
    public int[] Line3 ;
    public string[] dateDualcallChart;

}
[System.Serializable]
public class Step1
{
    public int period = -1; //0 ==full, 1AM, 2PM
    public int idMember;
    public string memberName;
    public int adoctor=-1;
    public int bkadoctor=-1;
    public int odoctor=-1;

    public void Clean()
    {
        period = -1; //0 ==full, 1AM, 2PM
        idMember = 0;
        memberName = "";
        adoctor = -1;
        bkadoctor = -1;
        odoctor = -1;
    }

    public int CaculateTotalDoctors()
    {
        int a, b, c;
        a = adoctor;
        b = bkadoctor;
        c = odoctor;
        if (a < 0)
        {
            a = 0;
        }
        if (b < 0)
        {
            b = 0;
        }
        if (c < 0)
        {
            c = 0;
        }
        return a + b + c;
        //return adoctor + bkadoctor + odoctor;
    }

   
    public bool CheckData()
    {
        if (adoctor == 0 || bkadoctor == 0 || idMember == 0 || odoctor == 0 || period == -1)
            return false;
        return true;
    }
}
[System.Serializable]
public class Step2
{
    public string datefrom;
    public string dateto;
    public int Achieve;
    public string longterm = "";
    public string howto = "";
    public string lastReport = "";

    public void Clean()
    {
        datefrom = DateTime.Today.ToString("dd-MMM-yyyy");
        dateto = DateTime.Today.ToString("dd-MMM-yyyy");
        Achieve = 0;
        longterm = "";
        howto = "";

    }
    public bool ValidateDaytime()
    {
        DateTime from;
        DateTime to;
        IFormatProvider culture = new System.Globalization.CultureInfo("fr-FR", true);
        if (string.IsNullOrEmpty(datefrom))
        {
            return false;
        }
        if (string.IsNullOrEmpty(dateto))
        {
            return false;
        }
        from = DateTime.Parse(datefrom, culture);
        to = DateTime.Parse(dateto, culture);
        return from > to ? false : true;
    }

    public bool CheckData()
    {
        if (string.IsNullOrEmpty(longterm) || string.IsNullOrEmpty(howto))
            return false;
        return true;

    }
}
[System.Serializable]
public class Step3
{
    public float[] dataChar = new float[9];
    public void Clean()
    {
        for (int i = 0; i < dataChar.Length; i++)
        {
            dataChar[i] = 0;
        }
    }
    public bool CheckData()
    {
        for (int i = 0; i < dataChar.Length - 1; i++)
        {
            if (dataChar[i] == 0)
            {
                return false;
            }

        }
        return true;
    }
}
[System.Serializable]
public class Step4
{
    public int valueChar;
    public string evaluaateQualityofNotes;
    public void Clean()
    {

        valueChar = 0;
        evaluaateQualityofNotes = "";
    }

    public bool CheckData()
    {
        if (string.IsNullOrEmpty(evaluaateQualityofNotes))
            return false;
        return true;
    }
}
[System.Serializable]
public class Step5
{

    public string evaluaateQualityofVisit;
    public void Clean()
    {
        evaluaateQualityofVisit = "";
    }
    public bool CheckData()
    {
        if (string.IsNullOrEmpty(evaluaateQualityofVisit))
            return false;
        return true;
    }
}
[System.Serializable]
public class Step6
{
   
    public string evaluateProductExpertise;
    public void Clean()
    {
        evaluateProductExpertise = "";
    }
    public bool CheckData()
    {
        if (string.IsNullOrEmpty(evaluateProductExpertise))
            return false;
        return true;
    }
}
[System.Serializable]
public class Step7
{
 
    public string evaluateCommentAgreement;
    public void Clean()
    {
        evaluateCommentAgreement = "";
    }
    public bool CheckData()
    {
        if (string.IsNullOrEmpty(evaluateCommentAgreement))
            return false;
        return true;
    }
}

public class QualityOfNotes
{
    public int id;
    public string product;
    public string doctorPharmacists;
    public DateTime date;
    public string comment;

}