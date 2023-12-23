using UnityEngine;

public class UIFactory : MonoBehaviour
{
    [SerializeField] private UICurrencyPool _currencyPool;
    
    public void Initialize()
    {
        _currencyPool.Initialize();
    }

    public UIPanelCurrency CreateUICurrency(Transform parent)
    {
        UIPanelCurrency uiCurrency = _currencyPool.GetElement();
        uiCurrency.transform.SetParent(parent);

        return uiCurrency;
    }
}