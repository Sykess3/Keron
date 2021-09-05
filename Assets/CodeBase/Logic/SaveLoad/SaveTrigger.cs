using CodeBase.Infrastructure.Services.SaveLoad;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic.SaveLoad
{
    [RequireComponent(typeof(Collider))]
    public class SaveTrigger : MonoBehaviour
    {
        [SerializeField] private BoxCollider _collider;
        private ISaveLoadService _saveLoadService;
        private bool _triggered;

        [Inject]
        private void Construct(ISaveLoadService saveLoadService) =>
            _saveLoadService = saveLoadService;

        private void OnTriggerEnter(Collider other)
        {
            if (_triggered)
                return;
            Save();
            
            Debug.Log("Saved");
        }

        private void Save()
        {
            _saveLoadService.SaveProgress();
            gameObject.SetActive(false);
            _triggered = true;
        }

        private void OnDrawGizmos()
        {
            if (_collider == null)
                return;
            
            Gizmos.color = new Color32(30, 200, 30, 130);

            Gizmos.DrawCube(transform.position + _collider.center, _collider.size);
        }
    }
}