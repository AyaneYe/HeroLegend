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
    [Header("����")]
    public Vector3 firstPosition;
    public Vector3 menuPosition;
    public GameSceneSO firstLoadScene;
    public GameSceneSO menuScene;
    [Header("�¼�����")]
    public SceneLoadEventSO loadEventSO;
    public VoidEventSO newGameEvent;
    [Header("�㲥")]
    public VoidEventSO afterSceneLoad;
    public FadeEventSO fadeEvent;
    public SceneLoadEventSO sceneUnloadedEvent;

    private GameSceneSO currentLoadScene;
    private GameSceneSO SceneToLoad;
    private Vector3 positionToGo;
    private bool fadeScreen;
    public float fadeDuration;
    private bool isLoading;

    private void Awake()
    {
        //���ڼ��ص�����һ������
        //currentLoadScene = firstLoadScene;
        //�첽���Ӽ��س���
        //currentLoadScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
    }

    private void Start()
    {
        //NewGame();
        loadEventSO.RaiseLoadRequestEvent(menuScene, menuPosition, true);
    }

    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += LoadScene;
        newGameEvent.onEventRaised += NewGame;
    }

    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= LoadScene;
        newGameEvent.onEventRaised -= NewGame;
    }

    void NewGame()
    {
        SceneToLoad = firstLoadScene;
        //LoadScene(SceneToLoad, firstPosition, true);
        loadEventSO.RaiseLoadRequestEvent(SceneToLoad, firstPosition, true);
    }

    private void LoadScene(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen)
    {
        if (isLoading)
            return;

        isLoading = true;
        //��ʱ�洢����
        SceneToLoad = locationToLoad;
        positionToGo = posToGo;
        this.fadeScreen = fadeScreen;

        //��Ϊ��ʱж�ص�ǰ����
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
        //���Ź��ɶ���
        if (fadeScreen)
        {
            fadeEvent.FadeIn(fadeDuration);
        }

        //�첽�ȴ�������
        yield return new WaitForSeconds(fadeDuration);

        //ж�س���������Ѫ����ʾ
        sceneUnloadedEvent.RaiseLoadRequestEvent(SceneToLoad, positionToGo, true);

        yield return currentLoadScene.sceneReference.UnLoadScene();

        //�������
        playerTransform.gameObject.SetActive(false);

        LoadNewScene();
    }

    private void LoadNewScene()
    {
        var loadingOption = SceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        //ִ�м�����ɺ���¼�
        loadingOption.Completed += OnLoadComplete;
    }

    private void OnLoadComplete(AsyncOperationHandle<SceneInstance> handle)
    {
        //���õ�ǰ�Ѽ��س�����ֵ�������������
        currentLoadScene = SceneToLoad;

        //����Ҵ��͵�ָ��λ��
        playerTransform.position = positionToGo;

        //��ʾ���
        playerTransform.gameObject.SetActive(true);

        if (fadeScreen)
        {
            fadeEvent.FadeOut(fadeDuration);
        }

        isLoading = false;
        if (currentLoadScene.SceneType == SceneType.Location)
        {
            //ִ�л�ȡ����߽���¼�
            afterSceneLoad.RaiseEvent();
        }
    }
}
