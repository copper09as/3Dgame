using UnityEngine;

public class LubanRotator : MonoBehaviour
{
    public float rotateSpeed = 5f;
    public float resetSpeed = 3f;

    private bool isRotating = false;
    private Vector3 lastMousePos;
    private Quaternion targetRotation;
    private bool isShowcasing = false;
    

    void Start()
    {
        targetRotation = transform.rotation;
    }

    void Update()
    {
        if (isShowcasing)
        {
            transform.Rotate(Vector3.up, 20f * Time.deltaTime, Space.World);
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            isRotating = true;
            lastMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isRotating = false;
            targetRotation = Quaternion.identity;
        }

        if (isRotating)
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            float rotX = -delta.y * rotateSpeed * Time.deltaTime;
            float rotY = delta.x * rotateSpeed * Time.deltaTime;

            transform.Rotate(Camera.main.transform.up, rotY, Space.World);
            transform.Rotate(Camera.main.transform.right, rotX, Space.World);

            lastMousePos = Input.mousePosition;
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * resetSpeed);
        }
    }

    public void StartShowcaseRotation()
    {
        isShowcasing = true;
    }

    
}