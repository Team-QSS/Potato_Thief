using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Firebase.Auth;
using UnityEngine.UI;


public class AuthManager : MonoBehaviour
{
    public Text LogText;

    private FirebaseAuth auth;
    private FirebaseUser user;
    
    private string playerName;
    private string uid;
    
    private string authCode;
    private Credential credential;

    private FirebaseUser newUser;
    
    void Start()
    {
        // 사용자가 처음으로 로그인하면 신규 사용자 계정이 생성되어 사용자의 Play 게임 ID에 연결됩니다.
        // 이 신규 계정은 Firebase 프로젝트에 저장되며 프로젝트의 모든 앱에서 사용자 식별에 사용할 수 있습니다.
        // 게임에서 Firebase.Auth.FirebaseUser 객체로부터 사용자의 Firebase UID를 가져올 수 있습니다.
        auth = FirebaseAuth.DefaultInstance;
        //user = auth.CurrentUser;
        LogText.text += "";
        
        PlayGamesPlatformInit();
        
        /*
        if (user != null) {
            playerName = user.DisplayName;

            // The user's Id, unique to the Firebase project.
            // Do NOT use this value to authenticate with your backend server, if you
            // have one; use User.TokenAsync() instead.
            uid = user.UserId;
            LogText.text += $"PlayerName : {playerName}\nuid : {uid}";
        }
        else
        {
            LogText.text += "user is null";
        }
        */
    }

    void PlayGamesPlatformInit()
    {
        //* 게임에서 RequestServerAuthCode 설정을 사용 설정하여 Play 게임 클라이언트를 구성합니다.
        // PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        //     .RequestServerAuthCode(false /* Don't force refresh */)
        //     .Build();
        // PlayGamesPlatform.InitializeInstance(config);
        // // Play Games 플랫폼을 활성화 -> Social.localUser.Authenticate같이 Social.Active에서 메서드 호출 가능
        // PlayGamesPlatform.Activate();
        //*
    }
    
    public void LogIn()
    {
        // 플레이어가 Play 게임으로 로그인한 후 로그인 지속 핸들러에서 플레이어 계정의 인증 코드를 가져옵니다
        Social.localUser.Authenticate(success => {
            Debug.Log(success ? "// Logged in successfully" : "// Failed to login");
            LogText.text = success ? "Logged in successfully" : "Failed to login";
            
            if (!success) return;
            // authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
            LogText.text += $"\nAuthCode : {authCode}";
        });

        // 그런 다음 Play 게임 서비스의 인증 코드를 Firebase 사용자 인증 정보로 교환하고,
        credential = PlayGamesAuthProvider.GetCredential(authCode);

        // Firebase 사용자 인증 정보를 사용하여 플레이어를 인증합니다
        auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError("// SignInWithCredentialAsync was canceled.");
                LogText.text += "\nSignInWithCredentialAsync was canceled.";
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError($"// SignInWithCredentialAsync encountered an error: {task.Exception}");
                LogText.text += $"\nSignInWithCredentialAsync encountered an error: {task.Exception}";
                return;
            }

            newUser = task.Result;
            Debug.Log($"// User signed in successfully: {newUser.DisplayName} ({newUser.UserId})");
            LogText.text += $"\nUser signed in successfully: {newUser.DisplayName} ({newUser.UserId})";
        });
    }

    public void LogOut()
    {
        auth.SignOut();
        Debug.Log("// User signed out successfully");
        LogText.text += "\nUser signed out successfully";
    }
}
