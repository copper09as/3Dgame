// RotateLock.cs
using UnityEngine;

public class RotateLock : MonoBehaviour
{
    public float rotationSpeed = 5f;
    private Vector3 lastMousePos;

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // ÓÒ¼ü°´ÏÂ
        {
            lastMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            transform.Rotate(Vector3.up, delta.x * rotationSpeed * Time.deltaTime, Space.World);
            transform.Rotate(Vector3.right, -delta.y * rotationSpeed * Time.deltaTime, Space.World);
            lastMousePos = Input.mousePosition;
        }
    }
}