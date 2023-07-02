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
using System;
using System.Threading.Tasks;

public class CloudSaveDataGoogle : MonoBehaviour
{
    public HeroSaveData m_HeroSaveData;
    public HeroSaveData m_HeroSaveDataTest;
    public TextMeshProUGUI txt_Authen;
    private string authCode;

    async void Awake()
    {
        try
        {
            Debug.Log("AWAKE TRY");
            await UnityServices.InitializeAsync();
            Debug.Log("AWAKE TRY AFTER");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        if (AuthenticationService.Instance.SessionTokenExists)
        {
            SignInWithAnonymouslyAsync();
        }
        else
        {
            //TODO: Handle Session Expire
        }

    }

    public void GoogleSignin(Action _successCallback)
    {
        Task<GoogleSignInUser> signIn = GoogleSignIn.DefaultInstance.SignIn();
        signIn.ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
            }
            else if (task.IsFaulted)
            {
            }
            else
            {
                SignInWithGoogleAsync(signIn.Result.IdToken, _successCallback);
            }
        });
    }

    async Task SignInWithGoogleAsync(string idToken, Action _successCallback = null)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithGoogleAsync(idToken);
            Debug.Log("SignIn is successful.");
            _successCallback?.Invoke();
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

    async Task SignInWithAnonymouslyAsync()
    {
        Debug.Log("SIGN IN WITH ANONYMOUS");
        try
        {
            Debug.Log("SIGN IN WITH ANONYMOUS TRYYYYYYYYYYYYYYYYY");
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            //UIManager.Instance.HideSigninPopup(false);
            Debug.Log("SignIn is successful.");
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
}
