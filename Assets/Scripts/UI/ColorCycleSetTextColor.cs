using TMPro;
using UnityEngine;

public class ColorCycleSetTextColor : MonoBehaviour, IColorCycle {

    // Create an interface for this, so I can use it for UI, sprites and for other possible uses cases

    [SerializeField] private float _cycleSpeed = 24f;

    private Color _currentColor;

    private void Start() {
        // Default red
        _currentColor = Color.red;
    }

    private void Update() {
        // Calculate the amount to traverse the color wheel
        float colorWheelFraction = Time.deltaTime / _cycleSpeed;

        _currentColor = ShiftHue(_currentColor, colorWheelFraction);

        transform.GetComponent<TextMeshProUGUI>().color = _currentColor;
    }

    public void SetElementColor() {

    }

    // Shifts the hue of a given color by the given fraction of the color wheel.
    private Color ShiftHue(Color color, float fraction) {
        // HSV
        float hue, saturation, value;
        // Gets the current hue, saturation and value from the current RGB color 
        Color.RGBToHSV(color, out hue, out saturation, out value);

        //Hue extremeties on the color wheel:
        // 1 = 360 degrees(Red),
        // 0 = 0 degress(Red)
        hue += fraction;
        if (hue > 1f) {
            hue -= 1f;
        }

        return Color.HSVToRGB(hue, saturation, value);
    }
}
