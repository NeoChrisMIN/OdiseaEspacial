using UnityEngine;
using System;
using System.Collections;

public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static UnityMainThreadDispatcher instance = null;

    private Queue actionQueue = new Queue();
    private object queueLock = new object();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        lock (queueLock)
        {
            while (actionQueue.Count > 0)
            {
                Action action = (Action)actionQueue.Dequeue();
                action.Invoke();
            }
        }
    }

    public static void RunOnMainThread(Action action)
    {
        lock (instance.queueLock)
        {
            instance.actionQueue.Enqueue(action);
        }
    }
}
