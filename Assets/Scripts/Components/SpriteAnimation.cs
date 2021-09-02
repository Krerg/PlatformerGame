using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimation : MonoBehaviour
    {
        [SerializeField] private int _framerate;
        [SerializeField] private UnityEvent<string> _onComplete;
        [SerializeField] private AnimationClip[] _clips;
        
        private SpriteRenderer _renderer;

        private float _secondsPerFrame;
        private float _nextFrameTime;
        private int _currentSprite;
        private bool _isPlaying = true;

        private int _currentClip;
        
        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _secondsPerFrame = 1f / _framerate;
            StartAnimation();
        }
        
        private void OnBecameVisible()
        {
            enabled = _isPlaying;
        }

        private void OnBecameInvisible()
        {
            enabled = false;
        }

        public void SetClip(string clipName)
        {
            for (int i = 0; i < _clips.Length; i++)
            {
                if (_clips[i].Name == clipName)
                {
                    _currentClip = i;
                    StartAnimation();
                    return;
                }
            }

            enabled = _isPlaying = false;
        }

        private void StartAnimation()
        {
            _nextFrameTime = Time.time;
            enabled = _isPlaying = true;
            _currentSprite = 0;
        }
        
        private void OnEnable()
        {
            _nextFrameTime = Time.time;
        }

        void Update()
        {
            if (_nextFrameTime > Time.time) return;

            var clip = _clips[_currentClip];

            if (_currentSprite >= clip.Sprites.Length)
            {
                if (clip.Loop)
                {
                    _currentSprite = 0;
                }
                else
                {
                    enabled = _isPlaying = clip.AllowNextClip;
                    clip.OnComplete?.Invoke();
                    _onComplete?.Invoke(clip.Name);
                    if (clip.AllowNextClip)
                    {
                        _currentSprite = 0;
                        _currentClip = (int) Mathf.Repeat(_currentClip + 1, _clips.Length);
                    }
                }

                return;
            }

            _renderer.sprite = clip.Sprites[_currentSprite];
            _nextFrameTime += _secondsPerFrame;
            _currentSprite++;

        }
    }

    [Serializable]
    public class AnimationClip
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private bool _loop;
        [SerializeField] private bool _allowNextClip;
        [SerializeField] private UnityEvent _onComplete;

        public string Name => _name;
        public Sprite[] Sprites => _sprites;
        public bool Loop => _loop;
        public bool AllowNextClip => _allowNextClip;
        public UnityEvent OnComplete => _onComplete;
        
    }
}