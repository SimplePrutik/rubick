﻿using UnityEngine;
using Zenject;

namespace Abilities
{
    public class AbilityViewChange : Ability
    {
        private CameraService cameraService;

        [Inject]
        public void Construct(CameraService cameraService)
        {
            this.cameraService = cameraService;
            
            cooldown = 2f;
            UseButton = ButtonSettings.ChangeView;
        }
        public override void Use()
        {
            base.Use();
            cameraService.RunPlayerCameraTransition();
        }
    }
}