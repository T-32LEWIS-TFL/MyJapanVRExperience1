using UnityEngine; // Core Unity namespace
using UnityEngine.UI; // For Button component
using TMPro; // For TextMeshProUGUI

public class SudokuCell : MonoBehaviour // Attach to each Sudoku cell GameObject
{
    public Button Button; // Reference to the UI Button
    public TextMeshProUGUI Display; // Reference to the number text display

    public bool CanChange { get; private set; } // Determines if the cell is editable by the user
    private int x, y; // Cell's position in the grid
    private System.Action<int, int> onClick; // Callback to trigger when cell is clicked

    public void Setup(int x, int y, bool canChange, string initialValue, System.Action<int, int> onClick)
    {
        this.x = x; // Set X coordinate
        this.y = y; // Set Y coordinate
        this.CanChange = canChange; // Set whether the cell is editable
        this.onClick = onClick; // Store the click callback

        Display.text = initialValue; // Set initial number to display
        Button.interactable = canChange; // Enable or disable button based on editability

        if (!canChange) // If it's a fixed/preset cell
        {
            Display.color = Color.black; // Make text color black to indicate it's not editable
        }

        Button.onClick.AddListener(() => onClick(x, y)); // Hook up the click event to pass cell coordinates
    }
}
