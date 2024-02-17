namespace Core.ShareExtension;

public static class ObjectExtension
{
    public static bool IsNullOrEmpty<T>(this ICollection<T> @this)
    {
        return @this == null || @this.Count == 0;
    }

    public static bool IsNull(this object @this)
    {
        return @this == null;
    }
}