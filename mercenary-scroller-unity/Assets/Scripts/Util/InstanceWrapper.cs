using System;
using UnityEngine;

namespace hinos.utility
{
    public abstract class InstanceWrapper {
        protected GameObject _sourceObject;

        public GameObject SourceObject {
            get => _sourceObject;
        }

        public InstanceWrapper(GameObject sourceObject) {
            _sourceObject = sourceObject;

            ProcessGetComponents();
        }

        protected abstract void ProcessGetComponents();

        protected T1 GetComponentOnSource<T1>() where T1 : MonoBehaviour {
            if (!_sourceObject.TryGetComponent<T1>(out var component)) {
                throw new Exception($"[{this.GetType().Name}] source has no {typeof(T1).Name} component");
            }

            return component;
        }
    }
}
