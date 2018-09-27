using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFlasher : MonoBehaviour
{
    // the image you want to fade, assign in inspector
    public Image img;

    public AudioSource buttonNoise;

    /// <summary>
    /// time before images starts fading out
    /// </summary>
    public float fadeOutTime;

    public void OnButtonClick()
    {
        StartCoroutine(FadeImage(true));
        if(buttonNoise != null)
        {
            buttonNoise.Play();
        }
    }

    IEnumerator FadeImage(bool fadeAway)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            img.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(fadeOutTime);
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                img.color = new Color(1, 1, 1, i);
                yield return null;
            }

        }
    }
}
