using UnityEngine;

public class UIPanel : MonoBehaviour, IUIPanel
{
    private bool _isActive;
    
    public bool IsActive => _isActive;

    public void Show()
    {
        gameObject.SetActive(true);
        _isActive = true;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        _isActive = false;
    }
}