using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew
{
    public class ExitLevelComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneName;

        // Start is called before the first frame update
        public void Exit()
        {
            SceneManager.LoadScene(_sceneName);
        }
    }
}