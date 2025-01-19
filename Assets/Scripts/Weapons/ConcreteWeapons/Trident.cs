using Abilities;
using Zenject;

namespace Weapons.ConcreteWeapons
{
    public class Trident : Weapon
    {
        [Inject]
        public void Construct(AbilityFactory abilityFactory)
        {
            abilities.Add(abilityFactory.GetAbility<AbilityThreeArrow>());
        }
        
        public void WindUp()
        {
            
        }
    }
}