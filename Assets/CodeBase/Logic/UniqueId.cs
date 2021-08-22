using System;
using CodeBase.CustomAttributes;
using UnityEngine;

namespace CodeBase.Logic
{
    public class UniqueId : MonoBehaviour
    {
         [ReadOnly] public string Id;

#if UNITY_EDITOR
         public event Action<string> Generated;
#endif
         
        private void Awake()
        {
            if (string.IsNullOrEmpty(Id)) 
                Generate();
        }

        public void Generate()
        {
            string sceneName = gameObject.scene.name;
            string GuidId = Guid.NewGuid().ToString();

            Id = $"{sceneName}_{GuidId}";
#if UNITY_EDITOR
            Generated?.Invoke(sceneName);
#endif
        }


        public void Reset() => 
            Id = string.Empty;
        
        
    }
}