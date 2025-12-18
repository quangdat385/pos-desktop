namespace PosApi.Utils
{
    using System;
    public static class RandomIntNumber
    {
        private static readonly Random _random = new Random();
        public static int Generate(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
}
