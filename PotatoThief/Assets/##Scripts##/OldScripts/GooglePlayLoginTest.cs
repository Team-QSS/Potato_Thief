using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Threading.Tasks;
using System.IO;

namespace Ws_Peroth
{
    [RequireComponent(typeof(GooglePlayLoginTest))]
    public class GooglePlayLoginTest : Singleton<GooglePlayLoginTest>
    {
        public Firebase.Auth.FirebaseAuth auth;
        public Firebase.Auth.FirebaseUser user;
        public string authCode;

        private void Start()
        {
            auth = null;
            user = null;
            authCode = "";

            PlayGamesClientConfiguration config =
                new PlayGamesClientConfiguration.Builder()
                .RequestServerAuthCode(false /* Don't force refresh */)
                .Build();

            PlayGamesPlatform.InitializeInstance(config);
            var x = PlayGamesPlatform.Activate();
            // 4. 클래스 내부 설명읽기

            x.Authenticate((bool success) =>
            {
                if (success)    // 3. 왜냐하면 success == false이기 때문에.
                {
                    authCode =
                    PlayGamesPlatform.Instance.GetServerAuthCode();

                    PrintLog.str = "Google Login Sucess";
                }
                else{
                    PrintLog.str = "Google Login Failed";
                }

            });

            auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

            Firebase.Auth.Credential credential =       // 2. 왜냐하면 authCode가 ""이기 때문에.
                Firebase.Auth.PlayGamesAuthProvider.GetCredential(authCode);

            auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
            {
                if (!credential.IsValid())  // 1. Credential이 유효하지 않음.
                {
                    PrintLog.str = $"Credential Invalid\n---{authCode}---";
                    return;
                }
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithCredentialAsync was canceled.");
                    PrintLog.str = "SignInWithCredentialAsync was canceled.";
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError($"SignInWithCredentialAsync encountered an error : {task.Exception}");
                    PrintLog.str = $"SignInWithCredentialAsync encountered an error: {task.Exception}";
                    return;
                }

                Firebase.Auth.FirebaseUser newUser = task.Result;

                Debug.Log($"User signed in successfully: {newUser.DisplayName} ({newUser.UserId})");
                PrintLog.str = $"User signed in successfully: {newUser.DisplayName} ({newUser.UserId})";
            });

            user = auth.CurrentUser;

            if (user != null)
            {
                string playerName = user.DisplayName;

                // The user's Id, unique to the Firebase project.
                // Do NOT use this value to authenticate with your backend server, if you
                // have one; use User.TokenAsync() instead.
                string uid = user.UserId;
                PrintLog.str = uid;
            }
        }
    }
}