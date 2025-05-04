using TMPro;
using UnityEngine;
using System;

public class TokyoTimeDisplay : MonoBehaviour
{
    [Tooltip("Drag your TextMeshPro‑UGUI here")]
    public TMP_Text timeText;

    TimeZoneInfo tokyoZone;

    void Start()
    {
        // On Windows this is "Tokyo Standard Time", on mac/Linux "Asia/Tokyo"
        try
        {
            tokyoZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
        }
        catch (TimeZoneNotFoundException)
        {
            tokyoZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo");
        }

        // First update right away…
        UpdateTime();

        // …then every minute on the minute
        var secondsUntilNextMinute = 60 - DateTime.UtcNow.Second;
        InvokeRepeating(nameof(UpdateTime), secondsUntilNextMinute, 60f);
    }

    void UpdateTime()
    {
        // Get UTC now, convert to Tokyo
        DateTime utc = DateTime.UtcNow;
        DateTime tokyo = TimeZoneInfo.ConvertTimeFromUtc(utc, tokyoZone);

        // Format e.g. “In Japan, it is currently 14:27”
        timeText.text = $"In Japan, it is currently {tokyo:HH:mm}";
    }
}
