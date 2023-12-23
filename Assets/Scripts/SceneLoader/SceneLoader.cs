using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class SceneLoader : ISceneLoader
{
    private IScreenFader _screenFader;
    private bool _isLoading;

    [Inject]
    public void Construct(IScreenFader screenFader)
    {
        _screenFader = screenFader;
    }

    public void LoadScene(string sceneName)
    {
        if (_isLoading)
            return;

        string currentSceneName = SceneManager.GetActiveScene().name;

        if (sceneName == currentSceneName)
            throw new Exception("You are trying to load already loaded scene.");

        LoadSceneAsync(sceneName);
    }

    private async UniTask LoadSceneAsync(string sceneName)
    {
        _isLoading = true;
        bool waitFading = true;
        _screenFader.FadeIn(() => waitFading = false);

        await UniTask.WaitUntil(() => !waitFading);

        AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(sceneName);
        loadSceneOperation.allowSceneActivation = false;

        await UniTask.WaitUntil(() => loadSceneOperation.progress >= 0.9f);

        loadSceneOperation.allowSceneActivation = true;
        waitFading = true;
        _screenFader.FadeOut(() => waitFading = false);

        await UniTask.WaitUntil(() => !waitFading);

        _isLoading = false;
    }
}