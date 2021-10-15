using System;
using Model.Data.Property;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widgets
{
    public class AudioSettingWidget: MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Text _value;

        private FloatPersistentProperty _model;
        
        private void Start()
        {
            _slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float value)
        {
            _model.Value = value;
        }

        public void SetModel(FloatPersistentProperty model)
        {
            _model = model;
            model.OnChanged += OnValueChanged;
            OnValueChanged(model.Value, model.Value);
        }

        private void OnValueChanged(float newvalue, float oldvalue)
        {
            var textValue = Mathf.Round(newvalue * 100);
            _value.text = textValue.ToString();
            _slider.normalizedValue = newvalue;
        }

        private void OnDestroy()
        {
            _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
            _model.OnChanged -= OnValueChanged;
        }
    }
}