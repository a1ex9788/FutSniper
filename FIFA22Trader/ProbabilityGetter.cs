using System;

namespace FIFA22Trader
{
    public static class ProbabilityGetter
    {
        private static Random random = new Random();

        public static bool GetHalfProbability()
        {
            return random.Next(0, 2) == 0;
        }
    }
}