using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class ServiceLocator
    {
        private static ServiceLocator _instance;
        public static ServiceLocator Instance => _instance ??= new ServiceLocator();
        private readonly Dictionary<Type, object> _services;

        public ServiceLocator()
        {
            _services = new Dictionary<Type, object>();
        }

        public void Register<T>(object service)
        {
            var type = typeof(T);

            if (_services.ContainsKey(type))
            {
                _services[type] = service;
                return;
            }

            _services.Add(type, service);
        }

        public T Get<T>()
        {
            var type = typeof(T);
            if (!_services.TryGetValue(type, out var service))
            {
                throw new Exception($"Service of type {type} not found");
            }

            return (T)service;
        }
    }
}