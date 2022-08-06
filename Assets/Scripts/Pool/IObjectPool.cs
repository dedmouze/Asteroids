public interface IObjectPool<TObject>
{
    TObject Request();
    void Return(TObject member);
}