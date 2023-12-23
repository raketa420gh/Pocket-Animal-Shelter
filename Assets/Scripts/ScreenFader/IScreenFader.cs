using System;

public interface IScreenFader
{
    void FadeIn(Action fadedInCallback);
    void FadeOut(Action fadedOutCallback);
}