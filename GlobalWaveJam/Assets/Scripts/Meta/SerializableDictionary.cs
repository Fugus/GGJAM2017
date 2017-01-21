using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// SERIALIZABLE DICTIONARY
// note: not ordered, therefore not as time efficient as dictionary or tree. To use only with less accessed data for save/load purpose in Unity

[System.Serializable]
public class SerializableDictionary<K,V> : IEnumerable<KeyValuePair<K,V>>
{
    [SerializeField]
    private List<K> Keys = new List<K>();
    [SerializeField]
    public List<V> Values = new List<V>();

    public SerializableDictionary()
    {
    }


    public int Count()
    {
        return Keys.Count;
    }

    public bool Contains(K key)
    {
        return Keys.Contains(key);
    }

    public int IndexOf(K key)
    {
        return Keys.IndexOf(key);
    }


    public void Add(K key, V value)
    {
        if (Keys.Contains(key))
            return;

        Keys.Add(key);
        Values.Add(value);
    }

    public void Set(K key, V value)
    {
        if (Keys.Contains(key) == false)
            return;
        if (IndexOf(key) < 0 || IndexOf(key) > Values.Count)
            return;

        Values[IndexOf(key)] = value;
    }

    public void AddOrSet(K key, V value)
    {
        if (Keys.Contains(key) == false)
            Add(key, value);
        else
            Set(key, value);
    }

    public void Remove(K key)
    {
        if (Keys.Contains(key) == false)
            return;
        if (IndexOf(key) < 0 || IndexOf(key) > Values.Count)
            return;

        Values.RemoveAt(IndexOf(key));
        Keys.Remove(key);
    }


    public V Get(K key)
    {
        if (Contains(key) == false)
            return default(V);

        if (IndexOf(key) < 0 || IndexOf(key) > Values.Count)
            return default(V);

        return Values[IndexOf(key)];
    }


    public K KeyAt(int idx)
    {
        if (idx < 0 || idx > Keys.Count)
            return default(K);

        return Keys[idx];
    }


    public V ValueAt(int idx)
    {
        if (idx < 0 || idx > Keys.Count)
            return default(V);

        return Values[idx];
    }


    public IEnumerator<KeyValuePair<K,V>> GetEnumerator()
    {
        foreach (K key in Keys)
            yield return new KeyValuePair<K,V>(key, Get(key));
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

}
