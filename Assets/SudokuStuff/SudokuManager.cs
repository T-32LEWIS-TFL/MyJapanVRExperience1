using TMPro; // Import TextMeshPro namespace for UI text handling
using UnityEngine; // Unity engine core functionalities
using UnityEngine.UI; // UI components like Button, Image, etc.
using System.Collections; // Needed for coroutines
using System.Collections.Generic; // Allows use of List and other generic collections

public class SudokuManager : MonoBehaviour // Define a MonoBehaviour class to attach to a GameObject
{
    public GameObject cellPrefab; // Prefab for each Sudoku cell
    public Transform gridParent; // Parent transform where cells will be instantiated
    public AudioSource clickSound; // Sound played when a cell is clicked
    public AudioSource victorySound; // Sound played when puzzle is completed
    public Color blinkColor = Color.yellow; // Color used when blinking cells on win
    public float blinkDuration = 0.5f; // Duration of the blink effect

    // Current puzzle state (null represents a blank cell)
    int?[,] puzzle = new int?[4, 4] {
        { null, null, 4, null },
        { 2, 4, null, null },
        { 3, null, null, 4 },
        { 4, null, 3, 1 }
    };

    // Indicates which cells are locked/fixed and not interactable
    bool[,] isLocked = new bool[4, 4] {
        { false, false, true, false },
        { true, true, false, false },
        { true, false, false, true },
        { true, false, true, true }
    };

    // The correct solution to the puzzle
    readonly int[,] solution = new int[4, 4] {
        { 1, 3, 4, 2 },
        { 2, 4, 1, 3 },
        { 3, 1, 2, 4 },
        { 4, 2, 3, 1 }
    };

    // Flag to prevent victory from being triggered more than once
    bool hasWon = false;

    void Start() // Called on the first frame the script is active
    {
        // Loop through each cell in a 4x4 grid
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                // Instantiate a new cell from the prefab as a child of gridParent
                var cellObj = Instantiate(cellPrefab, gridParent);
                // Get the Button component of the instantiated cell
                var cellButton = cellObj.GetComponent<Button>();
                // Get the TextMeshPro component inside the cell
                var text = cellObj.GetComponentInChildren<TMP_Text>();
                // Capture current x and y into local variables for lambda closure
                int _x = x, _y = y;

                if (isLocked[y, x]) // If the cell is a fixed/predefined one
                {
                    // Display the correct number in the cell
                    text.text = solution[y, x].ToString();
                    // Update the puzzle state with the fixed value
                    puzzle[y, x] = solution[y, x];
                    // Disable interaction on this button
                    cellButton.interactable = false;
                }
                else // If the cell is editable
                {
                    // Display existing value or leave it blank
                    text.text = puzzle[y, x]?.ToString() ?? "";
                    // Add a click event listener to this cell
                    cellButton.onClick.AddListener(() =>
                    {
                        // Play click sound if assigned
                        if (clickSound != null) clickSound.PlayOneShot(clickSound.clip);

                        // Get current value or default to 0
                        int current = puzzle[_y, _x] ?? 0;
                        // Increment value and wrap around to 0 after 9
                        current = (current + 1) % 10;
                        // Update puzzle state with new value or null if 0
                        puzzle[_y, _x] = current == 0 ? (int?)null : current;
                        // Update UI text to reflect new value
                        text.text = current == 0 ? "" : current.ToString();

                        // Check if the player has completed the puzzle
                        CheckForVictory();
                    });
                }
            }
        }
    }

    void CheckForVictory() // Verifies if the current puzzle state matches the solution
    {
        if (hasWon) return;  // Skip if already won

        // Check all cells for correctness
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                // If cell is null or doesn't match the solution, exit early
                if (!puzzle[y, x].HasValue || puzzle[y, x].Value != solution[y, x])
                {
                    return; // Puzzle is not yet complete or correct
                }
            }
        }

        // If all cells match the solution, mark as won
        hasWon = true;
        // Play victory sound if assigned
        if (victorySound != null) victorySound.Play();
        // Start coroutine to blink all buttons
        StartCoroutine(BlinkButtons());
    }

    IEnumerator BlinkButtons() // Coroutine to make all buttons flash briefly
    {
        // Get all buttons under gridParent
        var buttons = new List<Button>(gridParent.GetComponentsInChildren<Button>());
        // Store each button's original color
        var originalColors = new List<Color>(buttons.Count);
        foreach (var btn in buttons)
            originalColors.Add(btn.image.color);

        // Set each button to the blink color
        foreach (var btn in buttons)
            btn.image.color = blinkColor;

        // Wait for the specified blink duration
        yield return new WaitForSeconds(blinkDuration);

        // Restore each button's original color
        for (int i = 0; i < buttons.Count; i++)
            buttons[i].image.color = originalColors[i];
    }
}
