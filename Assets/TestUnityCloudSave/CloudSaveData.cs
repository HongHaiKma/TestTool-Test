using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using Sirenix.Serialization;
using LitJson;

public class CloudSaveData : MonoBehaviour
{
    public HeroSaveData m_HeroSaveData;
    public HeroSaveData m_HeroSaveDataTest;

    private async UniTask Start() 
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        Debug.Log("Singed in");
    }

    [Button]
    public void TestHero()
    {
        HeroSave heroSave = m_HeroSaveData.FindHero(1);
        Debug.Log("Name: " + heroSave.m_Name);
    }

    [Button]
    public async UniTask SavePlayerData()
    {
        var savedData = new Dictionary<string, object>();
        savedData.Add("HeroSave", m_HeroSaveData.Value);
        await CloudSaveService.Instance.Data.ForceSaveAsync(savedData);
    }

    [Button]
    public async UniTask ReadPlayerData()
    {
        Dictionary<string, string> savedData = await CloudSaveService.Instance.Data.LoadAllAsync();
        Debug.Log("HeroSave: " + savedData["HeroSave"]);
        HeroSave heroSave = new HeroSave();
        // JsonMapper.ToObject<HeroSave>(m_HeroSaveDataTest.Value);
        var heroObject = JsonMapper.ToObject<Collection<HeroSave>>(savedData["HeroSave"]);
        Debug.Log($"Count: {heroObject.Value.Count}");
        // for (int i = 0; i < heroObject.Value.Count; i++)
        // {
        //     Debug.Log($"ID: {heroObject.Value[i].m_Id}");
        //     Debug.Log($"Name: {heroObject.Value[i].m_Name}");
        // }
        // m_HeroSaveDataTest.Value = heroObject.Value;
    }
}
