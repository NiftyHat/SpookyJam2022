using System;
using System.Collections.Generic;
using System.Linq;

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
        
        private static readonly object sync = new object();
        
        public static int[] GenerateInts(int count, Random random, int min, int max)
        {
            int[] values = new int[count];

            for (int i = 0; i < count; ++i)
                values[i] = random.Next(min, max);

            return values;
        }
        
        public static void WithRandom<T>(this List<T> list, int count, Action<T> deltaFunction)
        {
            int len = Math.Min(count, list.Count);
            List<int> randomIndexList = Enumerable.Range(0, len).ToList();
            randomIndexList.Shuffle();
            for (int i = 0; i < len; i++)
            {
                int randomIndex = randomIndexList[i];
                deltaFunction(list[randomIndex]);
            }
        }
        
        public static List<T> GetRandomItems<T>(this List<T> list, int count)
        {
            return GetRandomInRange(list, 0, count,  new Random());
        }
        public static List<T> GetRandomItems<T>(this List<T> list, int count, System.Random random)
        {
            return GetRandomInRange(list, 0, count, random);
        }

        public static List<T> GetRandomInRange<T>(this List<T> list, int start = 0, int end = -1)
        {
            return GetRandomInRange(list, start, end, new Random());
        }
        
        public static List<T> GetRandomInRange<T>(this List<T> list, int start, int end, System.Random random)
        {
            if (end == -1)
            {
                end = list.Count;
            }

            List<T> output = new List<T>();
            List<int> randomIndexList = Enumerable.Range(start, end).ToList();
            randomIndexList.Shuffle(random);
            for (int i = start; i < end && i < list.Count; i++)
            {
                int randomIndex = randomIndexList[i];
                var traitData = list[randomIndex];
                output.Add(traitData);
            }
            return output;
        }
        
        //Fisher Yates shuffle.
        public static void Shuffle<T>(this T[] array, Random random)
        {
            int n = array.Length;
            for (int i = 0; i < (n - 1); i++)
            {
                // Use Next on random instance with an argument.
                // ... The argument is an exclusive bound.
                //     So we will not go past the end of the array.
                int r = i + random.Next(n - i);
                T t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
        }
        
        public static void Shuffle<T>(this T[] array)
        {
            Shuffle(array, new Random());
        }
        
        public static void Shuffle<T>(this List<T> list, Random random)
        {
            int n = list.Count;
            for (int i = 0; i < (n - 1); i++)
            {
                // Use Next on random instance with an argument.
                // ... The argument is an exclusive bound.
                //     So we will not go past the end of the array.
                int r = i + random.Next(n - i);
                T t = list[r];
                list[r] = list[i];
                list[i] = t;
            }
        }

        public static void Shuffle<T>(this List<T> list)
        {
            Shuffle(list, new Random());
        }
    }
}