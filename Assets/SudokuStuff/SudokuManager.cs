using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SudokuManager : MonoBehaviour
{
    public GameObject cellPrefab;
    public Transform gridParent;
    public AudioSource clickSound;
    public AudioSource victorySound;
    public Color blinkColor = Color.yellow;
    public float blinkDuration = 0.5f;

    // Current state (null = blank)
    int?[,] puzzle = new int?[4, 4] {
        { null, null, 4, null },
        { 2, 4, null, null },
        { 3, null, null, 4 },
        { 4, null, 3, 1 }
    };

    // Which cells are fixed
    bool[,] isLocked = new bool[4, 4] {
        { false, false, true, false },
        { true, true, false, false },
        { true, false, false, true },
        { true, false, true, true }
    };

    // The correct solution
    readonly int[,] solution = new int[4, 4] {
        { 1, 3, 4, 2 },
        { 2, 4, 1, 3 },
        { 3, 1, 2, 4 },
        { 4, 2, 3, 1 }
    };

    // Prevent firing victory repeatedly
    bool hasWon = false;

    void Start()
    {
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                var cellObj = Instantiate(cellPrefab, gridParent);
                var cellButton = cellObj.GetComponent<Button>();
                var text = cellObj.GetComponentInChildren<TMP_Text>();
                int _x = x, _y = y;

                if (isLocked[y, x])
                {
                    // Show the fixed, correct number
                    text.text = solution[y, x].ToString();
                    puzzle[y, x] = solution[y, x];
                    cellButton.interactable = false;
                }
                else
                {
                    // Start blank or with any preset
                    text.text = puzzle[y, x]?.ToString() ?? "";
                    cellButton.onClick.AddListener(() =>
                    {
                        // Cycle 0→1→…→9→0
                        if (clickSound != null) clickSound.PlayOneShot(clickSound.clip);

                        int current = puzzle[_y, _x] ?? 0;
                        current = (current + 1) % 10;
                        puzzle[_y, _x] = current == 0 ? (int?)null : current;
                        text.text = current == 0 ? "" : current.ToString();

                        CheckForVictory();
                    });
                }
            }
        }
    }

    void CheckForVictory()
    {
        if (hasWon) return;  // already won

        // Verify every cell matches the solution
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                // puzzle[y,x] must equal solution[y,x] (no nulls allowed)
                if (!puzzle[y, x].HasValue || puzzle[y, x].Value != solution[y, x])
                {
                    return; // not correct yet
                }
            }
        }

        // If we reach here, all cells are correct!
        hasWon = true;
        if (victorySound != null) victorySound.Play();
        StartCoroutine(BlinkButtons());
    }

    IEnumerator BlinkButtons()
    {
        // Gather all buttons and remember their original colors
        var buttons = new List<Button>(gridParent.GetComponentsInChildren<Button>());
        var originalColors = new List<Color>(buttons.Count);
        foreach (var btn in buttons)
            originalColors.Add(btn.image.color);

        // Set all to blinkColor
        foreach (var btn in buttons)
            btn.image.color = blinkColor;

        // Wait
        yield return new WaitForSeconds(blinkDuration);

        // Restore originals
        for (int i = 0; i < buttons.Count; i++)
            buttons[i].image.color = originalColors[i];
    }
}
