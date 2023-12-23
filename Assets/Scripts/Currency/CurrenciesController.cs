using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CurrenciesController : MonoBehaviour, ICurrenciesController
{
    [SerializeField] CurrenciesDatabase _currenciesDatabase;
    [SerializeField] private UICurrenciesController _uiCurrencies;
    private Currency[] _currencies;
    private Dictionary<Currency.Type, int> _currenciesLink;
    private ISaveService _saveService;

    public CurrenciesDatabase CurrenciesDatabase => _currenciesDatabase;
    public Currency[] Currencies => _currencies;
    public UICurrenciesController UICurrencies => _uiCurrencies;

    public event OnCurrencyAmountChangedCallback OnCurrencyAmountChanged;

    public void Initialise(ISaveService saveService)
    {
        _saveService = saveService;
        
        _currencies = _currenciesDatabase.Currencies;
        
        _currenciesLink = new Dictionary<Currency.Type, int>();
        
        for (int i = 0; i < _currencies.Length; i++)
        {
            if (!_currenciesLink.ContainsKey(_currencies[i].CurrencyType))
            {
                _currenciesLink.Add(_currencies[i].CurrencyType, i);
            }
            else
            {
                Debug.LogError(string.Format("[Currency System]: Currency with type {0} added to database twice!",
                    _currencies[i].CurrencyType));
            }

            
            var save = _saveService.GetSaveObject<Currency.Save>("currency" + ":" + (int)_currencies[i].CurrencyType);

            if (save != null)
            {
                _currencies[i].SetSave(save);
            }
            else
            {
                _currencies[i].SetSave(new Currency.Save());
            }
        }
        
        _uiCurrencies.Initialise(this);
    }

    public bool HasAmount(Currency.Type currencyType, int amount)
    {
        return _currencies[_currenciesLink[currencyType]].Amount >= amount;
    }

    public int Get(Currency.Type currencyType)
    {
        return _currencies[_currenciesLink[currencyType]].Amount;
    }

    public Currency GetCurrency(Currency.Type currencyType)
    {
        return _currencies[_currenciesLink[currencyType]];
    }

    public void Set(Currency.Type currencyType, int amount, bool redrawUI = true)
    {
        Currency currency = _currencies[_currenciesLink[currencyType]];

        currency.Amount = amount;
        
        _saveService.MarkAsSaveIsRequired();
        
        if (redrawUI)
        {
            UICurrencies.RedrawCurrency(currencyType, currency.Amount);
        }
        
        OnCurrencyAmountChanged?.Invoke(currencyType, currency.Amount, 0);
    }

    public void Add(Currency.Type currencyType, int amount, bool redrawUI = true)
    {
        Currency currency = _currencies[_currenciesLink[currencyType]];

        currency.Amount += amount;
        
        _saveService.MarkAsSaveIsRequired();
        _saveService.Save();
        
        if (redrawUI)
            UICurrencies.RedrawCurrency(currencyType, currency.Amount);
        
        OnCurrencyAmountChanged?.Invoke(currencyType, currency.Amount, amount);
    }

    public void Substract(Currency.Type currencyType, int amount, bool redrawUI = true)
    {
        Currency currency = _currencies[_currenciesLink[currencyType]];

        currency.Amount -= amount;
        
        _saveService.MarkAsSaveIsRequired();
        
        if (redrawUI)
            UICurrencies.RedrawCurrency(currencyType, currency.Amount);
        
        OnCurrencyAmountChanged?.Invoke(currencyType, currency.Amount, -amount);
    }

    public delegate void OnCurrencyAmountChangedCallback(Currency.Type currencyType, int amount, int amountDifference);
}