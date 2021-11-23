using System.Linq;
using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    
    [CreateAssetMenu(menuName = "Defs/PlayerDef", fileName = "PlayerDef")]
    public class PlayerDef : ScriptableObject
    {
        [SerializeField] private int _maxHealth;
        [SerializeField] private StatDef[] _stats;

        public int MaxHealth => _maxHealth;
        public StatDef[] Stats => _stats;

        public StatDef GetStat(StatId id) => _stats.FirstOrDefault(def => def.ID == id);
    }
}