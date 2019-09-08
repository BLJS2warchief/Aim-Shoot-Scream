using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour
{
    [SerializeField]
    private RectTransform healthBarRect;

    [SerializeField]
    private Text healthBarText;

    void Start()
    {
        if(healthBarRect == null)
        {
            Debug.Log("STATUS : NO healthbar object referenced");
        }
        if (healthBarText == null)
        {
            Debug.Log("STATUS : NO healthText object referenced");
        }
    }

    public void SetHealth(int cur, int max)
    {
        float value = (float) cur / max;
        healthBarRect.localScale = new Vector3(value, healthBarRect.localScale.y, healthBarRect.localScale.z);
        healthBarText.text = cur + " / " + max + " HP";

    }
}
