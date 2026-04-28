namespace LastLight.Systems
{
    /// <summary>
    /// Implemented by anything that can receive damage.
    /// </summary>
    public interface IDamageable
    {
        void TakeDamage(float amount);
    }
}