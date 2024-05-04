using System;
using Abilities;
using UI;
using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField] private TpvCameraController tpvCameraController;
    [SerializeField] private FpvCameraController fpvCameraController;
    [SerializeField] private PlayerController playerController;
    public override void InstallBindings()
    {
        Container.BindIFactory<Type, BaseScreen>().FromFactory<ScreensFactory>();
        
        Container.BindInterfacesAndSelfTo<ScreensService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<MovementService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<AbilityService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<UnitColliderService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<CameraManager>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<AppStart>().AsSingle().NonLazy();
        
        Container.Bind<TpvCameraController>().FromComponentInNewPrefab(tpvCameraController).AsSingle().NonLazy();
        Container.Bind<FpvCameraController>().FromComponentInNewPrefab(fpvCameraController).AsSingle().NonLazy();
        Container.Bind<PlayerController>().FromComponentInNewPrefab(playerController).AsSingle().NonLazy();
    }
}