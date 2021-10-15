using System;
using Model.Data;
using Model.Data.Property;
using UnityEngine;

namespace Components.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSettingComponent: MonoBehaviour
    {
        [SerializeField] private SoundSettingKey _mode;
        private FloatPersistentProperty _model;
        private AudioSource _source;
        private void Start()
        {
            _source = GetComponent<AudioSource>();
            _model = FindProperty();
            _model.OnChanged += OnSettingChanged;
            OnSettingChanged(_model.Value, _model.Value);
        }

        private void OnSettingChanged(float newvalue, float oldvalue)
        {
            _source.volume = newvalue;
        }

        private FloatPersistentProperty FindProperty()
        {
            switch (_mode)
            {
                case SoundSettingKey.Music:
                    return GameSettings.I.Music;
                case SoundSettingKey.Sfx:
                    return GameSettings.I.Sfx;
            }

            throw new ArgumentException("Undefined mode");
        }

        private void OnDestroy()
        {
            _model.OnChanged -= OnSettingChanged;
        }
    }
}