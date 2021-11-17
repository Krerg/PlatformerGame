using System;
using System.Collections.Generic;
using System.Linq;
using Level;
using Model.Data;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Model
{
    public class GameSession: MonoBehaviour
    {

        [SerializeField] private PlayerData _data;

        [SerializeField] private string _defaultCheckpoint;
        
        public PlayerData Data => _data;
        private PlayerData _save;
        public QuickInventoryModel QuickInventory { get; private set; }

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private List<string> _checkoints = new List<string>();

        private void Awake()
        {
            var existSession = GetExistSession();
            if (existSession != null)
            {
                existSession.StartSession(_defaultCheckpoint);
                Destroy(gameObject);
            }
            else
            {
                Save();
                InitModels();
                DontDestroyOnLoad(this);
                StartSession(_defaultCheckpoint);
            }
        }

        private void StartSession(string _defaultCheckpoint)
        {
            SetChecked(_defaultCheckpoint);
            LoadHud();
            SpawnHero();
        }

        private void SpawnHero()
        {
            var checkpoints = FindObjectsOfType<CheckPointComponent>();
            var lastCheckpoint = _checkoints.Last();
            foreach (var checkpoint in checkpoints)
            {
                if (checkpoint.Id == lastCheckpoint)
                {
                    checkpoint.SpawnHero();
                    break;
                }
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

        private GameSession GetExistSession()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var session in sessions)
            {
                if (session != this)
                {
                    return session;
                }
            }

            return null;
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

        public bool IsCheked(string id)
        {
            return _checkoints.Contains(id);
        }

        public void SetChecked(string id)
        {
            if (!_checkoints.Contains(id))
            {
                _checkoints.Add(id);
                Save();
            }
        }
    }
}