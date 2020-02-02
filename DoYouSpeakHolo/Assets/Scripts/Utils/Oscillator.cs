using UnityEngine;

public class Oscillator : MonoBehaviour {
    [SerializeField] [Range(0, 50)] private float oscillationRate = 3;
    [SerializeField] [Range(0, 1)] private float oscillationRange = 0.08f;
    [SerializeField] private float upperHeightLimit = 0.5f;
    [SerializeField] private float lowerHeightLimit = 0.2f;

    bool onFocus = false;

    private void Update() {
        if (!onFocus) {
            transform.position = ClampHeight((Vector3.up * Mathf.Cos(Time.time * oscillationRate) * ClampRange(oscillationRange)) + transform.position);
        }
    }

    private float ClampRange(float value) {

        if (transform.position.y > upperHeightLimit)
            upperHeightLimit = transform.position.y;

        if (transform.position.y < lowerHeightLimit)
            lowerHeightLimit = transform.position.y - lowerHeightLimit;

        if (upperHeightLimit < lowerHeightLimit)
            upperHeightLimit = lowerHeightLimit + 0.1f;

        if (lowerHeightLimit > upperHeightLimit)
            lowerHeightLimit = upperHeightLimit + 0.1f;

        if (value != ((upperHeightLimit + lowerHeightLimit) / 2) - 0.25f)
            value = ((upperHeightLimit + lowerHeightLimit) / 2) - 0.25f;

        if (value != value * oscillationRange)
            value *= oscillationRange;

        value *= 0.01f;

        return value;
    }

    private Vector3 ClampHeight(Vector3 value) {
        if (value.y < lowerHeightLimit)
            value.y = lowerHeightLimit;
        if (value.y > upperHeightLimit)
            value.y = upperHeightLimit;
        return value;
    }

    public void OnFocusEnter() {
        Debug.Log("Disabling oscillator");
        onFocus = true;
    }

    public void OnFocusExit() {
        Debug.Log("Enabling oscillator");
        onFocus = false;
    }
}