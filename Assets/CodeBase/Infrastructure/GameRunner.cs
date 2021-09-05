using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        private const string Initial = "Initial";

        [RuntimeInitializeOnLoadMethod]
        private static void LoadBootScene() => 
            SceneManager.LoadScene(Initial);
    }
}