using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFlasher : MonoBehaviour
{
    // the image you want to fade, assign in inspector
    public Image img;

    public float fadeOutTime;

    public void OnButtonClick()
    {
        // fades the image out when you click
        StartCoroutine(FadeImage(true));
        Wait();

    }

    IEnumerator FadeImage(bool fadeAway)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            for (float i = 0; i <= 1; i += 2*Time.deltaTime)
            {
                // set color with i as alpha
                img.color = new Color(1, 1, 1, i);
                yield return null;
            }
            // loop over 1 second backwards

        }
    }

    IEnumerable FadeInImage(bool fadeAway)
    {
        if (fadeAway)
        {
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                img.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(fadeOutTime);
    }
}
