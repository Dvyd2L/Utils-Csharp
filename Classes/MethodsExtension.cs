using System.Collections.Concurrent;
using System.Reflection;

namespace Utils.Classes;
public static class MethodsExtension
{
    #region Strings
    /// <summary>
    /// Rellena una cadena con un carácter de relleno especificado hasta que tenga una longitud total determinada.
    /// </summary>
    /// <param name="cadena">La cadena a rellenar.</param>
    /// <param name="totalLength">La longitud total deseada de la cadena.</param>
    /// <param name="paddingChar">El carácter de relleno que se utilizará. Por defecto, es un espacio en blanco.</param>
    /// <returns>La cadena rellenada.</returns>
    public static string Padding(this string cadena, int totalLength = 50, char paddingChar = ' ')
        => cadena.PadLeft((totalLength + cadena.Length) / 2, paddingChar).PadRight(totalLength, paddingChar);

    /// <summary>
    /// Convierte el primer carácter de una cadena en mayúscula y el resto en minúscula.
    /// </summary>
    /// <param name="cadena">La cadena a convertir.</param>
    /// <returns>La cadena convertida.</returns>
    public static string TitleCase(this string cadena)
        => cadena[..1].ToUpper() + cadena[1..].ToLower();
    #endregion

    #region Generics
    /// <summary>
    /// Método de extensión pipe que toma un objeto y una función 
    /// y devuelve el resultado de aplicar la función al objeto
    /// </summary>
    /// <typeparam name="T">El tipo de entrada</typeparam>
    /// <typeparam name="TResult">El tipo devuelto</typeparam>
    /// <param name="obj">Dato al que vamos a aplicar la función</param>
    /// <param name="func">Función a aplicar</param>
    /// <returns>El resultado de aplicar la función al objeto</returns>
    public static TResult Pipe<T, TResult>(this T obj, Func<T, TResult> func) => func(obj);

    /// <summary>
    /// Este método de extensión acepta una función y devuelve una nueva función que 
    /// almacena en caché los resultados de la función original. 
    /// La primera vez que se llama a la nueva función con un argumento determinado, 
    /// se ejecuta la función original y se almacena el resultado en caché. 
    /// Las siguientes veces que se llama a la nueva función con el mismo argumento, 
    /// se devuelve el resultado almacenado en caché en lugar de ejecutar la función original.
    ///  Este enfoque garantiza que func solo se llamará una vez por argumento, pero no es seguro 
    ///  para subprocesos. Si tu método puede ser llamado desde múltiples hilos, necesitarías 
    ///  agregar bloqueo para evitar condiciones de carrera.
    /// </summary>
    /// <typeparam name="T">Representa el tipo del argumento de entrada de la función original. 
    /// Incluye una restricción de tipo que significa que T no puede ser un tipo nulo.</typeparam>
    /// <typeparam name="TResult">Representa el tipo del resultado devuelto por la función original.</typeparam>
    /// <param name="func">Funcíon que vamos a cachear</param>
    /// <returns>El resultado devuelto por la función que hemos cacheado</returns>
    public static Func<T, TResult> Memoize<T, TResult>(this Func<T, TResult> func)
        where T : notnull
    {
        Dictionary<T, TResult> cache = new();

        return arg =>
        {
            if (cache.TryGetValue(arg, out TResult? result))
                return result;

            result = func(arg);
            cache[arg] = result;
            return result;
        };
    }

    /// <summary>
    /// Este método de extensión acepta una función y devuelve una nueva función que 
    /// almacena en caché los resultados de la función original. 
    /// La primera vez que se llama a la nueva función con un argumento determinado, 
    /// se ejecuta la función original y se almacena el resultado en caché. 
    /// Las siguientes veces que se llama a la nueva función con el mismo argumento, 
    /// se devuelve el resultado almacenado en caché en lugar de ejecutar la función original.
    ///  Esta es una buena opción si tu método puede ser llamado simultáneamente desde múltiples 
    ///  hilos, ya que ConcurrentDictionary está diseñado para manejar la concurrencia de manera segura. 
    ///  Sin embargo, GetOrAdd podría ejecutar la función func más de una vez para el mismo argumento 
    ///  si se llama simultáneamente desde múltiples hilos.
    /// </summary>
    /// <typeparam name="T">Representa el tipo del argumento de entrada de la función original. 
    /// Incluye una restricción de tipo que significa que T no puede ser un tipo nulo.</typeparam>
    /// <typeparam name="TResult">Representa el tipo del resultado devuelto por la función original.</typeparam>
    /// <param name="func">Funcíon que vamos a cachear</param>
    /// <returns>El resultado devuelto por la función que hemos cacheado</returns>
    public static Func<T, TResult> ConcurrentMemoize<T, TResult>(this Func<T, TResult> func)
        where T : notnull
    {
        ConcurrentDictionary<T, TResult> cache = new();
        return (arg) => cache.GetOrAdd(arg, func);
    }

    /// <summary>
    /// Método de extensión para reflejar un objeto en un diccionario que contiene los nombres y 
    /// valores de todas sus propiedades públicas. Este método incluye una verificación para 
    /// asegurarse de que el objeto no sea null antes de intentar reflejarlo. También se ha 
    /// especificado BindingFlags.Public | BindingFlags.Instance en la llamada a GetProperties para 
    /// asegurar que solo se obtengan las propiedades públicas de instancia.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto a reflejar.</typeparam>
    /// <param name="obj">El objeto a reflejar.</param>
    /// <returns>Un diccionario que contiene los nombres y valores de todas las propiedades públicas del objeto.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si el objeto es null.</exception>
    public static IDictionary<string, object?> Reflect<T>(this T obj)
        where T : notnull
    {
        if (obj is null) throw new ArgumentNullException(nameof(obj));

        Type type = obj.GetType();
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        return properties.ToDictionary(
            prop => prop.Name,
            prop => prop.GetValue(obj)
        );
    }
    #endregion
}