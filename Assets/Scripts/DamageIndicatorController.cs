using System;
using Entities;
using UnityEngine;
using Zenject;

public class DamageIndicatorController
{
    private DiContainer container;
    private PlayerController playerController;

    public Transform Root;
    
    [Inject]
    public void Construct(
        DiContainer container,
        PlayerController playerController)
    {
        this.container = container;
        this.playerController = playerController;
    }
    
    public void SpawnIndicator(float value, Transform enemy, Vector3 spawnPosition)
    {
        var prefab = Resources.Load("Prefabs/Indicator");
        var indicator = (Indicator)container.InstantiatePrefabForComponent(typeof(Indicator), prefab, Root, Array.Empty<object>());
        indicator.transform.position = spawnPosition;
        indicator.Init(value.ToString(), playerController.transform);
    }
}