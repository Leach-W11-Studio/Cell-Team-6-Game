using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [Tooltip("The start time in seconds")]
    public float startTime = 0;
    [Tooltip("Total time in seconds")]
    public float duration;
    public string stringTime;
    public float elapsedTime;
    [Tooltip("Value between 0% and 100% to start warning blink")]
    public float warningPercent;

    public bool paused = false;

    public UnityEvent OutOfTime;

    private TextMeshProUGUI watchFace;
    private Animator animator;

    private void Awake()
    {
        watchFace = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (startTime >= duration) {
            paused = true;
            animator.SetBool("outOfTime", true);
            Debug.LogWarning("Timer start time is greater than duration.");
        }

        elapsedTime = startTime;
        stringTime = formatTime(elapsedTime);
        watchFace.SetText(stringTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (paused) { return; }

        if (elapsedTime < duration)
        {
            stringTime = formatTime(elapsedTime);
            watchFace.SetText(stringTime);
            elapsedTime += Time.deltaTime;
        }
        else {
            watchFace.SetText(formatTime(duration));
            paused = true;
            OutOfTime.Invoke();
            animator.SetTrigger("outOfTime");
        }

        float percentage = (elapsedTime / duration) * 100;
        if (percentage > warningPercent && !animator.GetBool("blink"))
        {
            animator.SetTrigger("blink");
        }

    }

    private string formatTime(float seconds) {
        var span = new TimeSpan(0, 0, (int)seconds); //Or TimeSpan.FromSeconds(seconds); (see Jakob C´s answer)
        var str = string.Format("{0}:{1:00}", (int)span.TotalMinutes, span.Seconds);
        return str;
    }
}
