using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private int seconds;
    [SerializeField] private int min;
    [SerializeField] private int sec;
    [SerializeField] private Text time;
    [SerializeField] private GameObject gameOver;

    private void Start()
    {
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        time.text = string.Format("{0}:{1}", min.ToString("00"), sec.ToString("00"));
        seconds = min * 60 + sec;

        while (seconds > 0)
        {
            yield return new WaitForSeconds(1f);

            seconds--;
            sec--;

            if (sec < 0 && min > 0)
            {
                min--;
                sec = 59;
            }
            else if (sec < 0 && min == 0)
            {
                sec = 0;
            }

            time.text = string.Format("{0}:{1}", min.ToString("00"), sec.ToString("00"));
        }

        yield return new WaitForSeconds(1f);

        Time.timeScale = 0;

        if (!gameOver.activeSelf)
        {
            gameOver.SetActive(true);
        }
    }

    public void MoreTime()
    {
        min += 2;
    }
}
