public interface ICurrenciesController
{
    CurrenciesDatabase CurrenciesDatabase { get; }
    Currency[] Currencies { get; }
    UICurrenciesController UICurrencies { get; }

    event CurrenciesController.OnCurrencyAmountChangedCallback OnCurrencyAmountChanged;

    void Initialise(ISaveService saveService);
    bool HasAmount(Currency.Type currencyType, int amount);
    int Get(Currency.Type currencyType);
    Currency GetCurrency(Currency.Type currencyType);
    void Set(Currency.Type currencyType, int amount, bool redrawUI = true);
    void Add(Currency.Type currencyType, int amount, bool redrawUI = true);
    void Substract(Currency.Type currencyType, int amount, bool redrawUI = true);
    delegate void OnCurrencyAmountChangedCallback(Currency.Type currencyType, int amount, int amountDifference);
}