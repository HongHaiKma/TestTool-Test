using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using Sirenix.OdinInspector;
using ScriptableObjectArchitecture;
using TMPro;
using Cysharp.Threading.Tasks;
using Unity.Services.CloudSave;
using System.Collections.Generic;
using LitJson;
using UnityEngine.UI;
using Unity.Services.RemoteConfig;
using Newtonsoft.Json;

public class GPManager : MonoBehaviour
{
    public HeroSaveData m_HeroSaveData;
    public HeroSaveData m_HeroSaveDataTest;
    public TextMeshProUGUI txt_Authen;
    private string authCode;

    public Button btn_Save;
    public Button btn_Load;
    public Button btn_HeroSaveRemote;


    public string Token;
    public string Error;

    void Awake()
    {
        PlayGamesPlatform.Activate();
    }

    async void Start()
    {
        btn_Save.onClick.AddListener(() => SavePlayerData());
        btn_Load.onClick.AddListener(() => ReadPlayerData());
        btn_HeroSaveRemote.onClick.AddListener(() => GetRemoteHeroSave());
        await UnityServices.InitializeAsync();
        Debug.Log("Unity Gaming Service Init");
        await LoginGooglePlayGames();
        Debug.Log("LoginGooglePlayGames");
        await SignInWithGooglePlayGamesAsync(Token);
        Debug.Log("SignInWithGooglePlayGamesAsync");
    }
    //Fetch the Token / Auth code
    public Task LoginGooglePlayGames()
    {
        var tcs = new TaskCompletionSource<object>();
        PlayGamesPlatform.Instance.Authenticate((success) =>
        {
            if (success == SignInStatus.Success)
            {
                Debug.Log("Login with Google Play games successful.");
                PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                {
                    Debug.Log("Authorization code: " + code);
                    Token = code;
                    // This token serves as an example to be used for SignInWithGooglePlayGames
                    tcs.SetResult(null);
                });
            }
            else
            {
                Error = "Failed to retrieve Google play games authorization code";
                Debug.Log("Login Unsuccessful");
                tcs.SetException(new Exception("Failed"));
            }
        });
        return tcs.Task;
    }


    async Task SignInWithGooglePlayGamesAsync(string authCode)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(authCode);
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}"); //Display the Unity Authentication PlayerID
            Debug.Log("SignIn is successful.");

            GetRemoteConfig();
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
    }

    public struct userAttributes { }
    public struct appAttributes { }

    void GetRemoteConfig()
    {
        RemoteConfigService.Instance.FetchCompleted += ApplyRemoteSettings;
        RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
    }

    public TextAsset json1;
    void ApplyRemoteSettings(ConfigResponse configResponse)
    {
        Debug.Log("RemoteConfigService.Instance.appConfig fetched: " + RemoteConfigService.Instance.appConfig.config.ToString());

        // var heroObject = JsonMapper.ToS<List<HeroSave>>(savedData["HeroSave"]);
    }

    public void GetRemoteHeroSave()
    {
        Debug.Log("RemoteConfigService.Instance.appConfig fetched: " + RemoteConfigService.Instance.appConfig.config.ToString());
        TestJsonHeroSave(RemoteConfigService.Instance.appConfig.config.ToString());
    }

    [Button]
    public void TestJsonHeroSave(string _json)
    {
        // WeaponData wd = JsonConvert.DeserializeObject<WeaponData>(serialized);
        // var heroObject = JsonConvert.DeserializeObject<List<HeroSave>>(json1.ToString());
        var heroObject = JsonMapper.ToObject<List<HeroSave>>(_json.ToString());
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
    }

    [Button]
    public void SetHeroSaveValue()
    {
        // WeaponData wd = JsonConvert.DeserializeObject<WeaponData>(serialized);
        // var heroObject = JsonConvert.DeserializeObject<List<HeroSave>>(json1.ToString());
        var heroObject = JsonMapper.ToObject<List<HeroSave>>(json1.ToString());
        m_HeroSaveData.SetValue(heroObject);
    }


    [Button]
    public void TestHero()
    {
        HeroSave heroSave = m_HeroSaveData.FindHero(1);
        Debug.Log("Name: " + heroSave.m_Name);
    }

    public void SavePlayerData()
    {
        AsyncSavePlayerData();
    }

    public void ReadPlayerData()
    {
        AsyncReadPlayerData();
    }

    [Button]
    public async UniTask AsyncSavePlayerData()
    {
        Debug.Log("SAVEEEEEEEEEEE");
        var savedData = new Dictionary<string, object>();
        savedData.Add("HeroSave", m_HeroSaveData.Value);
        await CloudSaveService.Instance.Data.ForceSaveAsync(savedData);
        Debug.Log("SAVE SUCCEED");
    }

    [Button]
    public async UniTask AsyncReadPlayerData()
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
    }
}