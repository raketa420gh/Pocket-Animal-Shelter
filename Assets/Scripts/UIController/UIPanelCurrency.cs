using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
public class UIPanelCurrency : UIPanel
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _icon;
    private Currency _currency;
    
    public Currency Currency => _currency;

    public void Initialise(Currency currency)
    {
        _currency = currency;
        _icon.sprite = currency.Icon;

        Redraw();
    }

    public void Redraw()
    {
        _rect.localScale = Vector3.one;
        _text.text = CurrenciesFormatter.Format(_currency.Amount);
    }

    public void SetAmount(int amount)
    {
        _text.text = CurrenciesFormatter.Format(amount);
    }
}