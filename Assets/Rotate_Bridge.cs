using UnityEngine;

public class WaterWheelRotation : MonoBehaviour
{
    // ����ת���ٶȵı���
    public float rotationSpeed = 10f; // ��ת�ٶȣ���ֵ˳ʱ�룬��ֵ��ʱ��

    void Update()
    {
        // ÿ֡��ָ�����ٶ���X����ת
        transform.Rotate(rotationSpeed * Time.deltaTime, 0f, 0f);
    }
}
