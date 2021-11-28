using System;
using System.Linq;
using Components.Dialogs;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Models;
using PixelCrew.Utils.Disposables;
using UI.Widgets;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shop
{
    public class ShopWindow : AnimatedWindow
    {
        //TODO cache the instance
        private ShopModel _shop;

        private GameSession _session;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        [SerializeField] private Button buyButton;
        [SerializeField] private Button sellButton;


        [SerializeField] private ItemWidget sellPrice;
        [SerializeField] private ItemWidget buyPrice;

        private DataGroup<ItemPriceDef, ItemShopWidget> _buyDataGroup;
        private DataGroup<ItemPriceDef, ItemShopWidget> _sellDataGroup;

        [SerializeField] private Transform _sellPanel;
        [SerializeField] private Transform _buyPanel;

        [SerializeField] private ItemShopWidget _sellPrefab;
        [SerializeField] private ItemShopWidget _buyPrefab;

        [SerializeField] private ShowDialogComponent alertMessage;

        protected override void Start()
        {
            base.Start();
            _session = FindObjectOfType<GameSession>();
            _shop = new ShopModel(_session.Data.Inventory);

            _sellDataGroup = new DataGroup<ItemPriceDef, ItemShopWidget>(_sellPrefab, _sellPanel);
            _buyDataGroup = new DataGroup<ItemPriceDef, ItemShopWidget>(_buyPrefab, _buyPanel);

            UpdateSellSelection();
            UpdateBuySelection();

            _trash.Retain(buyButton.onClick.Subscribe(OnBuy));
            _trash.Retain(sellButton.onClick.Subscribe(OnSell));

            _trash.Retain(ShopModel.SelectionModel.BuySelection.Subscribe((value, oldValue) => UpdateView()));
            _trash.Retain(ShopModel.SelectionModel.SellSelection.Subscribe((value, oldValue) => UpdateView()));

            UpdateView();
        }

        private void UpdateButtonsState()
        {
            var buyPriceValue = _shop.GetBuyPrice();
            buyButton.gameObject.SetActive(_session.Data.Inventory.Count(buyPriceValue.ItemId) != 0 &&
                                           _session.Data.Inventory.IsEnough(buyPriceValue));
            sellButton.gameObject.SetActive(!string.IsNullOrEmpty(ShopModel.SelectionModel.SellSelection.Value));
        }
        
        private void UpdateView()
        {
            UpdatePrices();
            UpdateButtonsState();
            _sellDataGroup.UpdateItems();
            _buyDataGroup.UpdateItems();
        }

        private void UpdatePrices()
        {
            var sellPriceValue = _shop.GetSellPrice();
            var buyPriceValue = _shop.GetBuyPrice();
            UpdatePrice(ShopModel.SelectionModel.BuySelection.Value, buyPrice, buyPriceValue);
            UpdatePrice(ShopModel.SelectionModel.SellSelection.Value, sellPrice, sellPriceValue);
        }

        private void UpdatePrice(string selection, ItemWidget priceObject, ItemWithCount priceDef)
        {
            if (string.IsNullOrEmpty(selection))
            {
                priceObject.gameObject.SetActive(false);
                return;
            }

            priceObject.gameObject.SetActive(true);
            priceObject.SetData(priceDef);
        }

        private void OnBuy()
        {
            _shop.Buy();
            UpdateSellSelection();
            UpdateButtonsState();
            _sellDataGroup.UpdateItems();
        }

        private void OnSell()
        {
            if (IsLastSword())
            {
                alertMessage.Show();
                return;
            }

            _shop.Sell();
            UpdateSellSelection();
            UpdateButtonsState();
            _sellDataGroup.UpdateItems();
            _buyDataGroup.UpdateItems();
        }

        private bool IsLastSword()
        {
            return ShopModel.SelectionModel.SellSelection.Value.Equals("Sword") &&
                   _session.Data.Inventory.Count("Sword") == 1;
        }
        
        private void UpdateSellSelection()
        {
            _sellDataGroup.SetData(DefsFacade.I.ItemPrice.All
                .Where(def => def.SellPrice.Count != 0 && _session.Data.Inventory.Count(def.Id) > 0).ToArray());
        }

        private void UpdateBuySelection()
        {
            _buyDataGroup.SetData(DefsFacade.I.ItemPrice.All.Where(def => def.BuyPrice.Count != 0).ToArray());
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}