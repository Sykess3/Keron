using CodeBase.Data;
using CodeBase.Infrastructure;
using CodeBase.Services;
using CodeBase.Services.Input;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroMovement : MonoBehaviour, ISavedProgressCleanable
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private HeroAnimator _animator;

        [SerializeField] private float _speed;
        private IInputService _inputService;

        [Inject]
        private void Construct(IInputService inputService) => 
            _inputService = inputService;

        private void Update()
        {
            Vector3 movementDirection = Vector3.zero;

            if (_inputService.Axis.magnitude > Constants.Epsilon)
            {
                movementDirection = GetMovementDirection();
                movementDirection.Normalize();
                transform.forward = movementDirection;
            }

            movementDirection += Physics.gravity;

            _characterController.Move(GetVelocity(movementDirection));
            _animator.Move(speed: _characterController.velocity.magnitude);
            
        }

        public void LoadProgress(PlayerProgress @from)
        {
            if (IsCurrentLevel())
            {
                Vector3Data savedPosition = @from.WorldData.PositionOnLevel.Position;
                if (savedPosition != null) 
                    Warp(to: savedPosition);
            }
            
            bool IsCurrentLevel() => 
                CurrentLevel() == @from.WorldData.PositionOnLevel.Level;
        }

        public void UpdateProgress(ref PlayerProgress to) =>
            to.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(), transform.position.AsVector3Data());

        private void Warp(Vector3Data to)
        {
            _characterController.enabled = false;
            transform.position = to.AsUnityVector3().AddY(_characterController.height);
            _characterController.enabled = true;
        }

        private Vector3 GetMovementDirection() => 
            new Vector3(_inputService.Axis.x, 0, _inputService.Axis.y);

        private Vector3 GetVelocity(Vector3 movementDirection) => 
            movementDirection * (_speed * Time.deltaTime);

        private static string CurrentLevel() => 
            SceneManager.GetActiveScene().name;
    }
}