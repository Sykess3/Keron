using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure
{
    public interface ISceneLoader
    {
        Task Load(string name, LoadSceneMode sceneMode, Action onLoaded = null);
    }
}