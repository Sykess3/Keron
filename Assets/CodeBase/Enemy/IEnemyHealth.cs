using CodeBase.Logic;

namespace CodeBase.Enemy
{
    public interface IEnemyHealth : IHealth
    {
        void Construct(float health);
    }
}