using System.Diagnostics.Contracts;
using System.Text.Json;

namespace SanctuarySSLib.MiscUtil
{
    public class FilePersister : IObjectPersister
    {
        private readonly AppInfo appInfo;

        public FilePersister(AppInfo appInfo) 
        {
            this.appInfo = appInfo;
        }
        public T Load<T>(string name) where T : class, new()
        {
            var file = Path.Combine(appInfo.ShatteredSunInstallRoot, name + ".json");
            if (!File.Exists(file))
            {
                return new T();
            }
            var instance = JsonSerializer.Deserialize<T>(File.ReadAllText(file));
            Contract.Assert(instance != null);
            return instance;
        }

        public void Save<T>(T instance, string name)
        {
            var file = Path.Combine(appInfo.ShatteredSunInstallRoot, name + ".json");
            File.WriteAllText(file, JsonSerializer.Serialize(instance));
        }
    }
}
