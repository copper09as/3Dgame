using UnityEngine;

public class HeavyInSteel : MonoBehaviour, IClick
{
    [SerializeField] private bool InClick = false;
    [SerializeField] private Transform left;
    [SerializeField] private Transform right;
    public float heavy;
    
    
    public void CancleClick()
    {
        InClick = false;
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

            float clampedX = Mathf.Clamp(mouseWorldPosition.x, left.position.x, right.position.x);
            
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
        }
    }
}
