using UnityEngine;
using System.Collections;
using System.Threading;

public class TrungTest : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Send();

    }

  
    int count = 0;
    void Send()
    {
        isSending = false;
        count = 0;


        StartCoroutine(SendReportOffline());



    }

    private bool isSending = false;
    IEnumerator SendReportOffline()
    {
        for (int i = 0; i < 3; i++)
        {
            Debug.Log("send");
            isSending = true;
            //WWWForm form = new WWWForm();
            //string dt = 1;
            //form.AddField("data", dt);
            //WWW httpResponse = new WWW("http://vn1ln01.int.grs.net/api/user/save-report", form);

            yield return  new WaitForSeconds(5);
            yield return new WaitUntil(() => (isWait == false));
            isSending = false;
          
        }
     
    }

   
    private bool isWait;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            isWait = true;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            isWait = false;
        }
    }

    public GameObject can;
    void DisPlayCanvas()
    {
        can.SetActive(true);
    }
    void HideCanvas()
    {
        can.SetActive(false);
    }
}
