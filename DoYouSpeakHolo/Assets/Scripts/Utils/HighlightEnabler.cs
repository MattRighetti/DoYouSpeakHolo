using UnityEngine;

public class HighlightEnabler : MonoBehaviour
{
    Outline outline;

    private void OnEnable() {
        outline = gameObject.GetComponent<Outline>();
        outline.OutlineColor = Color.cyan;
        outline.OutlineMode = Outline.Mode.SilhouetteOnly;
        outline.OutlineWidth = 3f;
        outline.enabled = false;
    }

    internal void EnableOutline() => outline.enabled = true;

    internal void DisableOutline() => outline.enabled = false;
}
