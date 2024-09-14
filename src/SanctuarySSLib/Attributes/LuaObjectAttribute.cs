using System.Diagnostics;

namespace SanctuarySSLib.Attributes
{
    [DebuggerDisplay("File:{File} Table:{Table} Key:{Key}")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class LuaObjectAttribute : Attribute
    {
        public string Table { get; set; }
        public string File { get; set; }
        public string Key { get; set; }

        public LuaObjectAttribute()
        {
            Table = string.Empty;
            File = string.Empty;
            Key = string.Empty;
        }
    }

}
