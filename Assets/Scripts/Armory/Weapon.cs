using Abilities;

public class Weapon : Ability, IWeapon
{
    public float GetDamage()
    {
        throw new System.NotImplementedException();
    }

    public float GetAttackSpeed()
    {
        throw new System.NotImplementedException();
    }

    public IBullet GetProjectile()
    {
        throw new System.NotImplementedException();
    }
}