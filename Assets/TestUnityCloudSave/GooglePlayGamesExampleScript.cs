using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using TMPro;

public class GooglePlayGamesExampleScript : MonoBehaviour
{
    public string Token;
    public string Error;
    public TextMeshProUGUI txt_Authen;

    void Awake()
    {
        //Initialize PlayGamesPlatform
        PlayGamesPlatform.Activate();
        LoginGooglePlayGames();
    }

    public void LoginGooglePlayGames()
    {
        PlayGamesPlatform.Instance.Authenticate((success) =>
        {
            if (success == SignInStatus.Success)
            {
                Debug.Log("Login with Google Play games successful.");

                PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                {
                    // Debug.Log("Authorization code: " + code);
                    Token = code;
                    txt_Authen.text = $"Successful";
// This token serves as an example to be used for SignInWithGooglePlayGames
                });
            }
            else
            {
                // Error = "Failed to retrieve Google play games authorization code";
                // Debug.Log("Login Unsuccessful");
                txt_Authen.text = $"Unsuccessful";
            }
        });
    }
}