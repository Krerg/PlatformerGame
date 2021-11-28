using PixelCrew.Model.Definitions;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widgets
{
    public class ItemWidget : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _value;

        public void SetData(ItemWithCount price, string defaultText = "Undefined")
        {
            var def = DefsFacade.I.Items.Get(price.ItemId);
            _icon.sprite = def.icon;
            _value.text = price.Count.ToString();
        }
    }
}