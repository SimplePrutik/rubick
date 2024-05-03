using Extentions;
using Zenject;

namespace Abilities
{
    public class AbilityJump : Ability
    {
        private bool blockReset;
        private bool blockJump;
        
        public override void Use()
        {
            base.Use();
            if (blockJump)
                return;
            // if (Input.GetKey(MovementSettings.Jump))
            // {
            //     verticalMovingVelocity = playerStats.JumpHeight;
            //     blockJump = true;
            //     blockVerticalReset = true;
            // }
        }
    }
}