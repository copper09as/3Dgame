using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SliderBar : MonoBehaviour
{
    int time = 0;
    [SerializeField] Image BGImage;
    [SerializeField] Image StatuImage;
    
    [SerializeField] TMP_Text processText;
    private float width;
    private float height;

    public void UpdateSliderBar(float maxStatu, float currentStatu, bool showProcess = false)
    {
        if (time == 0)
        {
            width = GetComponent<RectTransform>().sizeDelta.x;
            height = GetComponent<RectTransform>().sizeDelta.y;
            time +=1;
        }
        float targetWidth = Mathf.Min(width * currentStatu / maxStatu, width);
        StatuImage.GetComponent<RectTransform>().sizeDelta = new Vector2(targetWidth, height);

        if (showProcess && processText != null)
        {
            processText.gameObject.SetActive(true);
            processText.text = (currentStatu / maxStatu * 100).ToString("0.0") + "%";
        }
        else if (processText != null)
        {
            processText.gameObject.SetActive(false);
        }
    }

}

