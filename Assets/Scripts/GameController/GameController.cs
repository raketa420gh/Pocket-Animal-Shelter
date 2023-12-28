using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    private ISaveService _saveService;
    private IFactory _factory;
    private ICurrenciesController _currenciesController;
    private IItemController _itemController;
    private PlayerController _playerController;
    private LevelController _levelController;

    [Inject]
    public void Construct(ISaveService saveService, IFactory factory, ICurrenciesController currenciesController,
        IItemController itemController, PlayerController playerController, LevelController levelController)
    {
        _saveService = saveService;
        _factory = factory;
        _currenciesController = currenciesController;
        _itemController = itemController;
        _playerController = playerController;
        _levelController = levelController;
    }
    
    private void Start()
    {
        InitializeGameLoop();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            _currenciesController.Add(Currency.Type.Money, 100);

        if (Input.GetKeyDown(KeyCode.S))
            _saveService.Save();
    }

    private void InitializeGameLoop()
    {
        _saveService.Initialise(Time.time, false, false);
        _factory.Initialize();
        _currenciesController.Initialise(_saveService);
        _itemController.Initialise();
        _playerController.Initialize(_factory, _currenciesController, _itemController);
        _levelController.InitializeLevel();
        
        Debug.Log("Game loop initialized");
    }
}