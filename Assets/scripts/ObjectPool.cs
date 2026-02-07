using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour where T : Component
{
    [SerializeField] protected T _objPrefab;
    protected Queue<T> _pool = new Queue<T>();
    public T Get()
    {
        if (_pool == null)
        {
            _pool = new Queue<T>();
        }
        if (_pool.Count > 0)
        {
            T obj = _pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            return Instantiate(_objPrefab);
        }
    }
    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        _pool.Enqueue(obj);
    }
}
