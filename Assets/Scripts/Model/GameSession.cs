using System;
using Model.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Model
{
    public class GameSession: MonoBehaviour
    {

        [SerializeField] private PlayerData _data;

        public PlayerData Data => _data;

        private void Awake()
        {
            LoadHud();
            if (IsSessionExists())
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(this);
            }
        }

        private void LoadHud()
        {
            SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
        }

        private bool IsSessionExists()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var session in sessions)
            {
                if (session != this)
                {
                    return true;
                }
            }

            return false;
        }
    }
}