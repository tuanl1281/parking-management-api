namespace Parking.Management.Service.Utilities;

public static class ArrayUtilities
{
    public static DifferentObject<T> Different<T>(List<T> originalArray, List<T> newArray)
    {
        var result = new DifferentObject<T>();
        result.Added = new List<T>();
        result.Removed = new List<T>();

        foreach (var item in originalArray)
            if (!newArray.Contains(item))
                result.Removed.Add(item);

        foreach (var item in newArray)
            if (!originalArray.Contains(item))
                result.Added.Add(item);

        return result;
    }
}

public class DifferentObject<T>
{
    public List<T> Added { get; set; } = new();

    public List<T> Removed { get; set; } = new();
}
