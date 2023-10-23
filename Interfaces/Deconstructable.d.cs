namespace Utils.Interfaces;

public interface IDeconstructable<T>
{
    T Deconstruct();
}

public interface IDeconstruct
{
    void Deconstruct();
}
