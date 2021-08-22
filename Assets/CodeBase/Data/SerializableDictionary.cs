using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.Data
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver, IEnumerable<KeyValuePair<TKey, TValue>>
    {
        [SerializeField] private TKey[] _keys;
        [SerializeField] private TValue[] _values;

        protected readonly Dictionary<TKey, TValue> Dictionary = new Dictionary<TKey, TValue>();

        public TValue this[TKey key] => 
            Dictionary[key];

        public bool Contains(TKey id) => 
            Dictionary.ContainsKey(id);

        public void Add(TKey key, TValue value) => 
            Dictionary.Add(key, value);

        public void SafeRemove(TKey Id)
        {
            if (Contains(Id)) 
                Remove(Id);
        }

        public int Count() => 
            Dictionary.Count;

        private void Remove(TKey id) => 
            Dictionary.Remove(id);

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            _keys = Dictionary.Keys.ToArray();
            _values = Dictionary.Values.ToArray();
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            for (int i = 0; i < _keys.Length; i++) 
                Dictionary.Add(_keys[i], _values[i]);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => 
            Dictionary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}