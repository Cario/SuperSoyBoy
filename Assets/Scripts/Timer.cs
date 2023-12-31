using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private Text timerText;

    void Awake()
    {
        timerText = GetComponent<Text>();
    }
    // Update is called once per frame
    void Update()
    {
        timerText.text = System.Math.Round((decimal)Time.timeSinceLevelLoad, 2).ToString();
    }
}
