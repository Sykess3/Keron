using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Data
{
    [Serializable]
    public class SerializableQueue<T> : ISerializationCallbackReceiver
    {
        public readonly Queue<T> Queue = new Queue<T>();

        [SerializeField] private T[] array;

        public void OnBeforeSerialize() =>
            array = Queue.ToArray();

        public void OnAfterDeserialize()
        {
            foreach (var member in array) 
                Queue.Enqueue(member);
        }
    }
}