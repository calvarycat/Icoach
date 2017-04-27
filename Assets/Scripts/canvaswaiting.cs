using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class canvaswaiting : MonoBehaviour
{
    public ControlPage control;
    public Image imgWaiting;
    private int amount = 1;
    public float timeOut = 90;
    public GameObject preventClick;

    public void OnEnable()
    {
        timeOut = 90;
    }

    public void OnDisable()
    {
        if (preventClick)
            preventClick.SetActive(false);
        imgWaiting.fillAmount = 1;
    }

    // Update is called once per frame

    void Update()
    {
        timeOut -= Time.deltaTime;
        if (timeOut <= 0)
        {
          //  Debug.Log("return");
            gameObject.SetActive(false);
            if (preventClick)
                preventClick.SetActive(false);
            return;
        }
        if (imgWaiting.fillAmount > 0)
        {
            imgWaiting.fillAmount -= Time.deltaTime * .2f;
        }
        else
        {
            imgWaiting.fillAmount = 1;
        }
    }
}
