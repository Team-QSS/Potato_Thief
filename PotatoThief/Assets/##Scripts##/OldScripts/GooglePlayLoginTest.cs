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
    public class GooglePlayLoginTest : Singleton<GooglePlayLoginTest>
    {
        public Firebase.Auth.FirebaseAuth auth;
        public Firebase.Auth.FirebaseUser user;
        public string authCode;

        private void Start()
        {
            Debug.Log("Start()\n");
            GooglePlayLogin();
        }

        private void GooglePlayLogin()
        {
            Debug.Log("GooglePlayLogin()\n");
            var config =
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
                    Debug.Log("Google Login Sucess\n");

                    // O.Auth 로그인에 성공하면 Firebase 로그인 시작
                    FirebaseLogin();
                }
                else
                {
                    Debug.Log("Google Login Failed");
                }

            });
        }
        private void FirebaseLogin()
        {
            Debug.Log("FirebaseLogin()\n");
            auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

            // 2. 왜냐하면 authCode가 ""이기 때문에.
            var credential = Firebase.Auth.PlayGamesAuthProvider.GetCredential(authCode);

            auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    Firebase.Auth.FirebaseUser newUser = task.Result;

                    Debug.Log($"User signed in successfully: {newUser.DisplayName} ({newUser.UserId})");
                    DisplayUserData();
                }
                else if (!credential.IsValid())  // 1. Credential이 유효하지 않음.
                {
                    Debug.Log($"Credential Invalid\n---{authCode}---");
                }
                else if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithCredentialAsync was canceled.");
                }
                else if (task.IsFaulted)
                {
                    Debug.LogError($"SignInWithCredentialAsync encountered an error : {task.Exception}");
                }
            });
        }


        private void DisplayUserData()
        {
            Debug.Log("DisplayUserData()\n");
            user = auth.CurrentUser;

            if (user == null) return;
            
            var playerName = user.DisplayName;

            // The user's Id, unique to the Firebase project.
            // Do NOT use this value to authenticate with your backend server, if you
            // have one; use User.TokenAsync() instead.
            var uid = user.UserId;
            Debug.Log($"UID = {uid}\n");
            Debug.Log($"PlayerName = {playerName}\n");
        }
    }
}