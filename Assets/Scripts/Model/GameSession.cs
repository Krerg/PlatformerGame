using System;
using Model.Data;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Model
{
    public class GameSession: MonoBehaviour
    {

        [SerializeField] private PlayerData _data;

        public PlayerData Data => _data;
        private PlayerData _save;
        public QuickInventoryModel QuickInventory { get; private set; }

        private readonly CompositeDisposable _trash = new CompositeDisposable();


        private void Awake()
        {
            LoadHud();
            if (IsSessionExists())
            {
                Destroy(gameObject);
            }
            else
            {
                Save();
                InitModels();
                DontDestroyOnLoad(this);
            }
        }

        private void InitModels()
        {
            QuickInventory = new QuickInventoryModel(Data);
            _trash.Retain(QuickInventory);
            
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
        
        public void Save()
        {
            _save = _data.Clone();
        }

        public void LoadLastSave()
        {
            _data = _save.Clone();
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}