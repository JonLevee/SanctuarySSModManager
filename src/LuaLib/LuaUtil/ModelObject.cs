using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace SanctuarySSLib.LuaUtil
{
    public class ModelObject : IDictionary<string, ModelObject>
    {
        public static ModelObject Null => new ModelObject();
        private readonly OrderedDictionary dictionary;
        public object Value { get; set; }

        public ICollection<string> Keys => (ICollection<string>)dictionary.Keys;

        public ICollection<ModelObject> Values => (ICollection<ModelObject>)dictionary.Values;

        public int Count => dictionary.Count;

        public bool IsReadOnly => dictionary.IsReadOnly;

        public ModelObject this[string key]
        {
            get => (ModelObject)dictionary[key];
            set => dictionary[key] = value;
        }

        public ModelObject()
        {
            dictionary = new OrderedDictionary();
            Value = null;
        }

        public void Add(string key, ModelObject value)
        {
            dictionary.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return dictionary[key] != null;
        }

        public bool Remove(string key)
        {
            var success = ContainsKey(key);
            dictionary.Remove(key);
            return success;
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out ModelObject value)
        {
            value = this[key];
            return value != null;
        }

        public void Add(KeyValuePair<string, ModelObject> item)
        {
            dictionary.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            dictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, ModelObject> item)
        {
            return ContainsKey(item.Key) && item.Value == this[item.Key];
        }

        public void CopyTo(KeyValuePair<string, ModelObject>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
            ((ICollection<KeyValuePair<string, ModelObject>>)dictionary).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, ModelObject> item)
        {
            throw new NotImplementedException();
            return ((ICollection<KeyValuePair<string, ModelObject>>)dictionary).Remove(item);
        }

        public IEnumerator<KeyValuePair<string, ModelObject>> GetEnumerator()
        {
            return Keys.Select(k => new KeyValuePair<string, ModelObject>(k, this[k])).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)dictionary).GetEnumerator();
        }
    }
}
