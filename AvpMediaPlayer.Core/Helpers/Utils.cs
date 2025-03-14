using AvpMediaPlayer.Core.Models;

namespace AvpMediaPlayer.Core.Helpers
{
    public static class Utils
    {
        public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
        {
            if(action == null) throw new ArgumentNullException("action");

            var en = values.GetEnumerator();
            while (en.MoveNext()) 
                action(en.Current);
        }
        public static void ForEach<T>(this IEnumerable<T> values, Action<T, int> action)
        {
            if (action == null) throw new ArgumentNullException("action");
            var index = -1;
            var en = values.GetEnumerator();
            while (en.MoveNext())
                action(en.Current, ++ index);
        }
        public static bool IsEmpty<T>(this IEnumerable<T> values)
            => values == null || values.Any() == false;
        public static bool IsNotEmpty<T>(this IEnumerable<T> values)
            => values != null && values.Any();
        public static bool IsEmpty(this IEnumerable<char> values)
            => values == null || values.All(char.IsWhiteSpace);
        public static double[] Average(this (double[], double[]) values)
        {
            var result = new List<double>();
            values.Item1.ForEach((d1, i) =>
            {
                var d2 = values.Item2[i];
                result.Add((d1, d2).Average());
            });

            return result.ToArray();
        }
        public static double Average(this (double, double) values)
            => (values.Item1 + values.Item2) / 2;
        public static void ForEachTwin(this (double[], double[]) values, Action<(double, double), int> action)
        {
            var index = -1;
            var en1 = values.Item1.GetEnumerator();
            var en2 = values.Item2.GetEnumerator();
            while(en1.MoveNext() && en2.MoveNext())
            {
                action(((double)en1.Current, (double)en2.Current), ++ index);
            }
        }
    }

    public static class ContentUtils
    {
        public static IEnumerable<Content> Find(this IEnumerable<Content> parent, string name)
        {
            throw new NotImplementedException();
        }
    }
   
}
