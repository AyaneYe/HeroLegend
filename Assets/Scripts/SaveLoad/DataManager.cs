using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    [Header("�¼�����")]
    public VoidEventSO saveDataEvent;

    public List<ISaveable> saveableList = new List<ISaveable>();

    private Data saveData;

    //ͨ��д����ֻ����һ������ģʽ
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        saveData = new Data();
    }

    private void OnEnable()
    {
        saveDataEvent.onEventRaised += Save;
    }

    private void OnDisable()
    {
        saveDataEvent.onEventRaised -= Save;
    }

    public void RegisterSaveData(ISaveable saveable)
    {
        if (!saveableList.Contains(saveable))
        {
            saveableList.Add(saveable);
        }
    }

    public void UnRegisterSaveData(ISaveable saveable)
    {
        saveableList.Remove(saveable);
    }

    public void Save()
    {
        foreach (var saveable in saveableList)
        {
            saveable.GetSaveData(saveData);
        }
        foreach (var item in saveData.characterPosDict)
        {
            Debug.Log(item.Key + " " + item.Value);
        }
    }

    public void Load()
    {

    }
}
