using System;
using UnityEngine;

namespace Components.Audio
{
    public class PlaySoundsComponent: MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioData[] _sounds;

        public void Play(string id)
        {
            foreach (var sound in _sounds)
            {
                if (sound.Id == id)
                {
                    if (_audioSource == null)
                        _audioSource = GameObject.FindWithTag("SfxAudioSource").GetComponent<AudioSource>();
                    
                    _audioSource.PlayOneShot(sound.Clip);
                    break;
                }
            }
        }
        
        [Serializable]
        public class AudioData
        {
            [SerializeField] private string _id;
            [SerializeField] private AudioClip _clip;

            public string Id => _id;
            public AudioClip Clip => _clip;

        }
        
    }
    
    
}