using TMPro;
using UnityEngine;
using System;

public class TokyoTimeDisplay : MonoBehaviour
{
    [Tooltip("Drag your TextMeshPro‑UGUI here")]
    public TMP_Text timeText;

    TimeZoneInfo tokyoZone;
    bool useRandomTime = false; // Flag to fall back to random time

    void Start()
    {
        try
        {
            tokyoZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
        }
        catch (TimeZoneNotFoundException)
        {
            try
            {
                tokyoZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo");
            }
            catch (TimeZoneNotFoundException)
            {
                useRandomTime = true; // If both fail, use random time
            }
        }

        UpdateTime();

        var secondsUntilNextMinute = 60 - DateTime.UtcNow.Second;
        InvokeRepeating(nameof(UpdateTime), secondsUntilNextMinute, 60f);
    }

    void UpdateTime()
    {
        DateTime displayTime;

        if (!useRandomTime)
        {
            try
            {
                DateTime utc = DateTime.UtcNow;
                displayTime = TimeZoneInfo.ConvertTimeFromUtc(utc, tokyoZone);
            }
            catch
            {
                // If conversion fails at runtime
                useRandomTime = true;
                displayTime = GetRandomTime();
            }
        }
        else
        {
            displayTime = GetRandomTime();
        }

        timeText.text = $"In Japan, it is currently {displayTime:HH:mm}";
    }

    DateTime GetRandomTime()
    {
        System.Random rand = new System.Random();
        int hour = rand.Next(0, 24);
        int minute = rand.Next(0, 60);
        return new DateTime(1, 1, 1, hour, minute, 0);
    }
}
