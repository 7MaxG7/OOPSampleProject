using System;
using UnityEngine;

namespace Configs.Data
{
    [Serializable]
    public class SpawnPosition
    {
        [SerializeField] private string _sceneName;
        [SerializeField] private Vector3 _position;
        [SerializeField] private Quaternion _rotation;

        public string SceneName => _sceneName;
        public Vector3 Position => _position;
        public Quaternion Rotation => _rotation;

        
        public SpawnPosition(string sceneName)
        {
            _sceneName = sceneName;
        }

        public void UpdatePosition(Transform markerTransform)
        {
            _position = markerTransform.position;
            _rotation = markerTransform.rotation;
        }
    }
}