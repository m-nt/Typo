using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(RectTransform))]
[ExecuteInEditMode]
public class RectSpriteRenderer : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private RectTransform _transform;

    // Update is called once per frame
    void Update()
    {
        if (_renderer == null) { _renderer = GetComponent<SpriteRenderer>(); return; }
        if (_renderer.drawMode != SpriteDrawMode.Sliced) { _renderer.drawMode = SpriteDrawMode.Sliced; return; }
        if (_transform == null) { _transform = GetComponent<RectTransform>(); return; }
        _renderer.size = _transform.sizeDelta;

    }
}
