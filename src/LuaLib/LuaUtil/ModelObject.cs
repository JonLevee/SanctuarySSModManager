using SanctuarySSModManager.Extensions;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace SanctuarySSLib.LuaUtil
{
    public class ModelObject : IDictionary<object, ModelObject>
    {
        public static ModelObject Null => new ModelObject();
        private readonly OrderedDictionary dictionary;
        public object Value { get; set; }

        public string Text => (string)Value;

        public long Long => (long)Value;

        public ICollection<object> Keys => dictionary.Keys.ToCollection<object>();

        public ICollection<ModelObject> Values => dictionary.Values.ToCollection< ModelObject>();

        public int Count => dictionary.Count;

        public bool IsReadOnly => dictionary.IsReadOnly;

        public ModelObject this[object key]
        {
            get => (ModelObject)dictionary[key];
            set => dictionary[key] = value;
        }


        public ModelObject()
        {
            dictionary = new OrderedDictionary();
            Value = null;
        }

        public void Add(object key, ModelObject value)
        {
            dictionary.Add(key, value);
        }

        public bool ContainsKey(object key)
        {
            return dictionary[key] != null;
        }

        public bool Remove(object key)
        {
            var success = ContainsKey(key);
            dictionary.Remove(key);
            return success;
        }

        public bool TryGetValue(object key, [MaybeNullWhen(false)] out ModelObject value)
        {
            value = this[key];
            return value != null;
        }

        public void Add(KeyValuePair<object, ModelObject> item)
        {
            dictionary.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            dictionary.Clear();
        }

        public bool Contains(KeyValuePair<object, ModelObject> item)
        {
            return ContainsKey(item.Key) && item.Value == this[item.Key];
        }

        public void CopyTo(KeyValuePair<object, ModelObject>[] array, int arrayIndex)
        {
            foreach(DictionaryEntry kv in dictionary)
            {
                var newKv = new KeyValuePair<object, ModelObject>(kv.Key, (ModelObject)kv.Value);
                array.SetValue(newKv, arrayIndex);
                ++arrayIndex;
            }
        }

        public bool Remove(KeyValuePair<object, ModelObject> item)
        {
            var success = ContainsKey((object)item.Key);
            dictionary.Remove(item.Key);
            return success;
        }

        public IEnumerator<KeyValuePair<object, ModelObject>> GetEnumerator()
        {
            return Keys.Select(k => new KeyValuePair<object, ModelObject>(k, this[k])).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)dictionary).GetEnumerator();
        }
    }
}
