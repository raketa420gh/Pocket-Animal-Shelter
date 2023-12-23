using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PurchaseArea : MonoBehaviour
{
    [SerializeField] RectTransform _canvasRectTransform;
    [Space] 
    [SerializeField] Text _amountText;
    [SerializeField] Image _currencyImage;
    [SerializeField] Image _blockImage;
    [SerializeField] Image _borderImage;
    private ICurrenciesController _currenciesController;
    private Currency.Type _requiredCurrencyType;
    private Currency _currency;
    private IPurchaseObject _targetPurchaseObject;
    private bool _isBlocked;
    private bool _isPurchasingEnabled;
    private bool _isAdAllowed;

    [Inject]
    public void Construct(ICurrenciesController currenciesController)
    {
        _currenciesController = currenciesController;
    }

    public void Initialise(IPurchaseObject targetPurchaseObject, bool blockState, bool isAdAllowed = false)
    {
        _targetPurchaseObject = targetPurchaseObject;
        _isAdAllowed = isAdAllowed;
        _requiredCurrencyType = targetPurchaseObject.PriceCurrencyType;
        
        _currency = _currenciesController.GetCurrency(_requiredCurrencyType);
        _currencyImage.sprite = _currency.Icon;
        SetAmount(targetPurchaseObject.PriceAmount - targetPurchaseObject.PlacedCurrencyAmount);
        SetBlockState(blockState);
    }
    
    public void SetAmount(int amount)
    {
        _amountText.text = CurrenciesFormatter.Format(amount);
    }

    public void SetBlockState(bool isBlocked)
    {
        _isBlocked = isBlocked;

        if (_isBlocked)
        {
            _amountText.gameObject.SetActive(false);
            _currencyImage.gameObject.SetActive(false);
            _blockImage.gameObject.SetActive(true);
        }
        else
        {
            _amountText.gameObject.SetActive(true);
            _currencyImage.gameObject.SetActive(true);
            _blockImage.gameObject.SetActive(false);
        }
    }

    public void Enable()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.localScale = Vector3.one;
        //transform.DOScale(1.0f, 0.38f).SetEasing(Ease.Type.SineOut);
        _borderImage.type = Image.Type.Filled;
        _borderImage.fillAmount = 0;
        _borderImage.fillAmount = 1;
        //borderImage.DOFillAmount(1.0f, 0.5f).OnComplete(delegate { });
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isBlocked)
            return;

        PlayerController player = other.GetComponent<PlayerController>();
        
        if (player)
        {
            Debug.Log($"PLAYER ENTERED {gameObject.name}");
            player.SetPurchaseObject(_targetPurchaseObject);
            _targetPurchaseObject.OnPlayerEntered(player);
            _isPurchasingEnabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_isBlocked)
            return;

        if (_isPurchasingEnabled)
        {
            PlayerController player = other.GetComponent<PlayerController>();
        
            if (player)
            {
                player.ResetPurchaseObject(_targetPurchaseObject);
                _targetPurchaseObject.OnPlayerExited(player);
                _isPurchasingEnabled = false;
            }
        }
    }
}