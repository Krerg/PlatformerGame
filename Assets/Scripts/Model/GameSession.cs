using UnityEngine;

namespace PixelCrew.Model
{
    public class GameSession: MonoBehaviour
    {

        [SerializeField] private PlayerData _data;

        public PlayerData Data => _data;

    }
}