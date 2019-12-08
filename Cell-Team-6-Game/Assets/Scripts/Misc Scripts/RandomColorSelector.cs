using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColorSelector : MonoBehaviour
{
    public Color32 baseColor;
    public int colorVariance = 10;
    public int alpha = 255;

    private int r,g,b;
    public Color finalColor;

    // Start is called before the first frame update
    void Start()
    {
        baseColor.r = (byte)Mathf.Clamp(baseColor.r, 0+colorVariance, 255-colorVariance);
        baseColor.g = (byte)Mathf.Clamp(baseColor.g, 0+colorVariance, 255-colorVariance);
        baseColor.b = (byte)Mathf.Clamp(baseColor.b, 0+colorVariance, 255-colorVariance);

        r = baseColor.r + Random.Range(-colorVariance, colorVariance);
        g = baseColor.g + Random.Range(-colorVariance, colorVariance);
        b = baseColor.b + Random.Range(-colorVariance, colorVariance);
        //Debug.Log(r + " " + g + " " + b);

        finalColor = new Color32((byte)r, (byte)g, (byte)b, (byte)alpha);
        gameObject.GetComponentInChildren<SpriteRenderer>().color = finalColor;
    }
}
