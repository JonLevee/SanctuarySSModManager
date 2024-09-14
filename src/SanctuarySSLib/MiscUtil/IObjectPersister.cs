namespace SanctuarySSLib.MiscUtil
{
    public interface IObjectPersister
    {
        T Load<T>(string name) where T : class, new();
        void Save<T>(T instance, string name);
    }
}
