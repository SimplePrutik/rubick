public interface IWeapon : IUsable
{
    public float GetDamage();

    public float GetAttackSpeed();

    public IBullet GetProjectile();
}
