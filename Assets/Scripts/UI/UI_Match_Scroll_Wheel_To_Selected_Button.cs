using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Match_Scroll_Wheel_To_Selected_Button : MonoBehaviour
{
    [SerializeField] private GameObject currentSelected;
    [SerializeField] private GameObject previousSelected;
    [SerializeField] RectTransform currentSelectedTransform;
    
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] ScrollRect scrollRect;

    private void Update()
    {
        currentSelected = EventSystem.current.currentSelectedGameObject;

        if (currentSelected != null)
        {
            if (currentSelected != previousSelected)
            {
                previousSelected = currentSelected;
                currentSelectedTransform = currentSelected.GetComponent<RectTransform>();
                SnapTo(currentSelectedTransform);
            }
        }
    }

    private void SnapTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();
        
        Vector2 newPosition = 
            (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position) - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
        
        // we only want to lock the position on the Y axis (up/down)
        newPosition.x = 0;
        
        contentPanel.anchoredPosition  = newPosition;
    }
}
