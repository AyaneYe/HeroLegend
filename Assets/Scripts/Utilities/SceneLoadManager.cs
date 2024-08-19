using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 firstPosition;
    [Header("事件监听")]
    public SceneLoadEventSO loadEventSO;
    public GameSceneSO firstLoadScene;
    [Header("广播")]
    public VoidEventSO afterSceneLoad;

    private GameSceneSO currentLoadScene;
    private GameSceneSO SceneToLoad;
    private Vector3 positionToGo;
    private bool fadeScreen;
    private float fadeDuration;
    private bool isLoading;

    private void Awake()
    {
        //现在加载的是哪一个场景
        //currentLoadScene = firstLoadScene;
        //异步叠加加载场景
        //currentLoadScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
    }

    private void Start()
    {
        NewGame();
    }

    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += LoadScene;
    }

    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= LoadScene;
    }

    void NewGame()
    {
        SceneToLoad = firstLoadScene;
        LoadScene(SceneToLoad, firstPosition, true);
    }

    private void LoadScene(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen)
    {
        if (isLoading)
            return;

        isLoading = true;
        //临时存储变量
        SceneToLoad = locationToLoad;
        positionToGo = posToGo;
        this.fadeScreen = fadeScreen;

        //不为空时卸载当前场景
        if (currentLoadScene != null)
        {
            StartCoroutine(UnloadPreviousScene());
        }
        else
        {
            LoadNewScene();
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

        //隐藏玩家
        playerTransform.gameObject.SetActive(false);

        LoadNewScene();
    }

    private void LoadNewScene()
    {
        var loadingOption = SceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        //执行加载完成后的事件
        loadingOption.Completed += OnLoadComplete;
    }

    private void OnLoadComplete(AsyncOperationHandle<SceneInstance> handle)
    {
        //重置当前已加载场景的值，方便后续操作
        currentLoadScene = SceneToLoad;

        //将玩家传送到指定位置
        playerTransform.position = positionToGo;

        //显示玩家
        playerTransform.gameObject.SetActive(true);

        if (fadeScreen)
        {
            //画面过渡
        }

        isLoading = false;

        //执行获取相机边界的事件
        afterSceneLoad.RaiseEvent();
    }
}
