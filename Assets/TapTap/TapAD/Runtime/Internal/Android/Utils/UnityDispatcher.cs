using System;
using System.Collections.Generic;
using UnityEngine;

namespace TapTap.TapAd
{
    /// <summary>
    /// The unity thread dispatcher.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class UnityDispatcher : MonoBehaviour
    {
        private static UnityDispatcher _instance;
    
        public static UnityDispatcher Instance
        {
            get
            {
                if (_instance == null)
                {
                    CreateInstance();
                }
                return _instance;
            }
        }
    
        private static void CreateInstance()
        {
            Type theType = typeof(UnityDispatcher);
    
            _instance = (UnityDispatcher)FindObjectOfType(theType);
    
            if (_instance == null)
            {
                var go = new GameObject(typeof(UnityDispatcher).Name);
                _instance = go.AddComponent<UnityDispatcher>();
    
                GameObject rootObj = GameObject.Find("TapSDKSingletons");
                if (rootObj == null)
                {
                    rootObj = new GameObject("TapSDKSingletons");
                    DontDestroyOnLoad(rootObj);
                }
                go.transform.SetParent(rootObj.transform);
            }
        }
    
        public static void DestroyInstance()
        {
            if (_instance != null)
            {
                Destroy(_instance.gameObject);
            }
        }
    
        public static bool HasInstance()
        {
            return _instance != null;
        }
        
        private void Awake()
        {
            if (_instance != null && _instance.gameObject != gameObject)
            {
                if (Application.isPlaying)
                {
                    Destroy(gameObject);
                }
                else
                {
                    DestroyImmediate(gameObject); // UNITY_EDITOR
                }
                return;
            }
            else if (_instance == null)
            {
                _instance = GetComponent<UnityDispatcher>();
            }
    
            DontDestroyOnLoad(gameObject);
        }
    
        private void OnDestroy()
        {
            Uninit();
    
            if (_instance != null && _instance.gameObject == gameObject)
            {
                _instance = null;
            }
        }
        
        public void Uninit()
        {
            _postTasks.Clear();
            _executing.Clear();
        }
            
        // The thread safe task queue.
        private static List<Action> _postTasks = new List<Action>();

        // The _executing buffer.
        private static List<Action> _executing = new List<Action>();
        
        /// <summary>
        /// Work thread post a task to the main thread.
        /// </summary>
        public void PostTask(Action task, bool executeOnMainThread = true)
        {
            if (executeOnMainThread)
            {
                lock (_postTasks)
                {
                    _postTasks.Add(task);
                }
            }
            else
            {
                task?.Invoke();
            }
        }
        
        private void Update()
        {
            lock (_postTasks)
            {
                if (_postTasks.Count > 0)
                {
                    for (int i = 0; i < _postTasks.Count; ++i)
                    {
                        _executing.Add(_postTasks[i]);
                    }

                    _postTasks.Clear();
                }
            }

            for (int i = 0; i < _executing.Count; ++i)
            {
                var task = _executing[i];
                try
                {
                    task();
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message + "\n" + e.StackTrace, this);
                }
            }

            _executing.Clear();
        }
    }
}
