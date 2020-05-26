using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Pauser : MonoBehaviour
{
    public static bool isPaused { get; set; } = false;
    public static float timeScale { get; set; } = 1.0f;
    public static void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }
    public static void Resume()
    {
        Time.timeScale = timeScale = 1f;
        isPaused = false;
    }
    public static void Play(float timeScale = 1.0f)
    {
        Time.timeScale = Pauser.timeScale = timeScale;
        isPaused = false;
        if (timeScale < 0.001f) Pause();
    }
}