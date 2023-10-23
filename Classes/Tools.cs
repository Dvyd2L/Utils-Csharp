using System.Text.Json;
using Utils.Interfaces;

namespace Utils.Classes;

public static class Tools
{
    #region Props
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
    #endregion

    #region Methods
    public static void WriteJSON<T>(T json) =>
        Console.WriteLine(JsonSerializer.Serialize(json, _jsonOptions));


    public static string StringFromJSON<T>(T obj) =>
        JsonSerializer.Serialize(obj, _jsonOptions);

    public static T Deconstruct<T>(this IDeconstructable<T> obj) => obj.Deconstruct();
    public static void Deconstruct(this IDeconstruct obj) => obj.Deconstruct();
    #endregion
}