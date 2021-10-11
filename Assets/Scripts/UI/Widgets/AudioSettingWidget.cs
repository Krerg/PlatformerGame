using Model.Data.Property;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widgets
{
    public class AudioSettingWidget: MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Text _value;

        public void SetModel(FloatPersistentProperty model)
        {
            model.OnChanged += OnValueChanged;
        }

        private void OnValueChanged(float newvalue, float oldvalue)
        {
            var textValue = Mathf.Round(newvalue * 100);
            _value.text = textValue.ToString();
        }
    }
}