using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace LuaParserUtil.LuaObjects
{
    [DebuggerDisplay("Count:{dictionary.Count}")]
    public class LuaDictionary : LuaValueObject
    {
        public static readonly LuaDictionary Null = new LuaDictionary();
        private Dictionary<object, LuaValueObject> dictionary = new Dictionary<object, LuaValueObject>();
        public LuaDictionary()
        {
        }
        public int ArrayLength { get; set; }

        public IEnumerable<LuaValueObject> ArrayItems => Enumerable.Range(1, ArrayLength).Select(i=> dictionary[i]);

        public void Add(LuaValueObject value)
        {
            dictionary.Add(++ArrayLength, value);
        }
        public void Add(LuaKeyValue keyValue)
        {
            dictionary.Add(keyValue.Key, keyValue.Value);
        }

        public IEnumerable<object> Keys => dictionary.Keys;

        public LuaValueObject this[object key]
        {
            get => dictionary[key];
        }
    }
}
