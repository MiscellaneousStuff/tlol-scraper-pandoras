using Api.Utils;

namespace Api.Internal.Utils
{
    internal class RandomGenerator : IRandomGenerator
    {
        private readonly Random _random;
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public RandomGenerator()
        {
            _random = new Random((int)DateTime.UtcNow.Ticks);
        }

        public string GetRandomString(int minLen, int maxLen)
        {
            return new string(Enumerable.Repeat(chars, _random.Next(minLen, maxLen)).Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        public float NexInt(int min, int max)
        {
            return _random.Next(min, max);
        }

        public float NextFloat()
        {
            return _random.NextSingle();
        }

        public float NextFloat(float min, float max)
        {
            return _random.NextSingle() * (max - min) + min;
        }
    }
}