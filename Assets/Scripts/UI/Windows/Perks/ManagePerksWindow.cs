using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Localization;
using PixelCrew.Utils.Disposables;
using UI.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Perks
{
    public class ManagePerksWindow : AnimatedWindow
    {
        [SerializeField] private Transform _container;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _useButton;
        [SerializeField] private Text _infoText;
        [SerializeField] private ItemWidget _price;

        private PredefinedDataGroup<PerkDef, PerkWidget> _dataGroup;
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private GameSession _session;

        protected override void Start()
        {
            base.Start();

            _dataGroup = new PredefinedDataGroup<PerkDef, PerkWidget>(_container);
            _session = FindObjectOfType<GameSession>();

            _trash.Retain(_session.PerksModel.SubscribeOnChange(OnPerksChanged));

            _trash.Retain(_buyButton.onClick.Subscribe(OnBuy));
            _trash.Retain(_useButton.onClick.Subscribe(OnUse));

            OnPerksChanged();
        }

        private void OnPerksChanged()
        {
            _dataGroup.SetData(DefsFacade.I.Perks.All);

            var selected = _session.PerksModel.InterfaceSelection.Value;

            _useButton.gameObject.SetActive(_session.PerksModel.IsUnlocked(selected));
            _useButton.interactable = _session.PerksModel.Used != selected;

            _buyButton.gameObject.SetActive(!_session.PerksModel.IsUnlocked(selected));
            _buyButton.interactable = _session.PerksModel.CanBuy(selected);

            var def = DefsFacade.I.Perks.Get(selected);
            _price.SetData(def.Price);

            _infoText.text = LocalizationManager.I.Localize(def.Info);
        }

        private void OnUse()
        {
            var selected = _session.PerksModel.InterfaceSelection.Value;
            _session.PerksModel.UsePerk(selected);
        }

        private void OnBuy()
        {
            var selected = _session.PerksModel.InterfaceSelection.Value;
            _session.PerksModel.Unlock(selected);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}