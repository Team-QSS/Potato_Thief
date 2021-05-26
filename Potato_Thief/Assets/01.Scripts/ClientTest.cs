using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Threading.Tasks;
using System.IO;

public class ClientTest : MonoBehaviour
{
    Firebase.Auth.FirebaseAuth auth = null;
    Firebase.Auth.FirebaseUser user = null;
    string authCode;

    private void Start()
    {

        // user
        // password

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
    .RequestServerAuthCode(false /* Don't force refresh */)
    .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
    }

    public void IsLoginButtonDown()
    {

        Social.localUser.Authenticate((bool success) => {
            if (success)
            {
                authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
            }
        });

        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        Firebase.Auth.Credential credential =
            Firebase.Auth.PlayGamesAuthProvider.GetCredential(authCode);

        auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;

            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });

        user = auth.CurrentUser;
        
        if (user != null)
        {
            string playerName = user.DisplayName;

            // The user's Id, unique to the Firebase project.
            // Do NOT use this value to authenticate with your backend server, if you
            // have one; use User.TokenAsync() instead.
            string uid = user.UserId;
        }
    }

    public void IsLogoutButtonDown()
    {
        auth.SignOut();
    }

}
