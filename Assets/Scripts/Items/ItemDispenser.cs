using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using Zenject;

public class ItemDispenser : MonoBehaviour
{
    [SerializeField] Item.Type _itemType;
    //[SerializeField] WaitingIndicatorBehaviour waitingIndicatorBehaviour;
    [SerializeField] InteractableZone _interactableZone;
    [Space] 
    [SerializeField] GameObject _graphicsGameObject;
    [SerializeField] GameObject _lockGameObject;
    [SerializeField] Transform _rotateTransform;
    [SerializeField] SpriteRenderer _iconSpriteRenderer;
    [Space] 
    [SerializeField] float _openTime;
    [SerializeField] Ease _openEasing;
    [SerializeField] float _closeTime;
    [SerializeField] Ease _closeEasing;
    private IItemController _itemController;
    private PlayerController _playerController;
    private bool _isPlayerEnteredZone;
    private bool _isUnlocked = true;
    private bool _isDispensingToPlayer = false;
    private float _carrierPickTime = 2f;
    private float _timer = 0f;
    private TweenerCore<Quaternion, Vector3, QuaternionOptions> _openCloseTween;

    public bool IsUnlocked => _isUnlocked;
    public Item.Type ItemType => _itemType;

    [Inject]
    public void Construct(IItemController itemController, PlayerController playerController)
    {
        _itemController = itemController;
        _playerController = playerController;
    }

    private void Update()
    {
        if (!_isUnlocked)
            return;
        
        if (!_isDispensingToPlayer)
            return;

        _timer += Time.deltaTime;

        if (_timer >= _carrierPickTime)
        {
            TryAddItemToPlayer();
            _timer = 0f;
        }
    }

    public void Initialise()
    {
        _interactableZone.OnPlayerEnter += OnPlayerZoneEnter;
        _interactableZone.OnPlayerExit += OnPlayerZoneExit;

        Item item = _itemController.GetItem(_itemType);
        _iconSpriteRenderer.sprite = item.Icon;
        _graphicsGameObject.SetActive(_isUnlocked);
        _lockGameObject.SetActive(!_isUnlocked);
        
        //Disable ui indicators
        
        SetUnlockState(true);
    }

    public void Deinitialize()
    {
        _interactableZone.OnPlayerEnter -= OnPlayerZoneEnter;
        _interactableZone.OnPlayerExit -= OnPlayerZoneExit;
    }

    public void SetUnlockState(bool state)
    {
        _isUnlocked = state;
        _graphicsGameObject.SetActive(_isUnlocked);
        _lockGameObject.SetActive(!_isUnlocked);
    }

    private void TryAddItemToPlayer()
    {
        if (!_playerController.ItemCarrier.HasFreeSpace())
        {
            Debug.Log("MAX");
        }
        else
        {
            PlayOpenCloseAnimation();
            _playerController.ItemCarrier.AddItem(_itemType);
        }
    }

    private void PlayOpenCloseAnimation()
    {
        if (_openCloseTween  != null && !_openCloseTween.IsComplete())
            _openCloseTween.Kill();

        _rotateTransform.localRotation = Quaternion.identity;
        _openCloseTween = _rotateTransform
            .DOLocalRotate(new Vector3(-98, 0, 0), _openTime).SetEase(_openEasing);

        _openCloseTween .onComplete += () =>
        {
            _openCloseTween  = _rotateTransform
                .DOLocalRotate(Vector3.one, _closeTime).SetEase(_closeEasing);
        };
    }

    private void OnPlayerZoneEnter()
    {
        if (!_isUnlocked)
            return;

        _isDispensingToPlayer = true;
        _timer = 0f;
        Debug.Log("PLAYER ENTER");
    }
    
    private void OnPlayerZoneExit()
    {
        if (!_isUnlocked)
            return;

        _isDispensingToPlayer = false;
        _timer = 0f;
        Debug.Log("PLAYER EXIT");
    }
}