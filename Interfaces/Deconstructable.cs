namespace Utils.Interfaces;

public interface IDeconstructible<T>
{
    void Deconstruct(out T value);
}

public interface IDeconstruct
{
    void Deconstruct();
}
