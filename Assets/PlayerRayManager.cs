using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRayManager : MonoBehaviour
{
    List<IClick> clickOb = new List<IClick>();
    // Start is called before the first frame update

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // �������ͨ�������λ�ô�������
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // ��������Ƿ��������ཻ
            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;

                var ob = clickedObject.GetComponent<IClick>();
                if (ob == null)
                    return;
                else
                {
                    
                    clickOb.Add(ob);
                    ob.Click();
                }
            }
        }
        if(Input.GetMouseButtonUp(0))
        {

            foreach(var  ob in clickOb)
            {
                ob.CancleClick();
            }
            clickOb.Clear();
        }
    }
}
