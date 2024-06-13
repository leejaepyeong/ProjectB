using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using System;
using Object = UnityEngine.Object;
public class ResourcePool
{
    private record AssetReferenceCount(int Count)
    {
        public AsyncOperationHandle Handle;
        public int Count { get; set; } = Count;
    }

    private Dictionary<object, AssetReferenceCount> dicAssetReferenceCount;
    private Dictionary<int, object> dicAssetKey;

    public ResourcePool()
    {
        dicAssetReferenceCount = new();
        dicAssetKey = new();
    }

    ~ResourcePool() 
    {
        if (dicAssetReferenceCount != null)
            UnLoadAll();

        dicAssetReferenceCount = null;

        dicAssetKey.Clear();
        dicAssetKey = null;
    }

    #region Load

    private void InternalCacheOperation(object key, in AsyncOperationHandle handle)
    {
        if(!dicAssetReferenceCount.TryGetValue(key, out var referenceCount))
        {
            referenceCount = new(0);
            dicAssetReferenceCount.Add(key, referenceCount);
        }

        referenceCount.Handle = handle;
        ++referenceCount.Count;
        dicAssetReferenceCount[key] = referenceCount;
    }

    private void InternalCacheKey(Object asset, object key)
    {
        var instanceId = asset.GetInstanceID();
        dicAssetKey.TryAdd(instanceId, key);
    }


    public E Load<E>(object key) where E : Object
    {
        if(key == null)
        {
            Debug.LogError(new NullReferenceException());
            return null;
        }

        try
        {
            var handle = Addressables.LoadAssetAsync<E>(key);
            handle.WaitForCompletion();
            if (!handle.IsValid()) return null;

            InternalCacheOperation(key, handle);
            InternalCacheKey(handle.Result, key);
            return handle.Result;
        }
        catch(Exception e)
        {
            Debug.LogError(e);
            return null;
        }
    }

    public bool TryLoad<E>(object key, out E obj) where E : Object
    {
        obj = null;
        if (key == null) return false;

        try
        {
            var handle = Addressables.LoadAssetAsync<E>(key);
            handle.WaitForCompletion();
            if (!handle.IsValid()) return false;

            InternalCacheOperation(key, handle);
            InternalCacheKey(handle.Result,key);

            obj = handle.Result;
            return true;
        }
        catch(Exception e)
        {
            Debug.LogError(e);
            return false;
        }
    }
    #endregion

    #region UnLoad
    private void InternalRemoveInvalidHandle(object key)
    {
        dicAssetReferenceCount.Remove(key);
    }

    private int InternalDecacheOperation(object key, in AsyncOperationHandle handle)
    {
        if (!dicAssetReferenceCount.TryGetValue(key, out var referenceCount)) return -1;

        referenceCount.Handle = handle;
        --referenceCount.Count;
        if (referenceCount.Count <= 0)
            dicAssetReferenceCount.Remove(key);
        else
            dicAssetReferenceCount[key] = referenceCount;

        return referenceCount.Count;
    }
    private void InternalUnCacheKey(Object asset)
    {
        var instanceId = asset.GetInstanceID();
        dicAssetKey.Remove(instanceId);
    }

    public int UnLoad(object key)
    {
        if (key == null) return -1;
        if (!dicAssetReferenceCount.TryGetValue(key, out var referenceCount)) return -1;
        if(!referenceCount.Handle.IsValid())
        {
            InternalRemoveInvalidHandle(key);
            return -1;
        }

        if (referenceCount.Count == 0)
            throw new Exception($"{key} Ref Count == 0");

        int count = referenceCount.Count;
        var handle = referenceCount.Handle;

        InternalUnCacheKey(handle.Result as Object);
        Addressables.Release(handle);
        return InternalDecacheOperation(key, handle);
    }

    public void UnLoadAll()
    {
        var enumerator = dicAssetReferenceCount.GetEnumerator();
        while(enumerator.MoveNext())
        {
            var element = enumerator.Current;
            var key = element.Key;
            var referenceCount = element.Value;
            var count = referenceCount.Count;
            var handle = referenceCount.Handle;

            if(referenceCount.Count == 0)
                throw new Exception($"{key} Ref Count == 0");

            for (int i = 0; i < count; i++)
            {
                Addressables.Release(handle);
            }
        }
        dicAssetReferenceCount.Clear();
        dicAssetKey.Clear();
    }


    #endregion
}
