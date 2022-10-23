using System;
using System.Collections.Generic;

namespace NiftyFramework.Scripts
{
    public static class ListUtils
    {
        public static T RandomItem<T>(this List<T> list)
        {
            var random = new Random();
            int randomIndex = random.Next(0, list.Count);
            return list[randomIndex];
        }
        
        public static T RandomItem<T>(this List<T> list, Random random)
        {
            if (random == null)
            {
                return RandomItem(list);
            }
            int randomIndex = random.Next(0, list.Count);
            return list[randomIndex];
        }
        
        public static int[] GenerateInts(int count, Random random, int min, int max)
        {
            int[] values = new int[count];

            for (int i = 0; i < count; ++i)
                values[i] = random.Next(min, max);

            return values;
        }
    }
}