using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChildController : MonoBehaviour
{
    public static UIChildController _ins;
    
    private GameObject[] ChildPages;

    private bool isSwitching = false;

    void Awake()
    {
        _ins = this;
        ChildPages = GameObject.FindGameObjectsWithTag("UI_Pages_Child");
    }

    void Start()
    {
        switchUIPage_Child("page-arm");
    }

    public void switchUIPage_Child(string pageName)
    {
        if (isSwitching) return;

        isSwitching = true;

        foreach (GameObject page in ChildPages)
        {
            Canvas canvas = page.GetComponent<Canvas>();
            if (pageName == page.name && !canvas.enabled)
            {
                // Enable Canvas for the target page
                canvas.enabled = true;
                // Ensure CanvasGroup component exists
                CanvasGroup canvasGroup = EnsureCanvasGroup(page);
                // Fade in effect
                StartCoroutine(FadeCanvas(canvasGroup, 0f, 1f, 0.5f));
            }
            else
            {
                if (canvas.enabled)
                {
                    // Disable Canvas for other pages only if it's currently enabled
                    canvas.enabled = false;
                }
            }
        }

        isSwitching = false;
    }

    CanvasGroup EnsureCanvasGroup(GameObject page)
    {
        // Ensure CanvasGroup component exists, add if not
        CanvasGroup canvasGroup = page.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = page.AddComponent<CanvasGroup>();
        }
        return canvasGroup;
    }

    IEnumerator FadeCanvas(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
}
