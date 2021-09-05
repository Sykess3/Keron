using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Elements
{
    public class OpenWindowButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private WindowId _windowId;
        
        private IWindowsService _windowsService;

        [Inject]
        private void Construct(IWindowsService windowsService)
        {
            _windowsService = windowsService;
        }

        private void Awake()
        {
            _button.onClick.AddListener(Open);        
        }

        private void Open() => 
            _windowsService.Open(_windowId);
    }
}