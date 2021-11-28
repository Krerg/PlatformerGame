using System;
using System.Globalization;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Localization;
using PixelCrew.Model.Models;
using UI.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shop
{
    public class ItemShopWidget : MonoBehaviour, IItemRenderer<ItemPriceDef>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _name;
        [SerializeField] private GameObject _selector;

        [SerializeField] private ItemType _itemType;

        private GameSession _session;
        private ItemDef _data;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            Update();
        }

        private bool IsSelected()
        {
            switch (_itemType)
            {
                case ItemType.Buy:
                    return ShopModel.SelectionModel.BuySelection.Value == _data.Id;
                case ItemType.Sell:
                    return ShopModel.SelectionModel.SellSelection.Value == _data.Id;
                default:
                    return false;
            }
        }

        public void OnSelect()
        {
            switch (_itemType)
            {
                case ItemType.Buy:
                    ShopModel.SelectionModel.BuySelection.Value = _data.Id;
                    break;
                case ItemType.Sell:
                    ShopModel.SelectionModel.SellSelection.Value = _data.Id;
                    break;
            }
            Update();
        }

        public void SetData(ItemPriceDef data, int index)
        {
            _data = DefsFacade.I.Items.Get(data.Id);
            if (_session != null)
                Update();
        }

        public void Update()
        {
            _icon.sprite = _data.icon;
            _name.text = LocalizationManager.I.Localize(_data.Id);
            _selector.SetActive(IsSelected());
        }
    }

    public enum ItemType
    {
        Sell,
        Buy
    }
}