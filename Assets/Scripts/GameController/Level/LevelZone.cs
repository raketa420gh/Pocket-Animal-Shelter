using UnityEngine;
using Zenject;

public class LevelZone : MonoBehaviour, IPurchaseObject
{
    [SerializeField] protected int _id;
    [Space] 
    [SerializeField] protected bool _isPermanentOpened;
    [Space] 
    [SerializeField] protected Currency.Type _currencyType;
    [SerializeField] protected int _price;
    [SerializeField] protected PurchaseAreaCase[] _purchaseAreaCases;
    [Header("Zone View")]
    [SerializeField] Material _enabledMaterial;
    [SerializeField] Material _disabledMaterial;
    [SerializeField] MeshRenderer[] _floorMeshRenderers;
    [SerializeField] MeshRenderer[] _wallsMeshRenderers;
    [SerializeField] GameObject _activeEnvironmentObject;
    [SerializeField] GameObject[] _activeObjects;
    [SerializeField] GameObject _disabledEnvironmentObject;
    [SerializeField] GameObject[] _disabledObjects;
    [SerializeField] Transform _maskCenterTransform;
    protected  ISaveService _saveService;
    protected  IFactory _factory;
    protected NavMeshController _navMeshController;
    protected  bool _isOpened;
    protected  bool _isActivated;
    protected  int _placedCurrencyAmount;
    protected  PurchaseArea _purchaseArea;

    public int ID => _id;
    public Transform Transform => transform;
    public bool IsOpened => _isOpened;
    public bool IsPermanentOpened => _isPermanentOpened;
    public Currency.Type PriceCurrencyType => _currencyType;
    public int PriceAmount => _price;
    public int PlacedCurrencyAmount => _placedCurrencyAmount;
    
    [Inject]
    public void Construct(ISaveService saveService, IFactory factory, NavMeshController navMeshController)
    {
        _saveService = saveService;
        _factory = factory;
        _navMeshController = navMeshController;
    }
    
    public virtual void Initialise()
    {
        if (_isOpened || _isPermanentOpened)
        {
            CreatePurchaseAreasForLinkedZones();
            SetZoneOpenedState();
        }
        else
        {
            SetZoneClosedState();
        }
    }

    public void Lock()
    {
        if (_purchaseArea != null)
            _purchaseArea.SetBlockState(true);
    }

    public void Unlock()
    {
        if (_purchaseArea != null)
            _purchaseArea.SetBlockState(false);
    }

    public void PlaceCurrency(int amount)
    {
        if (!_isOpened)
        {
            _placedCurrencyAmount += amount;
            _purchaseArea.SetAmount(_price - _placedCurrencyAmount);
        }
    }

    public void OnMoneyPicked()
    {
    }

    public void OnPurchaseCompleted()
    {
        ActivateZone();
        
        _saveService.Save();
    }

    public void OnPlayerEntered(PlayerController player)
    {
    }

    public void OnPlayerExited(PlayerController player)
    {
    }

    protected virtual void ActivateZone()
    {
        if (_isActivated)
            return;

        _isActivated = true;
        _isOpened = true;
        
        if (_purchaseArea != null)
        {
            _purchaseArea.Disable();
            _purchaseArea = null;
        }
        
        Initialise();
        
        _navMeshController.UpdateNavMesh();
    }

    private void CreatePurchaseAreasForLinkedZones()
    {
        foreach (PurchaseAreaCase purchaseAreaCase in _purchaseAreaCases)
            if (!purchaseAreaCase.LinkedLevelZone.IsOpened)
                purchaseAreaCase.LinkedLevelZone.CreatePurchaseZone(purchaseAreaCase.PurchaseAreaTransform.position);
    }

    private void CreatePurchaseZone(Vector3 position)
    {
        if (_purchaseArea != null)
        {
            _purchaseArea.Disable();
            _purchaseArea = null;
        }

        _purchaseArea = _factory.PurchaseArea.CreatePurchaseAreaM(position + Vector3.up * 0.01f);
        _purchaseArea.Initialise(this, true);
        _purchaseArea.Enable();
        _purchaseArea.SetBlockState(false);
    }

    private void SetZoneOpenedState()
    {
        for (int i = 0; i < _floorMeshRenderers.Length; i++)
            _floorMeshRenderers[i].material = _enabledMaterial;

        for (int i = 0; i < _wallsMeshRenderers.Length; i++)
            _wallsMeshRenderers[i].material = _enabledMaterial;

        for (int i = 0; i < _activeObjects.Length; i++)
            _activeObjects[i].SetActive(true);

        _activeEnvironmentObject.SetActive(true);
        _disabledEnvironmentObject.SetActive(false);
    }

    private void SetZoneClosedState()
    {
        for (int i = 0; i < _floorMeshRenderers.Length; i++)
            _floorMeshRenderers[i].material = _disabledMaterial;

        for (int i = 0; i < _wallsMeshRenderers.Length; i++)
            _wallsMeshRenderers[i].material = _disabledMaterial;

        for (int i = 0; i < _activeObjects.Length; i++)
            _activeObjects[i].SetActive(false);

        _disabledEnvironmentObject.SetActive(true);
        _activeEnvironmentObject.SetActive(false);
    }
}