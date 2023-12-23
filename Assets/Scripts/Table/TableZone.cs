using UnityEngine;
using Zenject;

public class TableZone : MonoBehaviour, IPurchaseObject
{
    [Header("Settings")] 
    [SerializeField] private int _tableZoneID;
    [SerializeField] private bool _isPermanentOpened = false;
    [SerializeField] private Table[] _tables;
    [SerializeField] private TableZone _nextTableZone;
    [SerializeField] private GameObject[] _environmentObjects;
    [Space] 
    [SerializeField] Currency.Type _priceCurrencyType;
    [SerializeField] int _priceAmount;
    [Space] 
    [SerializeField] GameObject _walkableArea;
    [SerializeField] Transform _purchaseAreaPoint;
    [Header("Lock")] 
    [SerializeField] GameObject _lockObject;
    [SerializeField] GameObject _solidLockContainer;
    [SerializeField] GameObject _purchaseLockContainer;
    [Space] 
    [SerializeField] Transform _purchaseLeftSideTransform;
    [SerializeField] Transform _purchaseRightSideTransform;
    [Space] 
    [SerializeField] float _scaleZSize = 1.0f;
    [Header("Ad")] 
    [SerializeField] bool _isAllowedAdOpening = true;
    private ISaveService _saveService;
    private IFactory _factory;
    private NavMeshController _navMeshController;
    private LevelZoneShelter _levelZoneShelter;
    private PurchaseArea _purchaseArea;
    private bool _isUnlocked;
    private bool _isOpened;
    private int _placedCurrencyAmount;

    public bool IsOpened => _isOpened;
    public Currency.Type PriceCurrencyType => _priceCurrencyType;
    public int PriceAmount => _priceAmount;
    public int PlacedCurrencyAmount => _placedCurrencyAmount;
    public Transform Transform => transform;
    public Table[] Tables => _tables;
    public int TableZoneID => _tableZoneID;

    [Inject]
    public void Construct(ISaveService saveService, IFactory factory, NavMeshController navMeshController)
    {
        _saveService = saveService;
        _factory = factory;
        _navMeshController = navMeshController;
    }

    public void Initialize(LevelZoneShelter zoneShelter)
    {
       _levelZoneShelter = zoneShelter;
       _isUnlocked = true;
       
        if (_isOpened || _isPermanentOpened)
        {
            ToggleWalkableArea(true);
       
            for (int i = 0; i < _tables.Length; i++)
                _tables[i].Initialise(zoneShelter, this, true);

            if (_nextTableZone != null)
                _nextTableZone.ActivatePurchase();

            for (int i = 0; i < _environmentObjects.Length; i++)
                _environmentObjects[i].SetActive(true);
        }
        else
        {
            ToggleWalkableArea(false);
            
            for (int i = 0; i < _environmentObjects.Length; i++)
                _environmentObjects[i].SetActive(false);
        }
    }
    
    public void Unlock()
    {
        _isUnlocked = true;

        if (_purchaseArea != null)
            _purchaseArea.SetBlockState(false);
    }

    public void Lock()
    {
        _isUnlocked = false;

        if (_purchaseArea != null)
            _purchaseArea.SetBlockState(true);
    }
    
    public void PlaceCurrency(int amount)
    {
        if (!_isOpened)
        {
            _placedCurrencyAmount += amount;
            _purchaseArea.SetAmount(_priceAmount - _placedCurrencyAmount);
        }
    }

    public void OnPurchaseCompleted()
    {
        if (_isOpened)
            return;

        _isOpened = true;
        
        if (_nextTableZone != null)
        {
            _nextTableZone.ActivatePurchase();
        }

        if (_nextTableZone != null)
        {
            _nextTableZone.ActivatePurchase();
        }

        for (int i = 0; i < _tables.Length; i++)
        {
            _tables[i].Initialise(_levelZoneShelter, this, true);
        }

        for (int i = 0; i < _environmentObjects.Length; i++)
        {
            Vector3 defaultScale = _environmentObjects[i].transform.localScale;

            _environmentObjects[i].SetActive(true);
            _environmentObjects[i].transform.localScale = Vector3.one;
        }
        
        _purchaseArea.Disable();
        ToggleWalkableArea(true);
        _saveService.Save();
    }

    public void OnPlayerEntered(PlayerController player)
    {
        
    }

    public void OnPlayerExited(PlayerController player)
    {
        
    }

    public void OnTableOpened(Table table)
    {
       
    }

    public void ActivatePurchase()
    {
        if (_isOpened)
            return;

        if (_isPermanentOpened)
        {
            ToggleWalkableArea(true);
            return;
        }

        CreatePurchaseZone();
        _lockObject.SetActive(true);
        _solidLockContainer.SetActive(false);
        _purchaseLockContainer.SetActive(true);
        _purchaseArea.SetBlockState(false);
        
        _navMeshController.UpdateNavMesh();
    }

    private void ToggleWalkableArea(bool isActive)
    {
        _lockObject.SetActive(!isActive);
        _walkableArea.gameObject.SetActive(isActive);
        _navMeshController.UpdateNavMesh();
    }
    
    private void CreatePurchaseZone()
    {
        if (_purchaseArea != null)
        {
            _purchaseArea.Disable();
            _purchaseArea = null;
        }
        
        _purchaseArea = _factory.PurchaseArea.CreatePurchaseAreaM(_purchaseAreaPoint.position + Vector3.up * 0.01f);
        _purchaseArea.Initialise(this, !_isUnlocked);
        _purchaseArea.Enable();
    }

    #region Load/Save

    public void Load(SaveData save)
    {
        _isOpened = save.IsOpened;
        _placedCurrencyAmount = save.PlacedCurrencyAmount;

        if (save.TablesSaveData != null)
        {
            for (int i = 0; i < save.TablesSaveData.Length; i++)
            {
                if (_tables.IsInRange(i))
                {
                    _tables[i].Load(save.TablesSaveData[i]);
                }
            }
        }
    }

    public SaveData Save()
    {
        TableSaveData[] tablesSaveData = new TableSaveData[_tables.Length];
        for (int i = 0; i < tablesSaveData.Length; i++)
        {
            tablesSaveData[i] = _tables[i].Save();
        }

        return new SaveData(_tableZoneID, _placedCurrencyAmount, _isOpened, tablesSaveData);
    }

    #endregion

    [System.Serializable]
    public class SaveData
    {
        [SerializeField] int id;
        [SerializeField] int placedCurrencyAmount;
        [SerializeField] bool isOpened;
        [SerializeField] TableSaveData[] tablesSaveData;

        public int ID => id;
        public int PlacedCurrencyAmount => placedCurrencyAmount;
        public bool IsOpened => isOpened;
        public TableSaveData[] TablesSaveData => tablesSaveData;

        public SaveData(int id, int placedCurrencyAmount, bool isOpened, TableSaveData[] tablesSaveData)
        {
            this.id = id;
            this.placedCurrencyAmount = placedCurrencyAmount;
            this.isOpened = isOpened;
            this.tablesSaveData = tablesSaveData;
        }
    }
}