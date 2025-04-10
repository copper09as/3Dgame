using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyObject : MonoBehaviour,IClick
{
    [SerializeField] private bool InClick = false;
    [SerializeField] private Vector3 initPosition;
    private Collider objectCollider;
    public float heavy;
    private void Start()
    {
        initPosition = transform.localPosition;
        objectCollider = GetComponent<Collider>();
    }

    public void CancleClick()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        objectCollider.enabled = false;
        // 检测射线是否与物体相交
        if (Physics.Raycast(ray, out hit))
        {
            GameObject clickedObject = hit.collider.gameObject;

            var ob = clickedObject.GetComponent<SteelPlate>();
            if (ob == null)
                transform.position = initPosition;
            else
            {
                ob.heavyObjects.Add(this);
                ob.AddHeavy(heavy);
            }
        }
        InClick = false;
        objectCollider.enabled = true;

    }

    public void Click()
    {
        InClick = true;
    }
    private void Update()
    {
        if (InClick)
        {

            Vector3 mouseScreenPosition = Input.mousePosition;

            Vector3 mouseWorldPosition =
                Camera.main.ScreenToWorldPoint
                (new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, 0));

            transform.position = new Vector3(mouseWorldPosition.x, transform.position.y, transform.position.z);
        }
    }
}
