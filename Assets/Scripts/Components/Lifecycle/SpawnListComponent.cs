using System;
using System.Linq;
using UnityEngine;

namespace Components.Lifecycle
{
    public class SpawnListComponent: MonoBehaviour
    {

        [SerializeField] private SpawnData[] _spawners;

        public void Spawn(string id)
        {
            _spawners.FirstOrDefault(x => x.Id == id)?.Component.Spawn();
        }
        

        [Serializable]
        public class SpawnData
        {
            public string Id;
            public SpawnComponent Component;
        }
    }
    
    
}