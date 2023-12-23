using UnityEngine;

public class PurchaseAreaFactory : MonoBehaviour
{
    [SerializeField] private PurchaseAreaPool _purchaseAreaSPool;
    [SerializeField] private PurchaseAreaPool _purchaseAreaMPool;

    public void Initialize()
    {
        _purchaseAreaSPool.Initialize();
        _purchaseAreaMPool.Initialize();
    }

    public PurchaseArea CreatePurchaseAreaS(Vector3 position)
    {
        PurchaseArea purchaseArea = _purchaseAreaSPool.GetElement();
        purchaseArea.transform.position = position;

        return purchaseArea;
    }

    public PurchaseArea CreatePurchaseAreaM(Vector3 position)
    {
        PurchaseArea purchaseArea = _purchaseAreaMPool.GetElement();
        purchaseArea.transform.position = position;

        return purchaseArea;
    }
}