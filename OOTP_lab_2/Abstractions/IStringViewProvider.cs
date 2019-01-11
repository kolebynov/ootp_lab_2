namespace OOTP_lab_2.Abstractions
{
    public interface IStringViewProvider<in T>
    {
        string ToString(T value);
    }
}
