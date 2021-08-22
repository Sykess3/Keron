using UnityEngine;

namespace CodeBase.Services.Input
{
    public class SimpleInputService : IInputService
    {
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";
        private const string Button = "Fire";

        public Vector2 Axis =>
            new Vector2(SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical));

        public bool IsAttackButtonUp() =>
            SimpleInput.GetButton(Button);
    }
    
    
}