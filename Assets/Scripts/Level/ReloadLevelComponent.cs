using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using PixelCrew.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components
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