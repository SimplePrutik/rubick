using System;
using Abilities;
using ScriptableObjects;
using UI;
using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField] private TpvCameraController tpvCameraController;
    [SerializeField] private FpvCameraController fpvCameraController;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PhysicsSettings physicsSettings;
    [SerializeField] private PlayerStats playerStats;
    public override void InstallBindings()
    {
        Container.BindIFactory<Type, BaseScreen>().FromFactory<ScreensFactory>();
        
        Container.BindInterfacesAndSelfTo<ScreensService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<MovementService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<AbilityService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<UnitColliderService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<CameraService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<AppStart>().AsSingle().NonLazy();
        
        Container.BindInterfacesAndSelfTo<AbilityJump>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<AbilityViewChange>().AsSingle().NonLazy();
        
        Container.Bind<TpvCameraController>().FromComponentInNewPrefab(tpvCameraController).AsSingle().NonLazy();
        Container.Bind<FpvCameraController>().FromComponentInNewPrefab(fpvCameraController).AsSingle().NonLazy();
        Container.Bind<PlayerController>().FromComponentInNewPrefab(playerController).AsSingle().NonLazy();
        Container.Bind<PhysicsSettings>().FromScriptableObject(physicsSettings).AsSingle().NonLazy();
        Container.Bind<PlayerStats>().FromScriptableObject(playerStats).AsSingle().NonLazy();
    }
}