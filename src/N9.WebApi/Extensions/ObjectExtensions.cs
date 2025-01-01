using System.Linq.Expressions;
using N9.Data.Init;

namespace N9.WebApi.Extensions;

public static class ObjectExtensions
{
    public static TResult Run<T, TResult>(this T value, Func<T, TResult> func)
    {
        return func(value);
    }
    
    public static void Let<T>(this T value, Action<T> action)
    {
        action(value);
    }
    
    public static async Task LetAsync<T>(this T value, Func<T, Task> action)
    {
        await action(value);
    }
    
    public static async Task RunAsync<T>(this T value, Func<T, Task> func)
    {
        await func.Invoke(value);
    }
    
    public static async Task<TResult> RunAsync<T, TResult>(this T value, Func<T, Task<TResult>> func)
    {
        return await func(value);
    }
}