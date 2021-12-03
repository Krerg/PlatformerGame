using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.MainMenu
{
    public class MainMenuWindow : AnimatedWindow
    {
        private Action _closeAction;
        
        public void OnShowSettings()
        {
            var window = Resources.Load<GameObject>("UI/SettingsWindow");
            var canvas = FindObjectOfType<Canvas>();
            Instantiate(window, canvas.transform);
        }

        public void OnStartGame()
        {
            _closeAction = () =>
            {
                var loader = FindObjectOfType<LevelLoader.LevelLoader>();
                loader.LoadLevel("Level1");
            };
            Close();
        }

        public void OnLanguages()
        {
            var window = Resources.Load<GameObject>("UI/LocalizationWindow");
            var canvas = FindObjectOfType<Canvas>();
            Instantiate(window, canvas.transform);
        }
        
        public void OnExit()
        {
            
            _closeAction = () =>
            {
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            };
            Close();
        }

        public override void OnCloseAnimationComplete()
        {
            _closeAction?.Invoke();
            base.OnCloseAnimationComplete();
        }
    }
}