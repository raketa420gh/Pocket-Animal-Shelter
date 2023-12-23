public interface IUIPanel
{
    bool IsActive { get; }
    
    void Show();
    void Hide();
}