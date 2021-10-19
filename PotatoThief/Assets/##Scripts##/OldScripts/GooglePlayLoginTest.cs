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
            PrintLog.instance.LogString = "Start()\n";
            GooglePlayLogin();
        }

        private void GooglePlayLogin()
        {
            PrintLog.instance.LogString += "GooglePlayLogin()\n";
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
                    PrintLog.instance.LogString += "Google Login Sucess\n";

                    // O.Auth 로그인에 성공하면 Firebase 로그인 시작
                    FirebaseLogin();
                }
                else
                {
                    PrintLog.instance.LogString = "Google Login Failed";
                }

            });
        }
        private void FirebaseLogin()
        {
            PrintLog.instance.LogString += "FirebaseLogin()\n";
            auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

            // 2. 왜냐하면 authCode가 ""이기 때문에.
            var credential = Firebase.Auth.PlayGamesAuthProvider.GetCredential(authCode);

            auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    Firebase.Auth.FirebaseUser newUser = task.Result;

                    Debug.Log($"User signed in successfully: {newUser.DisplayName} ({newUser.UserId})");
                    PrintLog.instance.LogString += $"User signed in successfully: {newUser.DisplayName} ({newUser.UserId})\n";
                    DisplayUserData();
                }
                else if (!credential.IsValid())  // 1. Credential이 유효하지 않음.
                {
                    PrintLog.instance.LogString = $"Credential Invalid\n---{authCode}---";
                    return;
                }
                else if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithCredentialAsync was canceled.");
                    PrintLog.instance.LogString = "SignInWithCredentialAsync was canceled.";
                    return;
                }
                else if (task.IsFaulted)
                {
                    Debug.LogError($"SignInWithCredentialAsync encountered an error : {task.Exception}");
                    PrintLog.instance.LogString = $"SignInWithCredentialAsync encountered an error: {task.Exception}";
                    return;
                }
            });
        }


        private void DisplayUserData()
        {
            PrintLog.instance.LogString += "DisplayUserData()\n";
            user = auth.CurrentUser;

            if (user != null)
            {
                string playerName = user.DisplayName;

                // The user's Id, unique to the Firebase project.
                // Do NOT use this value to authenticate with your backend server, if you
                // have one; use User.TokenAsync() instead.
                string uid = user.UserId;
                PrintLog.instance.LogString += $"UID = {uid}\n";
                PrintLog.instance.LogString += $"PlayerName = {playerName}\n";
            }
        }
    }
}