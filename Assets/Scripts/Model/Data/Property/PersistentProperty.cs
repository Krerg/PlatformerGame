using UnityEngine;

namespace Model.Data.Property
{
    public abstract class PersistentProperty<TPropertyType>
    {
        [SerializeField] private TPropertyType _value;
        private TPropertyType _stored;
        
        private TPropertyType _defaultValue;
        
        public delegate void OnPropertyChanged(TPropertyType newValue, TPropertyType oldValue);

        public event OnPropertyChanged OnChanged;

        public TPropertyType Value
        {
            get => _stored;
            set
            {
                var isEquals = _stored.Equals(value);
                if (isEquals) return;
                var oldValue = _value;
                Write(value);
                _stored = _value = value;
                OnChanged?.Invoke(value, oldValue);
            }
        }

        public PersistentProperty(TPropertyType defaultValue)
        {
            _defaultValue = defaultValue;
        }
        
        protected void Init()
        {
            _stored = _value = Read(_defaultValue);
        }

        public void Validate()
        {
            if (!_stored.Equals(_value))
            {
                Value = _value;
            }
        }
        protected abstract void Write(TPropertyType value);
        protected abstract TPropertyType Read(TPropertyType defaultValue);
    }
}