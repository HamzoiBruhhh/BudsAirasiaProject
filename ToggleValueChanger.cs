using UnityEngine;
using TMPro; // Required for TextMeshPro

public class ToggleValueChanger : MonoBehaviour
{
    [Header("TextMeshProUGUI References")]
    [Tooltip("The TextMeshProUGUI object that will be subtracted from.")]
    public TextMeshProUGUI minusValueText;
    [Tooltip("The TextMeshProUGUI object that will be multiplied.")]
    public TextMeshProUGUI multiplyValueText;

    [Header("Values for Operations")]
    [Tooltip("The value to subtract from 'minusValueText'.")]
    public float valueToSubtract = 5f;
    [Tooltip("The value to multiply 'multiplyValueText' by.")]
    public float valueToMultiply = 2f;

    void Start()
    {
        // Ensure TextMeshProUGUI references are set
        if (minusValueText == null)
        {
            Debug.LogError("Minus Value Text (TextMeshProUGUI) is not assigned in the Inspector.", this);
            return;
        }
        if (multiplyValueText == null)
        {
            Debug.LogError("Multiply Value Text (TextMeshProUGUI) is not assigned in the Inspector.", this);
            return;
        }
    }

    /// <summary>
    /// This function is called when the button is pressed.
    /// It will always perform both subtraction and multiplication.
    /// </summary>
    public void PerformBothOperations()
    {
        SubtractValue();
        MultiplyValue();
    }

    private void SubtractValue()
    {
        if (minusValueText == null) return; // Safeguard

        if (float.TryParse(minusValueText.text, out float currentValue))
        {
            currentValue -= valueToSubtract;
            minusValueText.text = currentValue.ToString();
            Debug.Log($"Subtracted {valueToSubtract}. New minus value: {currentValue}");
        }
        else
        {
            Debug.LogError($"Could not parse '{minusValueText.text}' as a number for subtraction. Please ensure the TextMeshProUGUI contains only numbers.", this);
        }
    }

    private void MultiplyValue()
    {
        if (multiplyValueText == null) return; // Safeguard

        if (float.TryParse(multiplyValueText.text, out float currentValue))
        {
            currentValue *= valueToMultiply;
            multiplyValueText.text = currentValue.ToString();
            Debug.Log($"Multiplied by {valueToMultiply}. New multiply value: {currentValue}");
        }
        else
        {
            Debug.LogError($"Could not parse '{multiplyValueText.text}' as a number for multiplication. Please ensure the TextMeshProUGUI contains only numbers.", this);
        }
    }
}