using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_StatBar : MonoBehaviour
{
    private Slider slider;
    private RectTransform rectTransform;
    
    // variable to scale bar size depending on stat (higher stat = longer bar)
    [Header("Bar Options")] 
    [SerializeField] protected bool scaleBarLengthWithStats = true;
    [SerializeField] protected float widthScaleMultiplier = 1;
    // secondary bar behind the bar for polish effect (yellow bar that shows how much an action/damage takes away from current stat)

    protected virtual void Awake()
    {
        slider = GetComponent<Slider>();
        rectTransform = GetComponent<RectTransform>();
    }

    public virtual void SetStat(int newValue)
    {
        slider.value = newValue;
    }
    
    public virtual void SetMaxStat(int maxValue)
    {
        slider.maxValue = maxValue;
        slider.value = slider.maxValue;

        if (scaleBarLengthWithStats)
        {
            // scale the transform of the bar
            rectTransform.sizeDelta = new Vector2(maxValue * widthScaleMultiplier, rectTransform.sizeDelta.y);
            // resets the position of the bars based on their layout group's settings
            PlayerUIManager.instance.playerUIHudManager.RefreshHUD();
        }
    }
}
