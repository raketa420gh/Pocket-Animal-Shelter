using System;
using UnityEngine;

public class ScreenFader : MonoBehaviour, IScreenFader
{
    [SerializeField] private Canvas _fadeCanvas;
    [SerializeField] private Animator _fadeAnimator;
    [SerializeField] private string _fadedParameterName = "Faded";
    private Action _fadedInCallback;
    private Action _fadedOutCallback;
    
    public bool IsFading { get; private set; }
    
    public void FadeIn(Action fadedInCallback)
    {
        if (IsFading)
            return;

        IsFading = true;
        _fadedInCallback = fadedInCallback;
        _fadeAnimator.SetBool(_fadedParameterName, true);
    }

    public void FadeOut(Action fadedOutCallback)
    {
        if (IsFading)
            return;

        IsFading = true;
        _fadedOutCallback = fadedOutCallback;
        _fadeAnimator.SetBool(_fadedParameterName, false);
    }

    #region AnimationEvents

    private void InvokeFadedInCallback()
    {
        _fadedInCallback?.Invoke();
        _fadedInCallback = null;
        IsFading = false;
    }

    private void InvokeFadedOutCallback()
    {
        _fadedOutCallback?.Invoke();
        _fadedOutCallback = null;
        IsFading = false;
    }

    #endregion
}