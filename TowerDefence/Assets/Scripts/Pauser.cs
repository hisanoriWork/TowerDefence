using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Pauser : MonoBehaviour
{
    public static bool isPaused { get; set; } = false;
    public static void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }
    public static void Resume()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }
    public static void Play(float timeScale = 1.0f)
    {
        Time.timeScale = timeScale;
        if (timeScale < 0.001f) Pause();
    }
}