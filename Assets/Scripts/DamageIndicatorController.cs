using System;
using Entities;
using Extentions;
using UnityEngine;
using Zenject;

public class DamageIndicatorController
{
    private DiContainer container;
    private FpvCameraController fpvCameraController;

    public RectTransform Root { get; set; }
    
    [Inject]
    public void Construct(
        DiContainer container,
        PlayerController playerController,
        FpvCameraController fpvCameraController)
    {
        this.container = container;
        this.fpvCameraController = fpvCameraController;
    }
    
    public void SpawnIndicator(float value, Vector3 spawnPosition)
    {
        var prefab = Resources.Load("Prefabs/UI/Indicator");
        var indicator = (Indicator)container.InstantiatePrefabForComponent(typeof(Indicator), prefab, Root, Array.Empty<object>());
        indicator.GetComponent<RectTransform>().anchoredPosition = fpvCameraController.Camera.GetPositionOnScreen(spawnPosition, Root);
        indicator.Init(value.ToString(), spawnPosition, fpvCameraController);
    }
}