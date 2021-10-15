using UnityEngine;

namespace Model.Data.Property
{
    public class ObservableProperty<TPropertyType>
    {
        [SerializeField] private TPropertyType _value;

        public delegate void OnPropertyChanged(TPropertyType newValue, TPropertyType oldValue);

        public event OnPropertyChanged OnChanged;
        
        public TPropertyType Value
        {
            get => _value;
            set
            {
                var isSame = _value.Equals(value);
                if (isSame) return;
                var oldValue = _value;
                _value = value;
                OnChanged?.Invoke(_value, oldValue);
            }
        }
        
        
    }
}