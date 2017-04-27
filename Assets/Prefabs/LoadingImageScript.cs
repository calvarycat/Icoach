using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingImageScript : MonoBehaviour
{

    public Image imgLoad;
    // Use this for initialization

    // Update is called once per frame
    void Update()
    {

        imgLoad.fillAmount += Time.deltaTime * 0.5f;
        if (imgLoad.fillAmount == 1)
        {
            imgLoad.fillAmount = 0;
        }


    }
}
