using Firebase.Auth;
using UnityEngine;
public class FirebaseLoginManager
{
    public FirebaseAuth Auth { get; set; }
    public FirebaseUser User { get; set; }
    public Credential Credential { get; set; }

    public Credential GetCredential(string authCode)
    {
        PrintLog.instance.LogString += "Start GetCredential()";
        Debug.Log("Start GetCredential()");

        Auth = FirebaseAuth.DefaultInstance;
        Credential = PlayGamesAuthProvider.GetCredential(authCode);

        if (Credential == null)
        {
            PrintLog.instance.LogString += "[Credential Failed] : credential is null";
            Debug.Log("[Credential Failed] : credential is null");
        }

        PrintLog.instance.LogString += $"Provider = {Credential.Provider}";
        Debug.Log($"Provider = {Credential.Provider}");
        return Credential;
    }

    /// <summary>
    /// nextAction : Login 완료 시 실행할 함수. bool 매개변수는 SignInWithCredentialAsync 작업의 성공 여부를 뜻함.
    /// </summary>
    /// <param name="callback"></param>
    public void OnFirebaseSignIn(Credential credential, System.Action<bool> callback)
    {
        PrintLog.instance.LogString += "Start LoginFirebase()";
        Debug.Log("TRY SIGNIN");

        // bool isCompleted = false;


        Auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                PrintLog.instance.LogString += "SignInWithCredentialAsync was canceled.";
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                PrintLog.instance.LogString += "SignInWithCredentialAsync encountered an error: " + task.Exception;

                return;
            }

            FirebaseUser newUser = task.Result;
            Debug.Log($"User signed in successfully: {newUser.DisplayName} ({newUser.UserId})");
            PrintLog.instance.LogString += $"User signed in successfully: {newUser.DisplayName} ({newUser.UserId})";

        });


        // callback(isCompleted);
    }

    /// <summary>
    /// 로그인이 성공적으로 완료 되었는지 확인하기 위한 예제 함수
    /// </summary>
    public void DisplayUserData(bool success)
    {
        Debug.Log("Start DisplayUserData()");
        PrintLog.instance.LogString += "Start DisplayUserData()";

        if (!success)
        {
            Debug.Log("[SignIn Failed] : success is false");
            PrintLog.instance.LogString += "[SignIn Failed] : success is false";
            return;
        }

        User = Auth.CurrentUser;

        if (User != null)
        {
            string playerName = User.DisplayName;

            // The user's Id, unique to the Firebase project.
            // Do NOT use this value to authenticate with your backend server, if you
            // have one; use User.TokenAsync() instead.
            string uid = User.UserId;
            Debug.Log($"UID = {uid}");
            Debug.Log($"PlayerName = {playerName}");
            PrintLog.instance.LogString += $"UID = {uid}";
            PrintLog.instance.LogString += $"PlayerName = {playerName}";
        }
        else
        {
            Debug.Log("[SignIn Failed] : User is null");
            PrintLog.instance.LogString += "[SignIn Failed] : User is null";
        }
    }
}
