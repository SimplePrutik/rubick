using Entities;
using Weapons.ConcreteWeapons;
using Zenject;

namespace Weapons
{
    public class WeaponService
    {
        private PlayerController playerController;
        private FpvCameraController fpvCameraController;
        
        [Inject]
        public void Construct(
            PlayerController playerController,
            FpvCameraController fpvCameraController)
        {
            this.playerController = playerController;
            this.fpvCameraController = fpvCameraController;
        }
        
        public void TakeWeaponInHand(Weapon weapon)
        {
            weapon.transform.SetParent(fpvCameraController.transform);
        }
    }
}