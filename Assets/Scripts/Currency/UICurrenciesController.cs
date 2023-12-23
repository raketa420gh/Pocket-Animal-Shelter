using System.Collections.Generic;
using UnityEngine;

public class UICurrenciesController : MonoBehaviour
{
    [SerializeField] GameObject _panelObject;
    [SerializeField] Transform _parentTransform;
    [SerializeField] private bool _hideAtStart = true;
    [SerializeField] private Factory _factory;

    private CurrenciesController _currenciesController;
    private Dictionary<Currency.Type, UIPanelCurrency> _activeUIPanels;

    public void Initialise(CurrenciesController currenciesController)
    {
        _currenciesController = currenciesController;
        _activeUIPanels = new Dictionary<Currency.Type, UIPanelCurrency>();
        Currency[] currencies = _currenciesController.Currencies;
        
        for (int i = 0; i < currencies.Length; i++)
        {
            UIPanelCurrency currencyUI = _factory.UI.CreateUICurrency(_parentTransform);
            currencyUI.Initialise(currencies[i]);
            currencyUI.Show();

            _activeUIPanels.Add(currencies[i].CurrencyType, currencyUI);
        }

        if (!_hideAtStart)
            return;

        HideCurrencyPanel();
    }

    public void ShowCurrencyPanel()
    {
        foreach (var aCurrencyUI in _activeUIPanels)
            aCurrencyUI.Value.Show();
    }

    public void HideCurrencyPanel()
    {
        foreach (var aCurrencyUI in _activeUIPanels)
            aCurrencyUI.Value.Hide();
    }

    public void ActivateAllExistingCurrencies()
    {
        Currency[] activeCurrencies = _currenciesController.Currencies;;
        
        for (int i = 0; i < activeCurrencies.Length; i++)
        {
            if (activeCurrencies[i].Amount > 0)
                ActivateCurrency(activeCurrencies[i].CurrencyType);
        }
    }

    public void RedrawCurrency(Currency.Type type, int amount)
    {
        if (_activeUIPanels.ContainsKey(type))
        {
            _activeUIPanels[type].SetAmount(amount);
        }
        else
        {
            ActivateCurrency(type);
        }
    }
    
    public void ActivateCurrency(Currency.Type type, bool doNotHide = false)
    {
        if (!_activeUIPanels.ContainsKey(type))
        {
            Currency currency = _currenciesController.GetCurrency(type);
            
            UIPanelCurrency currencyUI = _factory.UI.CreateUICurrency(_parentTransform);
            currencyUI.Initialise(currency);
            currencyUI.Show();

            _activeUIPanels.Add(type, currencyUI);
        }
        else
        {
            UIPanelCurrency currencyUI = _activeUIPanels[type];
            currencyUI.Redraw();
        }
    }

    public void DisableCurrency(Currency.Type type)
    {
        if (_activeUIPanels.ContainsKey(type))
        {
            _activeUIPanels[type].Hide();
            _activeUIPanels.Remove(type);
        }
    }
}