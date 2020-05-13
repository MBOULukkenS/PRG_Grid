using System;
using UnityEngine;

namespace Util
{
    public class CachedComponent<T> where T : Component
    {
        private readonly GameObject _gameObject;
        private T _cachedComponent;
        
        public CachedComponent(GameObject gameObject)
        {
            _gameObject = gameObject;
        }
        
        public CachedComponent(T component)
        {
            _cachedComponent = component;
        }

        public T Component
        {
            get
            {
                if (!_cachedComponent)
                {
                    if (!_gameObject)
                        throw new InvalidOperationException("Either CachedComponent or GameObject should be set!");
                    _cachedComponent = _gameObject.GetComponent<T>();
                }

                return _cachedComponent;
            }
        }
    }
}