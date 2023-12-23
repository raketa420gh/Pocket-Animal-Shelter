using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] NavMeshAgent _navAgent;
    [SerializeField] private float _aimSpeed;
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private ItemCarrier _itemCarrier;
    [SerializeField] private InputService _input;
    [SerializeField] private bool _isRunning;
    [SerializeField] private float _speed = 0;
    [SerializeField] private float _maxSpeed = 1;
    [SerializeField] private float _acceleration;
    private ICurrenciesController _currenciesController;
    private IItemController _itemController;
    private IPurchaseObject _activePurchaseObject;
    private Coroutine _purchaseCoroutine;
    private bool _isPurchaseParticleActive;
    private float _moneyPlaceSoundTime;

    public ItemCarrier ItemCarrier => _itemCarrier;

    private void Update()
    {
        UpdateInputMovement();
        
        if (_itemCarrier)
            _itemCarrier.RecalculateItemsPositions();
    }

    public void Initialize(ICurrenciesController currenciesController, IItemController itemController)
    {
        _currenciesController = currenciesController;
        _itemController = itemController;
        _itemCarrier.Initialize(_itemController);
    }

    public void SetPurchaseObject(IPurchaseObject purchaseObject)
    {
        if (_activePurchaseObject != null)
            StopCoroutine(_purchaseCoroutine);

        _activePurchaseObject = purchaseObject;
        _purchaseCoroutine = StartCoroutine(PurchaseCoroutine());
    }

    public void ResetPurchaseObject(IPurchaseObject purchaseObject)
    {
        if (_activePurchaseObject == purchaseObject)
        {
            // Disable money particle
            //moneyParticleSystem.Stop();

            _isPurchaseParticleActive = false;
            StopCoroutine(_purchaseCoroutine);
            _activePurchaseObject = null;
        }
    }

    private IEnumerator PurchaseCoroutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.5f);
        yield return waitForSeconds;
        
        int stepSize = 1;
        waitForSeconds = new WaitForSeconds(0.025f);
        
        while (!_activePurchaseObject.IsOpened)
        {
            yield return waitForSeconds;
            
            int allowedAmount = stepSize;
            int placeDifference = _activePurchaseObject.PriceAmount - _activePurchaseObject.PlacedCurrencyAmount;
            int currentAmount = _currenciesController.Get(_activePurchaseObject.PriceCurrencyType);

            if (placeDifference < allowedAmount)
                allowedAmount = placeDifference;

            if (currentAmount < allowedAmount)
                allowedAmount = currentAmount;

            if (allowedAmount != 0 && currentAmount >= allowedAmount)
            {
                /*if (!_isPurchaseParticleActive)
                {
                    // EnableWithTimer money particle
                    moneyParticleSystem.Play();

                    _isPurchaseParticleActive = true;
                }*/

                /*if (Time.time > _moneyPlaceSoundTime)
                {
                    //AudioController.PlaySound(AudioController.Sounds.moneyPlaceSound);

                    _moneyPlaceSoundTime = Time.time + 0.3f;
                }*/

                _currenciesController.Substract(_activePurchaseObject.PriceCurrencyType, allowedAmount);
                _activePurchaseObject.PlaceCurrency(allowedAmount);
                stepSize *= 2;

                if (_activePurchaseObject.PlacedCurrencyAmount >= _activePurchaseObject.PriceAmount)
                    break;
            }
            else
            {
                /*// Disable money particle
                moneyParticleSystem.Stop();

                _isPurchaseParticleActive = false;*/
            }
        }

        _activePurchaseObject.OnPurchaseCompleted();
        ResetPurchaseObject(_activePurchaseObject);
    }
    
    /*

    // Carrying
    private int _maxAnimalsAmount;
    private bool _isAnimalCarrying;
    private AnimalBehaviour _carryingAnimal;
    public AnimalBehaviour AnimalBehaviour => _carryingAnimal;
    
    private static float maxTextLastTime;

    // Diagnostic
    private float _diagnosticTime;
    public float DiagnosticSpeed => _diagnosticTime;

    // Building
    private BuildingBehaviour _activeBuilding;
    private bool _isBuildingExchangeEnable;
    private Coroutine _buildingExchangeCoroutine;

    // Purchase
    private IPurchaseObject _activePurchaseObject;
    private Coroutine _purchaseCoroutine;
    private bool _isPurchaseParticleActive;

    // Steps particle
    private Transform _leftFootTransform;
    private Transform _rightFootTransform;

    public bool IsDead { get; private set; }

    // Upgrades
    private MovementSpeedUpgrade _movementSpeedUpgrade;
    private CharacterStrengthUpgrade _characterStrengthUpgrade;
    private CharacterDiagnosticUpgrade _characterDiagnosticUpgrade;

    public int SkinIndex => _playerGraphics.SkinIndex;

    private void Update()
    {
        // Recalculate carrying animals position
        if (_isAnimalCarrying)
        {
            for (int i = 0; i < _carryingAnimalsAmount; i++)
            {
                if (_carryingAnimalsList[i].IsPicked)
                {
                    _carryingAnimalsList[i].AnimalBehaviour.transform.position =
                        _carryingAnimalsList[i].StorageObject.transform.position;
                    _carryingAnimalsList[i].AnimalBehaviour.transform.rotation =
                        _carryingAnimalsList[i].StorageObject.transform.rotation;
                }
            }
        }

        _cameraTarget.position = transform.position + _cameraCurrentOffset;
    }

    public void SetUpgradeBlockDiagnostic()
    {
        _characterDiagnosticUpgrade.Block();
    }

    public void SetGraphics(GameObject graphics)
    {
        // Check if graphics isn't exist already
        if (_playerPrefabGraphics != graphics)
        {
            // Store prefab link
            _playerPrefabGraphics = graphics;

            if (_playerGraphics != null)
            {
                for (int i = 0; i < _carryingAnimalsAmount; i++)
                {
                    _carryingAnimalsList[i].StorageObject.transform.SetParent(null);
                }

                Destroy(_playerGraphics.gameObject);
            }

            GameObject playerGraphicObject = Instantiate(graphics);
            playerGraphicObject.transform.SetParent(transform);
            playerGraphicObject.transform.localPosition = Vector3.zero;
            playerGraphicObject.transform.localRotation = Quaternion.identity;
            playerGraphicObject.SetActive(true);

            //_playerGraphics = playerGraphicObject.GetComponent<PlayerGraphics>();
            //_playerGraphics.Inititalise(this);

            _playerAnimator = _playerGraphics.Animator;

            _leftFootTransform = _playerAnimator.GetBoneTransform(HumanBodyBones.LeftFoot).GetChild(0);
            _rightFootTransform = _playerAnimator.GetBoneTransform(HumanBodyBones.RightFoot).GetChild(0);

            // Get layer hash
            _handLayerHash = _playerAnimator.GetLayerIndex("Hands");

            // Get storage transform
            _storageTransform = _playerGraphics.StorageTransform;

            if (_carryingAnimalsAmount > 0)
            {
                for (int i = 0; i < _carryingAnimalsAmount; i++)
                {
                    _carryingAnimalsList[i].StorageObject.transform.SetParent(_storageTransform);
                    _carryingAnimalsList[i].StorageObject.transform.localRotation = Quaternion.identity;
                    _carryingAnimalsList[i].StorageObject.transform.localPosition = STORAGE_OFFSET * i;
                }

                RegroupAnimals();
            }
        }
    }

    public void SetGraphicsState(bool state)
    {
        _playerGraphics.gameObject.SetActive(state);
    }

    public void SelectSkin(int index)
    {
        _playerGraphics.SetSkin(index);
    }

    #region Carrying

    private void EnableHands(float duration = 0)
    {
        if (_isHandEnabled)
            return;

        if (_handTweenCase != null && !_handTweenCase.isCompleted)
            _handTweenCase.Kill();

        if (duration > 0)
        {
            _handTweenCase = Tween
                .DoFloat(0, 1.0f, duration, (time) => { _playerAnimator.SetLayerWeight(_handLayerHash, time); })
                .OnComplete(delegate { _isHandEnabled = true; });
        }
        else
        {
            _playerAnimator.SetLayerWeight(_handLayerHash, 1.0f);

            _isHandEnabled = true;
        }
    }

    private void DisableHands(float duration = 0)
    {
        if (!_isHandEnabled)
            return;

        if (_handTweenCase != null && !_handTweenCase.isCompleted)
            _handTweenCase.Kill();

        if (duration > 0)
        {
            _handTweenCase = Tween
                .DoFloat(1.0f, 0.0f, duration, (time) => { _playerAnimator.SetLayerWeight(_handLayerHash, time); })
                .OnComplete(delegate { _isHandEnabled = false; });
        }
        else
        {
            _playerAnimator.SetLayerWeight(_handLayerHash, 0.0f);

            _isHandEnabled = false;
        }
    }

    public bool IsAnimalCarrying()
    {
        return _isAnimalCarrying;
    }

    public void CarryAnimal(AnimalBehaviour animalBehaviour)
    {
        AddAnimal(animalBehaviour);
    }

    public bool IsAnimalPickupAllowed()
    {
        return _carryingAnimalsAmount < _maxAnimalsAmount;
    }

    public AnimalBehaviour GetAnimal(Animal.Type[] allowedAnimalTypes)
    {
        if (_carryingAnimalsAmount > 0)
        {
            for (int i = _carryingAnimalsAmount - 1; i >= 0; i--)
            {
                for (int a = 0; a < allowedAnimalTypes.Length; a++)
                {
                    if (_carryingAnimalsList[i].IsPicked && allowedAnimalTypes[a] ==
                        _carryingAnimalsList[i].AnimalBehaviour.Animal.AnimalType)
                    {
                        return _carryingAnimalsList[i].AnimalBehaviour;
                    }
                }
            }
        }

        return null;
    }

    public void RemoveAnimal(AnimalBehaviour animalBehaviour)
    {
        if (_carryingAnimalsAmount > 0)
        {
            for (int i = _carryingAnimalsAmount - 1; i >= 0; i--)
            {
                if (_carryingAnimalsList[i].IsPicked && _carryingAnimalsList[i].AnimalBehaviour == animalBehaviour)
                {
                    _carryingAnimalsList[i].AnimalBehaviour.transform.SetParent(null);
                    _carryingAnimalsList[i].Reset();

                    _carryingAnimalsAmount--;
                    _carryingAnimalsList.RemoveAt(i);

                    if (_carryingAnimalsAmount == 0)
                    {
                        // Disable hold animation
                        DisableHands(0.3f);

                        _carryingAnimalsHeight = 0;
                        _isAnimalCarrying = false;

                        _mainUI.SetDropButtonState(false);
                    }

                    break;
                }
            }

            RegroupAnimals();
        }
    }

    public void DropAnimals(bool disableHands = false)
    {
        if (_carryingAnimalsAmount > 0)
        {
            for (int i = _carryingAnimalsAmount - 1; i >= 0; i--)
            {
                Vector3 dropPosition = transform.position + (Random.insideUnitSphere.SetY(0) * 5);

                AnimalBehaviour storedAnimalBehaviour = _carryingAnimalsList[i].AnimalBehaviour;
                storedAnimalBehaviour.transform.SetParent(null);
                storedAnimalBehaviour.transform.DOBezierMove(dropPosition, 3, 0, 0.3f).OnComplete(delegate
                {
                    storedAnimalBehaviour.OnAnimalDropped();
                });

                _carryingAnimalsList[i].Reset();

                _carryingAnimalsAmount--;
                _carryingAnimalsList.RemoveAt(i);
            }
        }

        _carryingAnimalsHeight = 0;
        _isAnimalCarrying = false;

        if (disableHands)
        {
            // Disable hold animation
            DisableHands(0.3f);
        }
    }

    public AnimalStorageCase AddAnimal(AnimalBehaviour animalBehaviour)
    {
        if (_isItemsCarrying)
            RemoveItems(false);

        EnableHands();

        GameObject storageObject = storageHolderPool.GetPooledObject();
        storageObject.transform.SetParent(_storageTransform);
        storageObject.transform.localPosition = new Vector3(0, _carryingAnimalsHeight, 0);
        storageObject.transform.localRotation = Quaternion.identity;
        storageObject.transform.localScale = Vector3.one;
        storageObject.SetActive(true);

        AnimalStorageCase storageCase = new AnimalStorageCase(animalBehaviour, storageObject);
        storageCase.SetIndex(_carryingAnimalsAmount);

        // Play animation
        animalBehaviour.transform.SetParent(null);
        animalBehaviour.transform.DOBezierFollow(storageObject.transform, Random.Range(3, 5), 0, 0.5f)
            .SetEasing(Ease.Type.SineIn).OnComplete(delegate
            {
                animalBehaviour.transform.localPosition = storageObject.transform.position;
                animalBehaviour.transform.localRotation = storageObject.transform.rotation;

                storageCase.MarkAsPicked();
            });

        _carryingAnimal = animalBehaviour;

        _carryingAnimalsAmount++;
        _carryingAnimalsList.Add(storageCase);

        _carryingAnimalsHeight += animalBehaviour.GetCarryingHeight();

        _isAnimalCarrying = true;

        _mainUI.SetDropButtonState(true);

        return storageCase;
    }

    private void RegroupAnimals()
    {
        _carryingAnimalsHeight = 0;

        if (_carryingAnimalsAmount > 0)
        {
            for (int i = 0; i < _carryingAnimalsAmount; i++)
            {
                _carryingAnimalsList[i].SetIndex(i);
                _carryingAnimalsList[i].StorageObject.transform.localPosition =
                    new Vector3(0, _carryingAnimalsHeight, 0);

                _carryingAnimalsHeight += _carryingAnimalsList[i].AnimalBehaviour.GetCarryingHeight();
            }

            _carryingAnimal = _carryingAnimalsList[0].AnimalBehaviour;
        }
    }

    #endregion

    #region Building

    public void SetActiveBuilding(BuildingBehaviour building)
    {
        _activeBuilding = building;
    }

    public void ResetBuildingLink(BuildingBehaviour building)
    {
        if (_activeBuilding == building)
            _activeBuilding = null;
    }

    #endregion

    #region Purchase

    public void SetPurchaseObject(IPurchaseObject purchaseObject)
    {
        if (_activePurchaseObject != null)
            StopCoroutine(_purchaseCoroutine);

        _activePurchaseObject = purchaseObject;

        _purchaseCoroutine = StartCoroutine(PurchaseCoroutine());
    }

    private float moneyPlaceSoundTime;

    private IEnumerator PurchaseCoroutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.5f);

        yield return waitForSeconds;

        int stepSize = 1;

        waitForSeconds = new WaitForSeconds(0.025f);
        while (!_activePurchaseObject.IsOpened)
        {
            yield return waitForSeconds;

            int allowedAmount = stepSize;
            int placeDifference = _activePurchaseObject.PriceAmount - _activePurchaseObject.PlacedCurrencyAmount;
            int currentAmount = CurrenciesController.Get(_activePurchaseObject.PriceCurrencyType);

            if (placeDifference < allowedAmount)
                allowedAmount = placeDifference;

            if (currentAmount < allowedAmount)
                allowedAmount = currentAmount;

            if (allowedAmount != 0 && currentAmount >= allowedAmount)
            {
                if (!_isPurchaseParticleActive)
                {
                    // EnableWithTimer money particle
                    moneyParticleSystem.Play();

                    _isPurchaseParticleActive = true;
                }

                if (Time.time > moneyPlaceSoundTime)
                {
                    AudioController.PlaySound(AudioController.Sounds.moneyPlaceSound);

                    moneyPlaceSoundTime = Time.time + 0.3f;
                }

                CurrenciesController.Substract(_activePurchaseObject.PriceCurrencyType, allowedAmount);

                _activePurchaseObject.PlaceCurrency(allowedAmount);

                stepSize *= 2;

                if (_activePurchaseObject.PlacedCurrencyAmount >= _activePurchaseObject.PriceAmount)
                    break;
            }
            else
            {
                // Disable money particle
                moneyParticleSystem.Stop();

                _isPurchaseParticleActive = false;
            }
        }

        _activePurchaseObject.OnPurchaseCompleted();

        ResetPurchaseObject(_activePurchaseObject);
    }

    public void ResetPurchaseObject(IPurchaseObject purchaseObject)
    {
        if (_activePurchaseObject == purchaseObject)
        {
            // Disable money particle
            moneyParticleSystem.Stop();

            _isPurchaseParticleActive = false;

            StopCoroutine(_purchaseCoroutine);

            _activePurchaseObject = null;
        }
    }

    #endregion

    public void LevelStart(Vector3 startPos)
    {
        transform.position = startPos;
        _navAgent.Warp(startPos);
    }

    public void Warp(Transform destination)
    {
        _navAgent.Warp(destination.position);
        transform.rotation = destination.rotation;
    }

    public void PlayMoneyPickUpParticle()
    {
        moneyPickUpParticleSystem.Stop();

        moneyPickUpParticleSystem.Play();
    }

    public static PlayerBehavior GetBehavior()
    {
        return playerBehavior;
    }

    public static void DropItemsAndAnimals()
    {
        if (playerBehavior._isItemsCarrying)
            playerBehavior.RemoveItems(true);

        if (playerBehavior._isAnimalCarrying)
            playerBehavior.DropAnimals(true);
    }

    public void LeftFootParticle()
    {
        if (!_isRunning)
            return;

        stepParticleSystem.transform.position = _leftFootTransform.position - transform.forward * 0.4f;
        stepParticleSystem.Play();
    }

    public void RightFootParticle()
    {
        if (!_isRunning)
            return;

        stepParticleSystem.transform.position = _rightFootTransform.position - transform.forward * 0.4f;
        stepParticleSystem.Play();
    }

    public static void SpawnMaxText()
    {
        if (Time.time < maxTextLastTime)
            return;

        maxTextLastTime = Time.time + 1.0f;

        FloatingTextController.SpawnFloatingText(FloatingTextStyle.Max, "MAX",
            playerBehavior.transform.position + new Vector3(0, 9, -1));
    }

    public void OnNavMeshUpdated()
    {
        _navAgent.enabled = true;
    }

    private void SetMaxDiagnosticSpeedUpgrade()
    {
        _characterDiagnosticUpgrade.ResetUpgrade();
        _characterDiagnosticUpgrade.UpgradeStage();
        _characterDiagnosticUpgrade.UpgradeStage();
        _characterDiagnosticUpgrade.UpgradeStage();
        _characterDiagnosticUpgrade.UpgradeStage();
        _characterDiagnosticUpgrade.UpgradeStage();
    }

    private void SetPlayerAsCameraTarget()
    {
        GameObject cameraTargetObject = new GameObject("[CameraTarget]");
        cameraTargetObject.transform.ResetGlobal();

        _cameraTarget = cameraTargetObject.transform;
        _cameraTarget.position = transform.position;
    }

    private void InitializeUpgrades()
    {
        _movementSpeedUpgrade = UpgradesController.GetUpgrade<MovementSpeedUpgrade>(UpgradeType.PlayerMovementSpeed);
        _characterStrengthUpgrade = UpgradesController.GetUpgrade<CharacterStrengthUpgrade>(UpgradeType.PlayerStrength);
        _characterDiagnosticUpgrade =
            UpgradesController.GetUpgrade<CharacterDiagnosticUpgrade>(UpgradeType.PlayerDiagnosticSpeed);

        UpgradesEventsHandler.OnUpgraded += OnUpgrade;
    }

    private void InitializeStorage()
    {
        _carryingAnimalsAmount = 0;
        _carryingAnimalsList = new List<AnimalStorageCase>();

        _carryingItemsAmount = 0;
        _carryingItemsList = new List<ItemStorageCase>();

        storageHolderPool = new Pool(new PoolSettings("StorageHolder", new GameObject("Storage Holder"), 3, true,
            _storageTransform));
    }

    #region Boosters

    public void ActivateSpeedBooster()
    {
        StartCoroutine(nameof(StartSpeedBoosterRoutine));
    }

    public void ActivateStrengthBooster()
    {
        StartCoroutine(nameof(StartStrengthBoosterRoutine));
    }

    public void ActivateVacuumCleanerBooster()
    {
        StartCoroutine(nameof(StartVacuumCleanerBoosterRoutine));
    }

    private IEnumerator StartSpeedBoosterRoutine()
    {
        if (!_speedBooster.IsActive)
        {
            _speedBooster.Activate();

            yield return new WaitForSeconds(_speedBooster.Duration);

            _speedBooster.Deactivate();
        }
    }

    private IEnumerator StartStrengthBoosterRoutine()
    {
        if (!_strengthBooster.IsActive)
        {
            int maxItemsAmount = _maxItemsAmount;
            int maxAnimalsAmount = _maxAnimalsAmount;
            _maxItemsAmount = _strengthBooster.BoostValue;
            _maxAnimalsAmount = _strengthBooster.BoostValue;

            _strengthBooster.Activate();

            yield return new WaitForSeconds(_strengthBooster.Duration);

            _maxItemsAmount = maxItemsAmount;
            _maxAnimalsAmount = maxAnimalsAmount;
            _strengthBooster.Deactivate();
        }
    }

    private IEnumerator StartVacuumCleanerBoosterRoutine()
    {
        if (!CheckPlayerZone())
            yield break;

        _vacuumCleanerBooster.Activate();

        Zone vacuumCleanerZone = _playerZone;
        vacuumCleanerZone.ActivateVacuumCleaner(transform.position);

        yield return new WaitForSeconds(_vacuumCleanerBooster.Duration);

        vacuumCleanerZone.DeactivateVacuumCleaner();
        _vacuumCleanerBooster.Deactivate();
    }

    #endregion

    #region Load/Save

    public void Load(PlayerSave save)
    {
        SelectSkin(save.SkinIndex);
    }

    #endregion

    public void SetPlayerZone(Zone zone)
    {
        _playerZone = zone;

        Debug.Log($"PLAYER ZONE = {zone}");
    }

    private bool CheckPlayerZone()
    {
        return _playerZone;
    }*/
    
    private void UpdateInputMovement()
    {
        if (_input.Axis.sqrMagnitude > 0.1f)
        {
            if (!_isRunning)
            {
                _isRunning = true;
                _playerView.Animator.SetRunAnimation(true);
                _speed = 0;
            }

            float maxAllowedSpeed = _input.AxisFormat.magnitude * _maxSpeed;

            if (_speed > maxAllowedSpeed)
            {
                _speed -= _acceleration * Time.deltaTime;

                if (_speed < maxAllowedSpeed)
                {
                    _speed = maxAllowedSpeed;
                }
            }
            else
            {
                _speed += _acceleration * Time.deltaTime;

                if (_speed > maxAllowedSpeed)
                {
                    _speed = maxAllowedSpeed;
                }
            }

            transform.position += _input.AxisFormat * (Time.deltaTime * _speed);
            transform.rotation =
                Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_input.AxisFormat.normalized), 0.2f);
            _playerView.Animator.SetMoveSpeedParameter(_speed / _maxSpeed);
        }
        else
        {
            if (_isRunning)
            {
                _isRunning = false;
                _playerView.Animator.SetRunAnimation(false);
            }
        }
    }
}