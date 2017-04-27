using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Data : MonoBehaviour
{
    public float[] data;
    public Transform ChartRoot;
    public InputField[] inputsData;
    public Text[] textDataLable;
    public Text[] textNumbers;
    public float ratio = 1;

    void OnEnable()
    {
        RefreshData();
        //for (int i = 0; i < inputsData.Length; i++)
        //{
        //    inputsData[i].text = data[i].ToString();
        //}
    }
    public void RefreshData()
    {
        CaculateRatio();
        CaculateTextNumber();
        for (int i = 0; i < data.Length; i++)
        {
            ChartRoot.GetChild(i).localScale = new Vector3(1, data[i] / ratio, 1);
            if (data[i] > 0)
            {
                textDataLable[i].text = data[i].ToString();
            }
            else
            {
                textDataLable[i].text = "";
            }
           
            if (data[i] > 0)
            {
             Vector2 vt=   new Vector2(ChartRoot.GetChild(i).GetComponent<RectTransform>().anchoredPosition.x,
                       45 * (data[i]) / ratio - 45);
                ChartRoot.GetChild(i).GetComponent<RectTransform>().anchoredPosition =
                   new Vector2(ChartRoot.GetChild(i).GetComponent<RectTransform>().anchoredPosition.x,
                      45 * (data[i]) / ratio - 45);
                textDataLable[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(vt.x, textNumbers[(int)data[i]].GetComponent<RectTransform>().anchoredPosition.y);
            }
               
        }
    }
    int max = 0;
    void CaculateRatio()
    {
        max = 0;
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i] > max)
                max = (int)data[i];
        }
        if(max>10)
        ratio = max / 10;
    }

    public void CaculateTextNumber()
    {
        for (int i = 0; i < textNumbers.Length; i++)
        {
         textNumbers[i].text = (i * max / 6).ToString();
        }
    }
    void Start()
    {
        RefreshData();
    }

    public void InputData(int id)
    {
       
        int con= int.Parse(inputsData[id].text);
       
        if (con > 6)
        {
            con = 6;
            inputsData[id].text = "6";
        }
        data[id] = con;
        RefreshData();
    }


}
