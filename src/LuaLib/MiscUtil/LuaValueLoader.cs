using NLua;
using SanctuarySSLib.Attributes;
using SanctuarySSLib.LuaUtil;
using SanctuarySSModManager.Extensions;
using System.Reflection;
using System.Text;

namespace SanctuarySSLib.MiscUtil
{
    [SingletonService]
    public class LuaValueLoader
    {
        private readonly IGameMetadata gameMetadata;

        public LuaValueLoader(IGameMetadata gameMetadata)
        {
            this.gameMetadata = gameMetadata;
        }
        public LuaTable GetTableFromFile(string relativePath, string tableName)
        {
            var luaFile = Path.Combine(gameMetadata.LuaPath, relativePath);
            using (Lua lua = new Lua())
            {
                lua.State.Encoding = Encoding.UTF8;
                lua.DoFile(luaFile);
                var table = (LuaTable)lua[tableName];
                var mo = table.ToModelObject();
                return table;
            }

        }
        public T CreateInstanceFromTable<T>(LuaTable luaTable) where T : class, new()
        {
            T instance = new T();
            foreach (var property in instance.GetType().GetProperties())
            {
                var attr = property.GetCustomAttribute<LuaValueAttribute>();
                if (null == attr)
                    continue;
                var aggregateKey = attr.Key ?? property.Name.CamelCase();
                object value = luaTable;
                foreach (var key in aggregateKey.Split('/'))
                {
                    value = ((LuaTable)value)[key];
                }

                property.SetValue(instance, value);
            }
            return instance;
        }
    }
}
