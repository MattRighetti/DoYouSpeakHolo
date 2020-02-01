﻿using UnityEngine;

public class HighlightEnabler : MonoBehaviour
{
    Outline outline;

    private void OnEnable() {
        outline = gameObject.AddComponent<Outline>();
        outline.OutlineColor = Color.cyan;
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineWidth = 5f;
        outline.enabled = false;
    }

    internal void EnableOutline() => outline.enabled = true;

    internal void DisableOutline() => outline.enabled = false;
}
