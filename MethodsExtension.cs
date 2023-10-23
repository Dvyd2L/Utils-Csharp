using System.Collections.Concurrent;

namespace Utils;
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
    /// </summary>
    /// <typeparam name="T">Representa el tipo del argumento de entrada de la función original. 
    /// Incluye una restricción de tipo que significa que T no puede ser un tipo nulo.</typeparam>
    /// <typeparam name="TResult">Representa el tipo del resultado devuelto por la función original.</typeparam>
    /// <param name="func">Funcíon que vamos a cachear</param>
    /// <returns>El resultado devuelto por la función que hemos cacheado</returns>
    public static Func<T, TResult> Memoize<T, TResult>(this Func<T, TResult> func)
        where T : notnull
    {
        ConcurrentDictionary<T, TResult> cache = new();
        return (arg) => cache.GetOrAdd(arg, func);

        #region OtraForma
        //Dictionary<T, TResult> cache = new();

        //return arg =>
        //{
        //    if (cache.TryGetValue(arg, out TResult? result))
        //        return result;

        //    result = func(arg);
        //    cache[arg] = result;
        //    return result;
        //};
        #endregion
    }
    #endregion
}