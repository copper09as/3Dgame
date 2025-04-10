using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private HeavyInSteel heavy1;
    [SerializeField] private HeavyInSteel heavy2;
    [SerializeField] private SteelPlate plate;
    [SerializeField] private Transform Center;
    [SerializeField] private float heavy = 9.8f;
    [SerializeField] private float speed = 1f;
    private float currentXRotation = 0;
    float heavy1Heave;
    float heavy2Heave;
    float plateHeave;
    private void Start()
    {
        currentXRotation = transform.eulerAngles.x;
    }
    private void Update()
    {
        heavy1Heave = (heavy1.transform.localPosition.x - Center.localPosition.x) * heavy1.heavy;
        heavy2Heave = (heavy2.transform.localPosition.x - Center.localPosition.x) * heavy2.heavy;
        plateHeave = (plate.transform.localPosition.x - 1.243f-Center.localPosition.x) * plate.Heavy;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.LogWarning(heavy1Heave);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.LogWarning(heavy2Heave);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.LogWarning(plateHeave);
        }

        if (Mathf.Abs(Mathf.Abs(plateHeave)-Mathf.Abs(heavy1Heave)-Mathf.Abs(heavy2Heave))<0.01f)
        {
            Debug.Log("Success");
        }

    }
}