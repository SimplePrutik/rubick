public interface IAbility
{
    public void Use();
    public void Halt();
    public void Prepare(params object [] args);
}