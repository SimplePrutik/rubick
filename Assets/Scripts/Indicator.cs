﻿using System;
using Extentions;
using TMPro;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public class Indicator : MonoBehaviour
{
    [SerializeField] private TMP_Text content;

    private RectTransform rect;
    private RectTransform parentRect;
    private Vector3 currentPosition;

    private IDisposable lifeCycleDisposable;

    public void Init(string value, Vector3 worldPosition, FpvCameraController fpvCameraController)
    {
        content.text = value;
        rect = GetComponent<RectTransform>();
        parentRect = transform.parent.GetComponent<RectTransform>();
        currentPosition = worldPosition;

        lifeCycleDisposable = Observable
            .EveryUpdate()
            .Subscribe(_ =>
            {
                //make proper easing
                currentPosition +=
                    new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0.7f, 0.9f), Random.Range(-0.5f, 0.5f))
                        .normalized * Time.deltaTime;
                var screenPoint = fpvCameraController.Camera.GetPositionOnScreen(currentPosition, parentRect);
                Debug.Log(screenPoint);
                rect.anchoredPosition = screenPoint;
            });

        Observable
            .Timer(TimeSpan.FromSeconds(2f))
            .Subscribe(_ => Destroy(gameObject));
    }

    private void OnDestroy()
    {
        lifeCycleDisposable?.Dispose();
    }
}