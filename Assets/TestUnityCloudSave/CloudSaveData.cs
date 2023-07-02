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
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
using Google;

public class CloudSaveData : MonoBehaviour
{
    public HeroSaveData m_HeroSaveData;
    public HeroSaveData m_HeroSaveDataTest;
    public TextMeshProUGUI txt_Authen;
    private string authCode;

    PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
    .RequestServerAuthCode(false /* Don't force refresh */)
    .Build();

    async UniTask Start()
    {
        await UnityServices.InitializeAsync();
        //Initialize PlayGamesPlatform
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        LoginGooglePlayGames();
        // SignInCachedUser();
    }

    // async UniTask SignInCachedUser()
    // {
    //     // Check if a cached player already exists by checking if the session token exists
    //     if (!AuthenticationService.Instance.SessionTokenExists) 
    //     {
    //         // if not, then do nothing
    //         return;
    //     }

    //     // Sign in Anonymously
    //     // This call will sign in the cached player.
    //     try
    //     {
    //         await AuthenticationService.Instance.SignInAnonymouslyAsync();
    //         Debug.Log("Sign in anonymously succeeded!");

    //         // Shows how to get the playerID
    //         Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");   
    //     }
    //     catch (AuthenticationException ex)
    //     {
    //         // Compare error code to AuthenticationErrorCodes
    //         // Notify the player with the proper error message
    //         Debug.LogException(ex);
    //     }
    //     catch (RequestFailedException exception)
    //     {
    //         // Compare error code to CommonErrorCodes
    //         // Notify the player with the proper error message
    //         Debug.LogException(exception);
    //     }  
    // }

    // private async UniTask Start() 
    // {
    //     try
    //     {
    //         await UnityServices.InitializeAsync();
    //         await AuthenticationService.Instance.SignInAnonymouslyAsync();
    //         Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
    //         Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
    //     }
    //     catch (AuthenticationException ex)
    //     {
    //         // Compare error code to AuthenticationErrorCodes
    //         // Notify the player with the proper error message
    //         Debug.LogException(ex);
    //     }
    //     catch (RequestFailedException ex)
    //     {
    //         // Compare error code to CommonErrorCodes
    //         // Notify the player with the proper error message
    //         Debug.LogException(ex);
    //     }
    // }

    public void LoginGooglePlayGames()
    {
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (success) =>
        {
            if (success == SignInStatus.Success)
            {
                txt_Authen.text = $"111111111";
            }
            else
            {
                txt_Authen.text = $"000000000";
            }
        });

        // PlayGamesPlatform.Instance.Authenticate((success) =>
        // {
        //     Social.Active.localUser.Authenticate((bool success) =>
        //     {
        //         if (success)
        //         {
        //             authCode = PlayGamesPlatform.Instance.GetIdToken();
        //             Debug.Log("AuthCode: " + authCode);
        //             txt_Authen.text = $"Successful";
        //             SignInWithGoogleAsync(authCode);

        //             // PlayGamesPlatform.Instance.RequestPermission(config, code =>
        //             // {
        //             //     SignInWithGoogleAsync(authCode);
        //             //     txt_Authen.text = $"Successful";
        //             // });
        //         }
        //         else
        //         {
        //             Debug.Log("Login Unsuccessful");
        //         }
        //     });
        //     //             if (success == SignInStatus.Success)
        //     //             {
        //     //                 Debug.Log("Login with Google Play games successful.");

        //     //                 PlayGamesPlatform.Instance.RequestPermission(true, code =>
        //     //                 {
        //     //                     SignInWithGoogleAsync(code);
        //     //                     // Debug.Log("Authorization code: " + code);
        //     //                     // Token = code;
        //     //                     // txt_Authen.text = $"Successful";
        //     // // This token serves as an example to be used for SignInWithGooglePlayGames
        //     //                 });
        //     //             }
        //     //             else
        //     //             {
        //     //                 // Error = "Failed to retrieve Google play games authorization code";
        //     //                 // Debug.Log("Login Unsuccessful");
        //     //                 // txt_Authen.text = $"Unsuccessful";
        //     //             }
        // });
    }

    async UniTask SignInWithGoogleAsync(string _authCode)
    {
        try
        {
            // SignInOptions options = new SignInOptions();
            // options.CreateAccount = true;
            Debug.Log("AuthCode: " + _authCode);
            await AuthenticationService.Instance.SignInWithGoogleAsync(_authCode);
            txt_Authen.text = "Successful";
            Debug.Log("SignIn is successful.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.Log("Failedddddddd");
            txt_Authen.text = "Unsuccessful";
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.Log("Failedddddddd");
            txt_Authen.text = "Unsuccessful";
            Debug.LogException(ex);
        }
    }

    // [Button]
    // async UniTask LinkWithGooglePlayGamesAsync(string authCode)
    // {
    //     try
    //     {
    //         await AuthenticationService.Instance.LinkWithGoogleAsync(authCode);
    //         Debug.Log("Link is successful.");
    //     }
    //     catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
    //     {
    //         // Prompt the player with an error message.
    //         Debug.LogError("This user is already linked with another account. Log in instead.");
    //     }

    //     catch (AuthenticationException ex)
    //     {
    //         // Compare error code to AuthenticationErrorCodes
    //         // Notify the player with the proper error message
    //         Debug.LogException(ex);
    //     }
    //     catch (RequestFailedException ex)
    //     {
    //         // Compare error code to CommonErrorCodes
    //         // Notify the player with the proper error message
    //         Debug.LogException(ex);
    //     }
    // }

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
        var savedData = new Dictionary<string, object>();
        savedData.Add("HeroSave", m_HeroSaveData.Value);
        await CloudSaveService.Instance.Data.ForceSaveAsync(savedData);
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
        // Debug.Log($"Count: {heroObject.Value.Count}");
        // for (int i = 0; i < heroObject.Value.Count; i++)
        // {
        //     Debug.Log($"ID: {heroObject.Value[i].m_Id}");
        //     Debug.Log($"Name: {heroObject.Value[i].m_Name}");
        // }
        // m_HeroSaveDataTest.Value = heroObject.Value;
    }
}
