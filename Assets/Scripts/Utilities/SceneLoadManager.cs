using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    [Header("事件监听")]
    public SceneLoadEventSO loadEventSO;
    public GameSceneSO firstLoadScene;

    private GameSceneSO currentLoadScene;
    private GameSceneSO SceneToLoad;
    private Vector3 positionToGo;
    private bool fadeScreen;
    private float fadeDuration;

    private void Awake()
    {
        //异步叠加加载第一个场景
        currentLoadScene = firstLoadScene;
        currentLoadScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
    }

    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += LoadScene;
    }

    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= LoadScene;
    }

    private void LoadScene(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen)
    {
        //临时存储变量
        SceneToLoad = locationToLoad;
        positionToGo = posToGo;
        this.fadeScreen = fadeScreen;

        //不为空时卸载当前场景
        if (currentLoadScene != null)
        {
            StartCoroutine(UnloadPreviousScene());
        }
    }

    private IEnumerator UnloadPreviousScene()
    {
        //播放过渡动画
        if (fadeScreen)
        {
            //画面过渡
        }

        //异步等待并加载
        yield return new WaitForSeconds(fadeDuration);

        yield return currentLoadScene.sceneReference.UnLoadScene();

        LoadNewScene();
    }

    private void LoadNewScene()
    {
        SceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
    }
}
