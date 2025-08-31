using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public interface IObjectPool<T> where T : MonoBehaviour, IObjectPool<T>
{
    public int poolID { get; set; }
    public ObjectPooler<T> pool { get; set; }
}

public class ObjectPooler<T> where T : MonoBehaviour, IObjectPool<T>
{
    public T[] instances;
    protected Stack<int> freeIndex;

    public void Initialize(Object spawner, int count, T prefab, Transform parent = null)
    {
        instances = new T[count];
        freeIndex = new Stack<int>(capacity: count);

        for (int i = 0; i < count; ++i)
        {
            instances[i] = Object.Instantiate(prefab, parent);
            instances[i].name += spawner ? $"_[{spawner.name}]" : "";
            instances[i].gameObject.SetActive(false);
            instances[i].poolID = i;
            instances[i].pool = this;

            freeIndex.Push(i);
        }
    }

    public T GetNew()
    {
        int idx = freeIndex.Pop();
        instances[idx].gameObject.SetActive(true);

        return instances[idx];
    }

    public void Free(T obj)
    {
        freeIndex.Push(obj.poolID);
        instances[obj.poolID].gameObject.SetActive(false);
    }

    public IEnumerator Free(T obj, float freeTime, UnityAction callback = null)
    {
        yield return new WaitForSeconds(freeTime);
        Free(obj);
        callback?.Invoke();
    }

    public void Destroy()
    {
        for (int i = 0; i < instances.Length; i++)
        {
            if (instances[i] != null)
            {
                Object.Destroy(instances[i].gameObject);
            }
        }

        instances = null;
        freeIndex.Clear();
    }
}