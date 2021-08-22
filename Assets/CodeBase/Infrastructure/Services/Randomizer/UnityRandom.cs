using UnityEngine;

namespace CodeBase.Infrastructure.Services.Randomizer
{
    public class UnityRandom : IRandomizer
    {
        public int Next(int min, int max) => 
            Random.Range(min, max);
    }
}