using UnityEngine;

public interface IPurchaseObject
{
    bool IsOpened { get; }
    Currency.Type PriceCurrencyType { get; }
    int PriceAmount { get; }
    int PlacedCurrencyAmount { get; }
    Transform Transform { get; }

    void PlaceCurrency(int amount);
    void OnPurchaseCompleted();
    void OnPlayerEntered(PlayerController player);
    void OnPlayerExited(PlayerController player);
}