using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemiurgEngine
{
    public static class EventBus
    {
        static Dictionary<Type, List<object>> _actionsDictionary = new();
        public static void Subscribe<T>(Action<T> action)
        {
            Type type = typeof(T);
            if (_actionsDictionary.ContainsKey(type))
            {
                _actionsDictionary[type].Add(action);
            }
            else
            {
                _actionsDictionary.Add(type, new List<object>() { action });
            }
        }
        public static void Unsubscribe<T>(Action<T> action)
        {
            Type type = typeof(T);
            if (_actionsDictionary.ContainsKey(type))
            {
                _actionsDictionary[type].Remove(action);
            }
        }
        public static void Publish<T>(T e)
        {
            Type type = typeof(T);
            if (_actionsDictionary.ContainsKey(type))
            {
                foreach (var item in _actionsDictionary[type])
                {
                    ((Action<T>)item).Invoke(e);
                }
            }
        }
    }
}