using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;

namespace Login
{
    public class FirebaseLoginManager
    {
        public FirebaseAuth Auth { get; set; }
        public FirebaseUser User { get; set; }
        public Credential Credential { get; set; }

        public Credential GetCredential(string authCode)
        {
            Debug.Log("Start GetCredential()");

            Auth = FirebaseAuth.DefaultInstance;
            Credential = PlayGamesAuthProvider.GetCredential(authCode);

            if (Credential == null)
            {
                Debug.Log("[Credential Failed] : credential is null");
            }

            Debug.Log($"Provider = {Credential.Provider}");
            return Credential;
        }

        /// <summary>
        /// nextAction : Login 완료 시 실행할 함수. bool 매개변수는 SignInWithCredentialAsync 작업의 성공 여부를 뜻함.
        /// </summary>
        /// <param name="callback"></param>
        public void OnFirebaseSignIn(Credential credential, System.Action<bool> callback)
        {
            Debug.Log("TRY SIGNIN");

            // bool isCompleted = false;


            Auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    User = task.Result;
                    Debug.Log($"User signed in successfully: {User.DisplayName} ({User.UserId})");
                    callback(task.IsCompleted);
                }
                else if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithCredentialAsync was canceled.");
                }
                else if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                }
            });


            // callback(isCompleted);
        }

        /// <summary>
        /// 로그인이 성공적으로 완료 되었는지 확인하기 위한 예제 함수
        /// </summary>
        public void DisplayUserData(bool success)
        {
            Debug.Log("Start DisplayUserData()");

            if (!success)
            {
                Debug.Log("[SignIn Failed] : success is false");
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
            }
            else
            {
                Debug.Log("[SignIn Failed] : User is null");
            }
        }
    }
}