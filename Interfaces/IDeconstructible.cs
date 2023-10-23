namespace Utils.Interfaces;

public interface IDeconstructible<T>
{
    void Deconstruct(out T value);
}
