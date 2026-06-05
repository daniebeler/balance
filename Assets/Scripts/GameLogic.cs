using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{

    private Camera cam;
    private bool timeIsRunning = false;

    private float time = 0f;

    public Text best;
    public Text current;

    public Text startAdvise;

    private int LevelReached = 1;

    private Coroutine coFadeOut = null;
    private Coroutine coFadeIn = null;

    private bool bFadeOut = false;
    private bool bFadeIn = false;

    void Start()
    {

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;


        cam = Camera.main;

        best.text = "Best: " + PlayerPrefs.GetFloat("best", 0f).ToString("F") + "s";

        startAdvise.transform.position = new Vector3(Screen.width / 2, Screen.height * 3 / 4, 0);
    }

    void Update()
    {

        Quaternion targetAngle = Quaternion.Euler(0, 0, 180);
        float precision = 0.75f;
        if (Mathf.Abs(Quaternion.Dot(this.transform.rotation, targetAngle)) > precision)
        {
            cam.backgroundColor = new Color32(10, 190, 103, 255);

            if (!timeIsRunning)
            {
                timeIsRunning = true;
                if (bFadeOut)
                {
                    StopCoroutine(coFadeOut);
                    bFadeOut = false;
                }
                else if (bFadeIn)
                {
                    StopCoroutine(coFadeIn);
                    bFadeIn = false;
                }
                coFadeOut = StartCoroutine(FadeOutAdvise());
            }

            time += Time.deltaTime;
            current.text = time.ToString("F") + "s";
        }
        else
        {
            cam.backgroundColor = new Color32(255, 198, 85, 255);

            if (timeIsRunning)
            {
                timeIsRunning = false;
                if (time > PlayerPrefs.GetFloat("best", 0f))
                {
                    PlayerPrefs.SetFloat("best", time);
                    best.text = "Best: " + time.ToString("F") + "s";
                }

                time = 0f;
                LevelReached = 1;

                transform.localScale = new Vector3(1f, 1f, 1);
                if (bFadeOut)
                {
                    StopCoroutine(coFadeOut);
                    bFadeOut = false;
                }
                else if (bFadeIn)
                {
                    StopCoroutine(coFadeIn);
                    bFadeIn = false;
                }
                coFadeIn = StartCoroutine(FadeInAdvise());
            }
        }

        if (time / LevelReached > 5f)
        {
            LevelReached++;
            transform.localScale = new Vector3(transform.localScale.x * 0.9f, transform.localScale.y * 0.9f, 1f);
        }
    }

    IEnumerator FadeOutAdvise()
    {
        bFadeOut = true;
        while (startAdvise.color.a > 0)
        {
            startAdvise.color = new Color(startAdvise.color.r, startAdvise.color.g, startAdvise.color.b, startAdvise.color.a - Time.deltaTime * 2);
            yield return null;
        }
        bFadeOut = false;
    }

    IEnumerator FadeInAdvise()
    {
        bFadeIn = true;
        float fZwischenergebnis = 1;
        while (startAdvise.color.a < 1)
        {
            fZwischenergebnis -= Time.deltaTime * 2;
            startAdvise.color = new Color(startAdvise.color.r, startAdvise.color.g, startAdvise.color.b, 1 - fZwischenergebnis);
            yield return null;
        }
        bFadeIn = false;
    }
}
