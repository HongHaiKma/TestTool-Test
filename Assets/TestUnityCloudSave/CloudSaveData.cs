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
        try
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        
        // Debug.Log("Singed in");
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
        // HeroSave heroSave = new HeroSave();
        var heroObject = JsonMapper.ToObject<List<HeroSave>>(savedData["HeroSave"]);
        Debug.Log($"Count: {heroObject.Count}");
        for (int i = 0; i < heroObject.Count; i++)
        {
            Debug.Log($"ID: {heroObject[i].m_Id}");
            Debug.Log($"Name: {heroObject[i].m_Name}");
            for (int j = 0; j < heroObject[i].m_Weapons.Count; j++)
            {
                Debug.Log($"Name: {heroObject[i].m_Weapons[j].m_Id}");
                Debug.Log($"Name: {heroObject[i].m_Weapons[j].m_WeaponType}");
            }
        }
        // Debug.Log($"Count: {heroObject.Value.Count}");
        // for (int i = 0; i < heroObject.Value.Count; i++)
        // {
        //     Debug.Log($"ID: {heroObject.Value[i].m_Id}");
        //     Debug.Log($"Name: {heroObject.Value[i].m_Name}");
        // }
        // m_HeroSaveDataTest.Value = heroObject.Value;
    }
}
