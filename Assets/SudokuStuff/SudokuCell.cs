using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SudokuCell : MonoBehaviour
{
    public Button Button;
    public TextMeshProUGUI Display;

    public bool CanChange { get; private set; }
    private int x, y;
    private System.Action<int, int> onClick;

    public void Setup(int x, int y, bool canChange, string initialValue, System.Action<int, int> onClick)
    {
        this.x = x;
        this.y = y;
        this.CanChange = canChange;
        this.onClick = onClick;

        Display.text = initialValue;
        Button.interactable = canChange;

        if (!canChange)
        {
            Display.color = Color.black;
        }

        Button.onClick.AddListener(() => onClick(x, y));
    }
}
