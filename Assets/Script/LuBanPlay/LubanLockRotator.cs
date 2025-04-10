using UnityEngine;

public class LubanLockRotator : MonoBehaviour
{
    public float rotationSpeed = 200f;

    private bool isRotating = false;
    private Vector3 lastMousePosition;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isRotating = true;
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isRotating = false;
        }

        if (isRotating)
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            float rotX = delta.y * rotationSpeed * Time.deltaTime;
            float rotY = -delta.x * rotationSpeed * Time.deltaTime;

            // Ðý×ª×ÔÉí
            transform.Rotate(Vector3.right, rotX, Space.World);
            transform.Rotate(Vector3.up, rotY, Space.World);

            lastMousePosition = Input.mousePosition;
        }
    }
}
