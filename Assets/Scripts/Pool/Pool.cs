using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T: MonoBehaviour
{
    private List<T> _pool;
    
    public T Prefab { get; }
    public bool AutoExpand { get; set; }
    public Transform Container { get; }

    public Pool(T prefab, int count)
    {
        Prefab = prefab;
        Container = null;
        
        CreatePool(count);
    }
    
    public Pool(T prefab, int count, Transform container)
    {
        Prefab = prefab;
        Container = container;
        
        CreatePool(count);
    }
    
    public bool HasFreeElement(out T element)
    {
        foreach (var mono in _pool)
        {
            if (!mono.gameObject.activeInHierarchy)
            {
                element = mono;
                mono.gameObject.SetActive(true);
                return true;
            }
        }
        
        element = null;
        return false;
    }
    
    public T GetFreeElement()
    {
        if (HasFreeElement(out var element))
            return element;
        
        if (AutoExpand)
            return CreateObject(true);

        return null;
    }
    
    private void CreatePool(int count)
    {
        _pool = new List<T>();
        
        for (int i = 0; i < count; i++)
        {
            CreateObject();
        }
    }
    
    private T CreateObject(bool isActiveBuDefault = false)
    {
        var createdObject = Object.Instantiate(this.Prefab, this.Container);
        createdObject.gameObject.SetActive(isActiveBuDefault);
        _pool.Add(createdObject);
        
        return createdObject;
    }
}