using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    [Header("�¼�����")]
    public SceneLoadEventSO loadEventSO;
    public GameSceneSO firstLoadScene;

    private GameSceneSO currentLoadScene;
    private GameSceneSO SceneToLoad;
    private Vector3 positionToGo;
    private bool fadeScreen;
    private float fadeDuration;

    private void Awake()
    {
        //�첽���Ӽ��ص�һ������
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
        //��ʱ�洢����
        SceneToLoad = locationToLoad;
        positionToGo = posToGo;
        this.fadeScreen = fadeScreen;

        //��Ϊ��ʱж�ص�ǰ����
        if (currentLoadScene != null)
        {
            StartCoroutine(UnloadPreviousScene());
        }
    }

    private IEnumerator UnloadPreviousScene()
    {
        //���Ź��ɶ���
        if (fadeScreen)
        {
            //�������
        }

        //�첽�ȴ�������
        yield return new WaitForSeconds(fadeDuration);

        yield return currentLoadScene.sceneReference.UnLoadScene();

        LoadNewScene();
    }

    private void LoadNewScene()
    {
        SceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
    }
}
