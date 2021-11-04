using UnityEngine;

public class LoginManager : Singleton<LoginManager>
{
    public static OAuthLoginManager googlePlayLoginManager = new OAuthLoginManager();
    public static FirebaseLoginManager firebaseLoginManager = new FirebaseLoginManager();

    protected override void Awake()
    {
        dontDestroyOnLoad = true;
        base.Awake();
    }

    public void Start()
    {

        PrintLog.Instance.LogString += "Start LoginOAuth";
        StartLogin();
    }

    public void StartLogin()
    {
        googlePlayLoginManager.OnOAuthAuthenticate(CheckLoginResult);
    }

    /// <summary>
    /// Credential을 가져오기 위해서 먼저 OAuth에 로그인 할 필요가 있음. LoginOAuth() 호출
    /// </summary>
    /// <returns></returns>
    public static void SetFirebaseCredential()
    {
        var authCode = googlePlayLoginManager.AuthCode;
        firebaseLoginManager.GetCredential(authCode);
    }

    /// <summary>
    /// nextAction : SignInFirebase 완료 시 실행할 함수. bool 매개변수는 Authenticate 작업의 성공 여부를 뜻함.
    /// SetFirebaseCredential을 통해 먼저 Credential을 설정할 필요가 있음. SetFirebaseCredential() 호출
    /// </summary>
    /// <param name="callback"></param>
    public static void OnFirebaseSignIn(System.Action<bool> callback)
    {
        firebaseLoginManager.OnFirebaseSignIn(firebaseLoginManager.Credential, callback);
    }

    private void CheckLoginResult(bool result)
    {
        PrintLog.instance.LogString += "Start CheckLoginResult";
        if (result)
        {
            PrintLog.instance.LogString += "[OAuth Login Success]";
            SetFirebaseCredential();
            OnFirebaseSignIn(CheckLoginSuccess);
        }
        else
        {
            PrintLog.instance.LogString += "[Login Failed] : LoginOAuth result is false";
        }
    }

    private void CheckLoginSuccess(bool success)
    {
        PrintLog.instance.LogString += "Start CheckLoginSuccess";

        if (success)
        {
            PrintLog.instance.LogString += $"[Login Success]";
        }
        else
        {
            PrintLog.instance.LogString += $"[Login Failed]";
        }
    }
}
