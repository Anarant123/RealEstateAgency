using RealEstateAgency.DBClient.Data.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateAgency.Desktop.Utilities
{
    public class LevenshteinDistance
    {
        public static int Calculate(string source, string target)
        {
            int sourceLength = source.Length;
            int targetLength = target.Length;
            int[,] distance = new int[sourceLength + 1, targetLength + 1];

            if (sourceLength == 0) return targetLength;
            if (targetLength == 0) return sourceLength;

            for (int i = 0; i <= sourceLength; i++)
                distance[i, 0] = i;

            for (int j = 0; j <= targetLength; j++)
                distance[0, j] = j;

            for (int i = 1; i <= sourceLength; i++)
            {
                for (int j = 1; j <= targetLength; j++)
                {
                    int cost = (source[i - 1] == target[j - 1]) ? 0 : 1;
                    distance[i, j] = Math.Min(Math.Min(
                        distance[i - 1, j] + 1,
                        distance[i, j - 1] + 1),
                        distance[i - 1, j - 1] + cost);
                }
            }

            return distance[sourceLength, targetLength];
        }

        public static List<T> SearchItemsByNameFuzzy<T>(string searchName, List<T> items, Func<T, string> nameSelector)
        {
            var result = new List<T>();

            foreach (var item in items)
            {
                if (IsFuzzyMatch(nameSelector(item), searchName))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        private static bool IsFuzzyMatch(string? name1, string? name2)
        {
            if (name1 == null || name2 == null)
                return false;

            int threshold = 3;

            int distance = LevenshteinDistance.Calculate(name1, name2);
            return distance <= threshold;
        }
    }
}
