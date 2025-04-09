using UnityEngine;

public class WaterWheelRotation : MonoBehaviour
{
    // 控制转动速度的变量
    public float rotationSpeed = 10f; // 旋转速度，正值顺时针，负值逆时针

    void Update()
    {
        // 每帧以指定的速度绕X轴旋转
        transform.Rotate(rotationSpeed * Time.deltaTime, 0f, 0f);
    }
}
