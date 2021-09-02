using PixelCrew.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Level
{
    public class ReloadLevelComponent : MonoBehaviour
    {
        public void OnOutOfLevel()
        {
            DestroyCurrentSession();
            var currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }

        private void DestroyCurrentSession()
        {
            var session = FindObjectOfType<GameSession>();
            Destroy(session.gameObject);
        }
    }
}