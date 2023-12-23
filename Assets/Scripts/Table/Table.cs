using UnityEngine;
using Zenject;

public class Table : MonoBehaviour, IAnimalHolder, IPurchaseObject
{
    [SerializeField] int _id;    
    [Header("Settings")] 
    [SerializeField] bool _isPermanentOpened = false;
    [Space] 
    [SerializeField] Currency.Type _priceCurrencyType;
    [SerializeField] int _priceAmount;
    [Header("Purchase Zone")] 
    [SerializeField] Transform _purchaseAreaPoint;
    [SerializeField] Vector2 _purchaseZoneSize;
    [Header("References")] 
    [SerializeField] GameObject _viewObject;
    [SerializeField] Transform _storageContainer;
    private IFactory _factory;
    private LevelZoneShelter _levelZoneShelter;
    private bool _isOpened;
    private bool _isUnlocked;
    private int _placedCurrencyAmount;
    private bool _isBusy;
    private bool _isAnimalPlaced;
    private PurchaseArea purchaseArea;
    private TableZone _tableZone;
    //private AnimalBehaviour _animalBehaviour;

    public Transform Transform => transform;
    public LevelZoneShelter LevelZoneShelter => _levelZoneShelter;
    public bool IsOpened => _isOpened;
    public bool IsUnlocked => _isUnlocked;
    public bool IsBusy => _isBusy;
    public int PlacedCurrencyAmount => _placedCurrencyAmount;
    public int PriceAmount => _priceAmount;
    public Currency.Type PriceCurrencyType => _priceCurrencyType;
    public bool IsAnimalPlaced => _isAnimalPlaced;

    [Inject]
    public void Construct(IFactory factory)
    {
        _factory = factory;
    }
    
    public void Initialise(LevelZoneShelter zoneShelter, TableZone tableZoneBehaviour, bool openingAnimation)
    {
        _levelZoneShelter = zoneShelter;
        _tableZone = tableZoneBehaviour;
        _isUnlocked = true;
        _isBusy = false;

        if (_isOpened || _isPermanentOpened)
        {
            gameObject.SetActive(true);
            _viewObject.SetActive(true);
            _viewObject.transform.localScale = Vector3.one;
            _isUnlocked = true;
            _isOpened = true;
        }
        else
        {
            gameObject.SetActive(false);
            CreatePurchaseZone();
        }
    }

    public void Unlock()
    {
        _isUnlocked = true;

        if (purchaseArea != null)
            purchaseArea.SetBlockState(false);
    }

    public void Lock()
    {
        _isUnlocked = false;

        if (purchaseArea != null)
            purchaseArea.SetBlockState(true);
    }

    public void PlaceCurrency(int amount)
    {
        if (!_isOpened)
        {
            //SaveService.MarkAsSaveIsRequired();
            _placedCurrencyAmount += amount;
            purchaseArea.SetAmount(_priceAmount - _placedCurrencyAmount);
        }
    }

    public void OnPurchaseCompleted()
    {
        ActivateWithAnimation();
        
        purchaseArea.Disable();
    }

    public void OnPlayerEntered(PlayerController player)
    {
        
    }

    public void OnPlayerExited(PlayerController player)
    {
        
    }

    public void ActivateWithAnimation()
    {
        if (_isOpened)
            return;

        _isOpened = true;

        // Invoke OnTableOpened method in table _levelZone script
        _tableZone.OnTableOpened(this);

        // Enable table object
        gameObject.SetActive(true);

        // Enable graphcs object
        _viewObject.SetActive(true);

        // Reset transform values
        //_viewObject.transform.localScale = Vector3.zero;

        // Play particle effect
        //ParticlesController.PlayParticle(PARTICLE_CONFETTI_HASH).SetPosition(transform.position);

        // Play open animation
        //_viewObject.transform.DOScale(Vector3.one, 0.4f).SetEasing(Ease.Type.BackOut).OnComplete(delegate
        //{
            // Add table to NavMesh
            //NavMeshController.RecalculateNavMesh(delegate { });
        //});
    }
    
    private void CreatePurchaseZone()
    {
        if (purchaseArea != null)
        {
            purchaseArea.Disable();
            purchaseArea = null;
        }
        
        purchaseArea = _factory.PurchaseArea.CreatePurchaseAreaS(_purchaseAreaPoint.position + Vector3.up * 0.01f);
        purchaseArea.Initialise(this, !_isUnlocked);
        purchaseArea.Enable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isUnlocked)
        {
            if (_isOpened)
            {
                if (!_isAnimalPlaced)
                {
                    if (other.GetComponent<PlayerController>())
                    {
                        Debug.Log($"PLAYER ENTERED {gameObject.name}");
                    }
                }
            }
        }
    }
    
    public void Load(TableSaveData save)
    {
        _isOpened = save.IsOpened;
        _placedCurrencyAmount = save.PlacedCurrencyAmount;
        _isUnlocked = save.IsUnlocked;
    }

    public TableSaveData Save()
    {
        return new TableSaveData(_placedCurrencyAmount, _isOpened, _isUnlocked);
    }

    /*#region AI

    public void OnAnimalPicked(AnimalBehaviour animalBehaviour)
    {
    }

    public void PlaceAnimal(IAnimalCarrying carrying, bool playSound = false)
    {
        if (isAnimalPlaced)
            return;

        AnimalBehaviour animalBehaviour = carrying.GetAnimal(allowedAnimalTypes);

        if (animalBehaviour == null)
            return;

        carrying.RemoveAnimal(animalBehaviour);

        isAnimalPlaced = true;

        // Add animal to table list
        _levelZone.AddTableAnimal(animalBehaviour);

        // Play animation
        animalBehaviour.transform.SetParent(null);
        animalBehaviour.transform.DORotate(_storageContainer.rotation, 0.5f);
        animalBehaviour.transform.DOBezierFollow(_storageContainer, Random.Range(3, 5), 0, 0.5f)
            .SetEasing(Ease.Type.SineIn)
            .OnComplete(delegate
            {
                animalBehaviour.transform.SetParent(_storageContainer);
                animalBehaviour.transform.localPosition = Vector3.zero;
                animalBehaviour.transform.localRotation = Quaternion.identity;

                animalBehaviour.SetAnimalHolder(this);
                animalBehaviour.OnAnimalPlacedOnTable(this);

                CheckDiagnosticTriggerAndActivate(animalBehaviour);
            });

        if (playSound)
            AudioController.PlaySound(AudioController.Sounds.animalPlaceSound);

        _animalBehaviour = animalBehaviour;
    }

    private static void CheckDiagnosticTriggerAndActivate(AnimalBehaviour animalBehaviour)
    {
        if (!animalBehaviour.IsDiagnostic)
            return;

        Vector3 center = animalBehaviour.transform.position;
        Vector3 size = animalBehaviour.Collider.size;
        Collider[] intersectingColliders = Physics.OverlapBox(center, size / 2);

        foreach (Collider collider in intersectingColliders)
        {
            PlayerBehavior player = collider.gameObject.GetComponent<PlayerBehavior>();

            if (player)
            {
                animalBehaviour.DisableNoDiagnosedIndicator();
                animalBehaviour.ActivateWaitingToDiagnostic(player);
            }
        }
    }

    public virtual void OnAnimalCured()
    {
        isBusy = false;

        // Remove animal from table list
        _levelZone.RemoveTableAnimal(_animalBehaviour);

        ResetTable();
    }

    public void ResetTable()
    {
        isAnimalPlaced = false;
        isBusy = false;

        _animalBehaviour = null;
    }

    public void MarkAsBusy()
    {
        isBusy = true;
    }

    #endregion*/

    /*#region Purchase

    public void PlaceCurrency(int amount)
    {
        if (!isOpened)
        {
            SaveService.MarkAsSaveIsRequired();

            placedCurrencyAmount += amount;

            purchaseAreaBehaviour.SetAmount(_priceAmount - placedCurrencyAmount);
        }
    }

    public void OnPurchaseCompleted()
    {
        ActivateWithAnimation();

        purchaseAreaBehaviour.DisableWithAnimation();

        AudioController.PlaySound(AudioController.Sounds.animalCureSound);
        AudioController.PlaySound(AudioController.Sounds.tableOpenSound);

        var parameters = new Dictionary<string, object> { { "Price", _priceAmount } };
        AppMetrica.Instance.ReportEvent("UnlockTable", parameters);

        TutorialController.OnTableUnlocked(this);

#if MOREMOUNTAINS_NICEVIBRATIONS
        if (AudioController.IsVibrationEnabled())
            Lofelt.NiceVibrations.HapticPatterns.PlayPreset(Lofelt.NiceVibrations.HapticPatterns.PresetType.Success);
#endif
    }

    #endregion

    #region Load/Save

    

    #endregion#1#*/
}