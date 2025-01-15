using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerUIPopUpManager : MonoBehaviour
{
    [Header("You Died Pop Up")] 
    [SerializeField] GameObject youDiedPopUpGameObject;
    [SerializeField] TextMeshProUGUI youDiedPopUpBackgroundText;
    [SerializeField] private TextMeshProUGUI youDiedPopUpText;
    [SerializeField] private CanvasGroup youDiedPopUpCanvasGroup; // allow us to set the alpha to fade over time

    public void SendYouDiedPopUp()
    {
        // activate post-processing effects
        
        youDiedPopUpGameObject.SetActive(true);
        youDiedPopUpBackgroundText.characterSpacing = 0;
        // stretch out the pop up
        StartCoroutine(StretchPopUpTextOverTime(youDiedPopUpBackgroundText, 8, 19));
        // fade in the pop up
        StartCoroutine(FadeInPopUpOverTime(youDiedPopUpCanvasGroup, 5));
        // wait, then fade out the pop up
        StartCoroutine(WaitThenFadeOutPopUpOverTime(youDiedPopUpCanvasGroup, 2, 5));
    }

    private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount)
    {
        if (duration > 0)
        {
            text.characterSpacing = 0; // resets our character spacing
            float timer = 0;
            yield return null;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount , duration * Time.deltaTime / 20);
                yield return null;
            }
        }
    }

    private IEnumerator FadeInPopUpOverTime(CanvasGroup canvas, float duration)
    {
        if (duration > 0)
        {
            canvas.alpha = 0;
            float timer = 0;
            
            yield return null;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                canvas.alpha = Mathf.Lerp(canvas.alpha, 1, duration * Time.deltaTime);
                yield return null;
            }
        }
        
        canvas.alpha = 1;

        yield return null;
    }

    private IEnumerator WaitThenFadeOutPopUpOverTime(CanvasGroup canvas, float duration, float delay)
    {
        if (duration > 0)
        {
            while (delay > 0)
            {
                delay -= Time.deltaTime;
                yield return null;
            }
            
            canvas.alpha = 1;
            float timer = 0;
            
            yield return null;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration * Time.deltaTime);
                yield return null;
            }
        }
        
        canvas.alpha = 0;

        yield return null;
    }
}
