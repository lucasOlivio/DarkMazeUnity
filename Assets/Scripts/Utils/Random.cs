using System;
using System.Numerics;

namespace DM
{
    public class LORandom
    {
        private static readonly Random randomInstance = new Random(1);

        public static int RandomRange(int minInclusive, int maxInclusive)
        {
            return randomInstance.Next(minInclusive, maxInclusive + 1);
        }

        public static T RandomEnumValue<T>()
        {
            Array valuesArray = Enum.GetValues(typeof(T));

            int randomEnumIndex = randomInstance.Next(valuesArray.Length);

            return (T)valuesArray.GetValue(randomEnumIndex);
        }
    }
}