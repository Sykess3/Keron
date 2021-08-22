using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        [SerializeField] private GameBootstrapper _gameBootstrapperPrefab;
        private void Awake()
        {
            if (FindObjectOfType<GameBootstrapper>() == null)
                Instantiate(_gameBootstrapperPrefab);
            DestroyImmediate(gameObject);
        }
    }
}