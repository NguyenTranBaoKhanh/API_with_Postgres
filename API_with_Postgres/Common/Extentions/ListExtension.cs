using System.Security.Cryptography;

namespace API_with_Postgres.Common.Extentions
{
    public static class ListExtension
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            using var provider = RandomNumberGenerator.Create();

            var n = list.Count;

            while (n > 1)
            {
                var box = new byte[1];
                do provider.GetBytes(box);
                while (box[0] >= n * (byte.MaxValue / n));
                var k = box[0] % n;
                n--;
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        public static T[] PrependToArray<T>(this T value, T[] source)
        {
            return new[] { value }.Concat(source).ToArray();
        }

        public static T[] AppendToArray<T>(this T value, T[] source)
        {
            return source.Concat(new[] { value }).ToArray();
        }
    }
}
