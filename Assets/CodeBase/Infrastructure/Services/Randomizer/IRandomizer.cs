using CodeBase.Services;

namespace CodeBase.Infrastructure.Services.Randomizer
{
    public interface IRandomizer 
    {
        int Next(int min, int max);
    }
}