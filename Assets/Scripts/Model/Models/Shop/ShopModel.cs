using Model.Data;
using Model.Data.Property;
using PixelCrew.Model.Definitions;

namespace PixelCrew.Model.Models
{
    public class ShopModel
    {
        private InventoryData _playerInventory;

        public static ShopSelectionModel SelectionModel = new ShopSelectionModel();

        public class ShopSelectionModel
        {
            public readonly StringProperty SellSelection = new StringProperty();
            public readonly StringProperty BuySelection = new StringProperty();

            public void ResetSelection()
            {
                var allPrices = DefsFacade.I.ItemPrice.All;
                SellSelection.Value = allPrices[0].Id;
                BuySelection.Value = allPrices[0].Id;
            }
        }
        
        public ShopModel(InventoryData playerInventory)
        {
            _playerInventory = playerInventory;
        }

        public ItemWithCount GetSellPrice()
        {
            return DefsFacade.I.ItemPrice.Get(SelectionModel.SellSelection.Value).SellPrice;
        }

        public ItemWithCount GetBuyPrice()
        {
            return DefsFacade.I.ItemPrice.Get(SelectionModel.BuySelection.Value).BuyPrice;
        }

        public void Buy()
        {
            var itemPriceDef = DefsFacade.I.ItemPrice.Get(SelectionModel.BuySelection.Value);
            _playerInventory.Remove(itemPriceDef.BuyPrice.ItemId, itemPriceDef.BuyPrice.Count);
            _playerInventory.Add(itemPriceDef.Id, 1);
        }

        public void Sell()
        {
            var itemPriceDef = DefsFacade.I.ItemPrice.Get(SelectionModel.SellSelection.Value);
            _playerInventory.Remove(itemPriceDef.Id, 1);
            _playerInventory.Add(itemPriceDef.SellPrice.ItemId, itemPriceDef.SellPrice.Count);
            if (_playerInventory.Count(itemPriceDef.Id) == 0)
            {
                SelectionModel.SellSelection.Value = null;
            }
        }
    }
}