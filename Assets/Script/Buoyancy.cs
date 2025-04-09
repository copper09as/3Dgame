using UnityEngine;

public class Buoyancy : MonoBehaviour
{
    public float floatHeight = 0.3f;  // ���帡���ĸ߶ȣ�΢С���������
    public float floatSpeed = 0.5f;   // �������ٶȣ����������
    public float waterLevelOffset = 0.1f;  // ˮ��ƫ���������ڱ���������ˮ���Ϸ�

    private bool isInWater = false;   // �����Ƿ����ˮ��
    private Rigidbody rb;             // ����ĸ���

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // ��ȡ����ĸ������
        rb.useGravity = true;            // Ĭ����������
    }

    void Update()
    {
        if (isInWater)
        {
            // ͣ�������Ա����������µ�
            rb.useGravity = false;

            // ���㸡����Ŀ��λ�ã���΢���������
            float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
            Vector3 targetPosition = new Vector3(transform.position.x, waterLevelOffset + yOffset, transform.position.z);

            // ʹ�ø���� MovePosition ��ƽ���ƶ����壬����������ˮ���Ϸ�
            rb.MovePosition(targetPosition);
        }
        else
        {
            // �ָ����������
            rb.useGravity = true;
        }
    }

    // ���������ˮ��ʱ����
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))  // ˮ���ǩΪ "Water"
        {
            isInWater = true;  // ����������ˮ��
        }
    }

    // �����������ˮ��ʱ����
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = true;  // ��������ˮ���ϣ���������
        }
    }

    // �������뿪ˮ��ʱ����
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = false;  // ��������뿪ˮ��
        }
    }
}
