using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Windows
{
    public abstract class WindowBase : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        protected PlayerProgress Progress { get; private set; }

        private void Awake()
        {
            _closeButton.onClick.AddListener(OnAwake);
        }

        [Inject]
        private void Construct(IPersistentProgressService progressService)
        {
            Progress = progressService.Progress;
        }

        private void Start()
        {
            Initialize();
            SubscribeUpdate();
        }

        private void OnDestroy()
        {
            Cleanup();
        }

        protected virtual void OnAwake() => 
            Destroy(gameObject);

        protected abstract void Initialize();
        protected abstract void SubscribeUpdate();
        protected abstract void Cleanup();
    }
}