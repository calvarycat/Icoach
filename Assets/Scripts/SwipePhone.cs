using UnityEngine;
using System.Collections;

public class SwipePhone : MonoBehaviour
{
    public ControlPage mainData;
    void Update()
    {
        Swipe();
    }
    private float wipeResistanceX = 200;
    private float wipeResistanceY = 200;
    private Vector3 touchPosition;
    private SwipeDirection swipeDirection;
    public GameObject panelWaiting;
    public void Swipe()
    {
        if (!mainData.PopupWaiting.activeSelf && !mainData.ConfirmSend.activeSelf && !mainData.ConfirmExit.activeSelf)
        {
           // Debug.Log("Co chay dau?");
            if (Input.GetMouseButtonDown(0))
            {
                touchPosition = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0) && !mainData.ConfirmSend.activeSelf && !mainData.panelCheckVPN.activeSelf 
                && !mainData.PopupWaiting.activeSelf && !panelWaiting.activeSelf && !mainData.showMessagePanel.activeSelf)
            {
                //panelCheckVPN.SetActive(false);
                //if (!ConfirmSend.activeSelf)
                    Vector2 deltaSwipe = touchPosition - Input.mousePosition;
                if (Mathf.Abs(deltaSwipe.x) > wipeResistanceX)
                {
                    //swipe in the x axis
                    if (deltaSwipe.x < 0)
                    {
                        mainData.OnButtonPageOverViewClick(0);
                    }
                    else
                    {
                        mainData.OnButtonPageOverViewClick(1);
                    }
                }
            }
        }
       
    }
}
