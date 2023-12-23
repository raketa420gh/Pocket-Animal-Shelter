using UnityEngine;

[System.Serializable]
public class PurchaseAreaCase
{
    [SerializeField] Transform _purchaseAreaTransform;
    [SerializeField] LevelZone _linkedLevelZone;
    
    public Transform PurchaseAreaTransform => _purchaseAreaTransform;
    public LevelZone LinkedLevelZone => _linkedLevelZone;
}