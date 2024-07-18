using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace LuaParserUtil.LuaObjects
{
    [DebuggerDisplay("Count:{dictionary.Count}")]
    public class LuaDictionary : LuaValueObject, IDictionary<object, LuaValueObject>
    {
        public static readonly LuaDictionary Null = new LuaDictionary();
        private Dictionary<object, LuaValueObject> dictionary = new Dictionary<object, LuaValueObject>();
        public LuaDictionary()
        {
        }

        public LuaValueObject this[object key] 
        { 
            get => ((IDictionary<object, LuaValueObject>)dictionary)[key]; 
            set => ((IDictionary<object, LuaValueObject>)dictionary)[key] = value; 
        }

        public int ArrayLength { get; set; }

        public ICollection<object> Keys => ((IDictionary<object, LuaValueObject>)dictionary).Keys;

        public ICollection<LuaValueObject> Values => ((IDictionary<object, LuaValueObject>)dictionary).Values;

        public int Count => ((ICollection<KeyValuePair<object, LuaValueObject>>)dictionary).Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<object, LuaValueObject>>)dictionary).IsReadOnly;

        public void Add(LuaValueObject value)
        {
            dictionary.Add(++ArrayLength, value);
        }
        public void Add(LuaKeyValue keyValue)
        {
            dictionary.Add(keyValue.Key, keyValue.Value);
        }

        public void Add(object key, LuaValueObject value)
        {
            ((IDictionary<object, LuaValueObject>)dictionary).Add(key, value);
        }

        public void Add(KeyValuePair<object, LuaValueObject> item)
        {
            ((ICollection<KeyValuePair<object, LuaValueObject>>)dictionary).Add(item);
        }

        public void Clear()
        {
            ((ICollection<KeyValuePair<object, LuaValueObject>>)dictionary).Clear();
        }

        public bool Contains(KeyValuePair<object, LuaValueObject> item)
        {
            return ((ICollection<KeyValuePair<object, LuaValueObject>>)dictionary).Contains(item);
        }

        public bool ContainsKey(object key)
        {
            return ((IDictionary<object, LuaValueObject>)dictionary).ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<object, LuaValueObject>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<object, LuaValueObject>>)dictionary).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<object, LuaValueObject>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<object, LuaValueObject>>)dictionary).GetEnumerator();
        }

        public bool Remove(object key)
        {
            return ((IDictionary<object, LuaValueObject>)dictionary).Remove(key);
        }

        public bool Remove(KeyValuePair<object, LuaValueObject> item)
        {
            return ((ICollection<KeyValuePair<object, LuaValueObject>>)dictionary).Remove(item);
        }

        public bool TryGetValue(object key, [MaybeNullWhen(false)] out LuaValueObject value)
        {
            return ((IDictionary<object, LuaValueObject>)dictionary).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)dictionary).GetEnumerator();
        }
    }
}
