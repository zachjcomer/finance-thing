namespace Finance.Library;

public static class IEnumerableExtensions
{
    public static IEnumerable<V> Accumulate<T, U, V>(this IEnumerable<T> source, Func<T, U> selector, Func<U, V?, V> accumulator, V? initial = default)
    {
        V? result = initial;

        foreach (var item in source)
        {
            U value = selector(item);
            result = accumulator(value, result);

            yield return result;
        }
    }

    public static IEnumerable<U> SlidingWindow<T, U>(this IEnumerable<T> source, Func<IEnumerable<T>, U> selector, int windowSize)
    {
        Queue<T> window = new();

        foreach (var item in source)
        {
            if (window.Count == windowSize)
            {
                window.Dequeue();
            }

            window.Enqueue(item);

            yield return selector(window);
        }
    }
}
