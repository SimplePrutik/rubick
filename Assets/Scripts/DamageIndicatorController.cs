using System;
using Entities;
using Extentions;
using Pooling;
using UnityEngine;
using Zenject;

public class DamageIndicatorController
{
    private DiContainer container;
    private FpvCameraController fpvCameraController;
    private Pool<Indicator> pool;
    private RectTransform root;
    
    [Inject]
    public void Construct(
        DiContainer container,
        PlayerController playerController,
        FpvCameraController fpvCameraController)
    {
        this.container = container;
        this.fpvCameraController = fpvCameraController;
    }

    public void Init(RectTransform root)
    {
        this.root = root;
        pool = new Pool<Indicator>(30, this.root, "Prefabs/UI/Indicator", container);
    }
    
    public void SpawnIndicator(float value, Vector3 spawnPosition)
    {
        var indicator = pool.SpawnRectTransform(fpvCameraController.Camera.GetPositionOnScreen(spawnPosition, root));
        indicator.Init(value.ToString(), spawnPosition, fpvCameraController);
    }
}