using System;
using Abilities;
using Fight.Stances;
using ScriptableObjects;
using UI;
using UI.Reticle;
using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField] private TpvCameraController tpvCameraController;
    [SerializeField] private FpvCameraController fpvCameraController;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PhysicsSettings physicsSettings;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private CameraSettings cameraSettings;
    public override void InstallBindings()
    {
        Container.BindIFactory<Type, BaseScreen>().FromFactory<ScreensFactory>();
        Container.BindIFactory<Type, BaseReticle>().FromFactory<ReticleFactory>();
        Container.BindIFactory<Type, Ability>().FromFactory<AbilityFactory>();
        Container.BindIFactory<Type, BaseStance>().FromFactory<StanceFactory>();
        
        Container.BindInterfacesAndSelfTo<ScreensService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<MovementService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<AbilityService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<UnitColliderService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ReticleService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<CameraService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<AppStart>().AsSingle().NonLazy();
        
        Container.Bind<TpvCameraController>().FromComponentInNewPrefab(tpvCameraController).AsSingle().NonLazy();
        Container.Bind<FpvCameraController>().FromComponentInNewPrefab(fpvCameraController).AsSingle().NonLazy();
        Container.Bind<PlayerController>().FromComponentInNewPrefab(playerController).AsSingle().NonLazy();
        Container.Bind<PhysicsSettings>().FromScriptableObject(physicsSettings).AsSingle().NonLazy();
        Container.Bind<PlayerStats>().FromScriptableObject(playerStats).AsSingle().NonLazy();
        Container.Bind<CameraSettings>().FromScriptableObject(cameraSettings).AsSingle().NonLazy();
    }
}