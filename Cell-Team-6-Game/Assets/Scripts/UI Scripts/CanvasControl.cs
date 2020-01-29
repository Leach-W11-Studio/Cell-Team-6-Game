using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasControl : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    private int[] groupAlphaArr = {1, 0};
    private int groupAlpha = 1;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = groupAlpha;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("Thing");
            groupAlpha = groupAlphaArr[groupAlpha];
            canvasGroup.alpha = groupAlpha;
        }
    }
}
