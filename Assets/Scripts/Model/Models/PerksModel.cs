using System;
using Components;
using Model.Data;
using Model.Data.Property;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils.Disposables;
using UnityEngine.PlayerLoop;

namespace PixelCrew.Model.Models
{
        public class PerksModel : IDisposable
    {
        private readonly PlayerData _data;
        public readonly StringProperty InterfaceSelection = new StringProperty();

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        
        private Cooldown _perkCooldown;
        
        public event Action OnChanged;

        public delegate void OnPerkUse(string perkId);
        public event OnPerkUse OnUse;
        
        public delegate void OnCooldownReach(string perkId);
        public event OnCooldownReach OnCooldown;

        public PerksModel(PlayerData data)
        {
            _data = data;
            InterfaceSelection.Value = DefsFacade.I.Perks.All[0].Id;
            _perkCooldown = new Cooldown();
            
            _trash.Retain(_data.Perks.Used.Subscribe((x, y) => OnChanged?.Invoke()));
            _trash.Retain(InterfaceSelection.Subscribe((x, y) => OnChanged?.Invoke()));
        }

        public void UpdateModel()
        {
            if (string.IsNullOrEmpty(Used))
            {
                return;
            }

            var cooldownValue = GetCooldownValue();
            if (cooldownValue <= 0)
            {
                var tmpUsed = Used;
                _data.Perks.Used.Value = null;
                OnCooldown?.Invoke(tmpUsed);
            }
        }

        public IDisposable SubscribeOnChange(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }
        
        public IDisposable SubscribeOnUse(OnPerkUse call)
        {
            OnUse += call;
            return new ActionDisposable(() => OnUse -= call);
        }
        
        public IDisposable SubscribeOnCooldown(OnCooldownReach call)
        {
            OnCooldown += call;
            return new ActionDisposable(() => OnCooldown -= call);
        }

        public string Used => _data.Perks.Used.Value;
        public bool IsSuperThrowSupported => _data.Perks.Used.Value == "super-throw";
        public bool IsShieldSupported => _data.Perks.Used.Value == "shield";
        public bool IsDoubleJumpSupported => _data.Perks.Used.Value == "double-jump";

        public void Unlock(string id)
        {
            var def = DefsFacade.I.Perks.Get(id);
            var isEnoughResources = _data.Inventory.IsEnough(def.Price);

            if (isEnoughResources)
            {
                _data.Inventory.Remove(def.Price.ItemId, def.Price.Count);
                _data.Perks.AddPerk(id);
                OnChanged?.Invoke();
            }
        }

        public float GetCooldownValue()
        {
            return _perkCooldown.GetTimeLeftInPercent();
        }

        public void UsePerk(string selected)
        {
            _data.Perks.Used.Value = selected;
            var def = DefsFacade.I.Perks.Get(selected);
            _perkCooldown.UpdateValue(def.Cooldown);
            OnUse?.Invoke(selected);
        }

        public bool IsUsed(string perkId)
        {
            return _data.Perks.Used.Value == perkId;
        }

        public bool IsUnlocked(string perkId)
        {
            return _data.Perks.IsUnlocked(perkId);
        }

        public bool CanBuy(string perkId)
        {
            var def = DefsFacade.I.Perks.Get(perkId);
            return _data.Inventory.IsEnough(def.Price);
        }

        public void Dispose()
        {
            _trash.Dispose();
        }
    }
}