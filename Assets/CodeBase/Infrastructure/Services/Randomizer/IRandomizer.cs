using CodeBase.Services;

namespace CodeBase.Infrastructure.Services.Randomizer
{
    public interface IRandomizer : IService
    {
        int Next(int min, int max);
    }
}