using System;
using System.Collections;
using Model.Data;
using PixelCrew.Model.Definitions.Localization;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud.Dialogs
{
    public class DialogBoxController : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private GameObject _container;
        [SerializeField] private Animator _animator;

        [SerializeField] private bool localize = true;

        [Space] [SerializeField] private float _textSpeed = 0.09f;
        [Header("Sounds")] [SerializeField] private AudioClip _typing;
        [SerializeField] private AudioClip _open;
        [SerializeField] private AudioClip _close;

        private static readonly int IsOpen = Animator.StringToHash("isOpen");

        private DialogData _data;
        private int _currentSentence;
        private AudioSource _sfxSource;

        private Coroutine _typingCoroutine;

        private void Start()
        {
            _sfxSource = AudioUtils.FindSfxSource();
        }

        public void OnSkip()
        {
            if (_typingCoroutine == null) return;
            StopTypeAnimation();
        }

        private void StopTypeAnimation()
        {
            if (_typingCoroutine != null)
                StopCoroutine(_typingCoroutine);
            _typingCoroutine = null;
            _text.text = _data.Sentences[_currentSentence];
        }

        public void OnContinue()
        {
            StopTypeAnimation();
            _currentSentence++;
            var isDialogComplete = _currentSentence >= _data.Sentences.Length;
            if (isDialogComplete)
            {
                HideDialogBox();
            }
            else
            {
                OnStartDialogAnimation();
            }
        }

        private void HideDialogBox()
        {
            _animator.SetBool(IsOpen, false);
            _sfxSource.PlayOneShot(_close);
        }

        private void OnStartDialogAnimation()
        {
            _typingCoroutine = StartCoroutine(TypeDialogText());
        }

        private IEnumerator TypeDialogText()
        {
            _text.text = string.Empty;
            var sentence = localize
                ? LocalizationManager.I.Localize(_data.Sentences[_currentSentence])
                : _data.Sentences[_currentSentence];
            foreach (var letter in sentence)
            {
                _text.text += letter;
                _sfxSource.PlayOneShot(_typing);
                yield return new WaitForSeconds(_textSpeed);
            }

            _typingCoroutine = null;
        }

        private void OnCloseAnimationComplete()
        {
        }

        public void ShowDialog(DialogData data, bool localize)
        {
            this.localize = localize;
            _data = data;
            _currentSentence = 0;
            _text.text = String.Empty;

            _container.SetActive(true);
            _sfxSource.PlayOneShot(_open);
            _animator.SetBool(IsOpen, true);
        }
    }
}