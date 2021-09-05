using UnityEngine;

namespace CodeBase.Services.PersistentProgress
{
    public interface IProgressWatchers
    {
        void Register(GameObject gameObject);
    }
}