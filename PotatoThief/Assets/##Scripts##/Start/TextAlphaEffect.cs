using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TextAlphaEffect : MonoBehaviour
{
    [SerializeField] private Text _text;
    private void Start()
    {
        _text.DOFade(0f, 1.5f).SetLoops(-1, LoopType.Yoyo);
    }
}
