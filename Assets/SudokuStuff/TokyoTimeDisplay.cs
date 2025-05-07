using TMPro; // For TMP_Text component
using UnityEngine; // Unity engine base types
using System; // For DateTime and TimeZoneInfo

public class TokyoTimeDisplay : MonoBehaviour // Attach this script to a UI object
{
    [Tooltip("Drag your TextMeshPro‑UGUI here")]
    public TMP_Text timeText; // Reference to the UI text component that shows the time

    TimeZoneInfo tokyoZone; // Will hold the Tokyo time zone info

    void Start()
    {
        // On Windows this is "Tokyo Standard Time", on mac/Linux "Asia/Tokyo"
        try
        {
            tokyoZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time"); // Try Windows ID
        }
        catch (TimeZoneNotFoundException)
        {
            tokyoZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo"); // Fallback for macOS/Linux
        }

        UpdateTime(); // Update immediately on start

        var secondsUntilNextMinute = 60 - DateTime.UtcNow.Second; // Calculate delay to sync with next full minute
        InvokeRepeating(nameof(UpdateTime), secondsUntilNextMinute, 60f); // Then update every 60s
    }

    void UpdateTime()
    {
        DateTime utc = DateTime.UtcNow; // Get current UTC time
        DateTime tokyo = TimeZoneInfo.ConvertTimeFromUtc(utc, tokyoZone); // Convert to Tokyo time

        timeText.text = $"In Japan, it is currently {tokyo:HH:mm}"; // Format and display the time
    }
}
