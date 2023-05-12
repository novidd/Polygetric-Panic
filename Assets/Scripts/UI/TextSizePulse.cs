using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

/*
 * Creates a pulsating motion by tweening the transform forward and backward
 */
public class TextSizePulse : MonoBehaviour
{
    [SerializeField] private float _desiredShrinkedSize = 0.85f;
    [SerializeField] private float _timeToShrink = 0.4f;

    private void Start() {
        transform.DOScale(_desiredShrinkedSize, _timeToShrink).SetLoops(-1, LoopType.Yoyo);
    }
}
