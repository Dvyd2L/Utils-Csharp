using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;

namespace Utils.Exceptions;

/// <summary>
/// Plantilla de clase para crear mis propias excepciones personalizadas
/// </summary>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
internal class CustomException : Exception, IEquatable<CustomException?>
{
    #region Props
    /// <summary>
    /// Propiedad heredada de la clase Exception que se utiliza para almacenar información adicional sobre la excepción.
    /// </summary>
    public override IDictionary Data => base.Data;

    /// <summary>
    /// Propiedad que se utiliza para proporcionar un vínculo a la ayuda en línea relacionada con la excepción.
    /// </summary>
    public override string? HelpLink { get => base.HelpLink; set => base.HelpLink = value; }

    /// <summary>
    /// Propiedad que devuelve un mensaje que describe la excepción.
    /// </summary>
    public override string Message => base.Message;

    /// <summary>
    /// Propiedad que devuelve el nombre del objeto o la aplicación que generó la excepción.
    /// </summary>
    public override string? Source { get => base.Source; set => base.Source = value; }

    /// <summary>
    /// Propiedad que devuelve una representación como cadena de la pila de llamadas en el momento en que se produjo la excepción.
    /// </summary>
    public override string? StackTrace => base.StackTrace;
    #endregion

    #region Constructors
    internal CustomException()
        : base() { }

    internal CustomException(string? message)
        : base(message) { }

    internal CustomException(string? message, Exception? innerException)
        : base(message, innerException) { }

    protected CustomException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }

    public CustomException(string? helpLink, string? source)
        : this(helpLink) => Source = source;
    #endregion

    #region Methods
    /// <summary>
    /// Método utilizado para personalizar cómo se muestra la instancia de la clase en la ventana de variables del depurador.
    /// </summary>
    private string GetDebuggerDisplay() => ToString();

    /// <summary>
    /// Método heredado de la clase Exception utilizado para determinar si dos instancias de la clase son iguales.
    /// </summary>
    public override bool Equals(object? obj)
        => Equals(obj as CustomException);
    public bool Equals(CustomException? other)
        => other is not null
        && EqualityComparer<IDictionary>.Default.Equals(Data, other.Data)
        && HelpLink == other.HelpLink
        && HResult == other.HResult
        && EqualityComparer<Exception?>.Default.Equals(InnerException, other.InnerException)
        && Message == other.Message
        && Source == other.Source
        && StackTrace == other.StackTrace
        && EqualityComparer<MethodBase?>.Default.Equals(TargetSite, other.TargetSite);

    /// <summary>
    /// Método heredado de la clase Exception utilizado para devolver un código hash para la instancia de la clase.
    /// </summary>
    public override int GetHashCode()
        => base.GetHashCode();

    /// <summary>
    /// Método heredado de la clase Exception utilizado para devolver la excepción raíz que causó esta excepción.
    /// </summary>
    public override Exception GetBaseException()
        => base.GetBaseException();

    /// <summary>
    /// Método heredado de la clase Exception utilizado para serializar los datos necesarios para crear una instancia de esta excepción.
    /// </summary>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
        => base.GetObjectData(info, context);

    /// <summary>
    /// Método heredado de la clase Exception utilizado para devolver una cadena que representa la excepción actual.
    /// </summary>
    public override string ToString()
        => base.ToString();

    /// <summary>
    /// Operador sobrecargado utilizado para determinar si dos instancias de la clase son iguales.
    /// </summary>
    public static bool operator ==(CustomException? left, CustomException? right)
        => EqualityComparer<CustomException>.Default.Equals(left, right);

    /// <summary>
    /// Operador sobrecargado utilizado para determinar si dos instancias de la clase son diferentes.
    /// </summary>
    public static bool operator !=(CustomException? left, CustomException? right)
        => !(left == right);
    #endregion
}