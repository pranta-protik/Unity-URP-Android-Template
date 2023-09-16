using System;
using System.Collections.Generic;
using Toolbox.Utilities;
using UnityEngine;

namespace Project.Persistent.SaveSystem
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<TKey> _keysList = new();
        [SerializeField] private List<TValue> _valuesList = new();

        // Save the dictionary to lists
        public void OnBeforeSerialize()
        {
            _keysList.Clear();
            _valuesList.Clear();

            foreach (var keyValuePair in this)
            {
                _keysList.Add(keyValuePair.Key);
                _valuesList.Add(keyValuePair.Value);
            }
        }

        // Load the dictionary from lists
        public void OnAfterDeserialize()
        {
            this.Clear();

            if (_keysList.Count != _valuesList.Count)
            {
                DebugUtils.LogError("Tried to deserialize a SerializableDictionary, but the amount of keys ("
                + _keysList.Count + ") does not match the number of values ("
                + _valuesList.Count + ") which indicates that something went wrong.");

            }

            for (var i = 0; i < _keysList.Count; i++)
            {
                this.Add(_keysList[i], _valuesList[i]);
            }
        }
    }
}
