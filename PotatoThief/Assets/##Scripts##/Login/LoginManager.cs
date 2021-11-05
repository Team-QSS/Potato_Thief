﻿using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Login
{
    public class LoginManager : Singleton<LoginManager>
    {
        private static bool _isStart;
        private static BoolReactiveProperty _isSigned;
        
        public static OAuthLoginManager googlePlayLoginManager = new OAuthLoginManager();
        public static FirebaseLoginManager firebaseLoginManager = new FirebaseLoginManager();

        protected override void Awake()
        {
            _isStart = false;
            dontDestroyOnLoad = true;
            base.Awake();
        }

        public void StartButtonDown()
        {
            _isStart = true;
            if (_isSigned.Value)
            {
                MoveScene();
            }
        }
        
        public void Start()
        {
            _isSigned = new BoolReactiveProperty(false);

            _isSigned
                .Where(x => _isStart && x)
                .Subscribe(_ =>
                {
                    Debug.Log("[ReactiveProperty] Call Property");
                    _isStart = false;
                    MoveScene();
                });
            
            Debug.Log("Start LoginOAuth");
            StartLogin();
        }

        private void MoveScene()
        {
            _isSigned.Dispose();
            _isStart = false;
            Debug.Log("Login Success");
            SceneManager.LoadScene(1);
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
            Debug.Log("Start CheckLoginResult");
            if (result)
            {
                Debug.Log("[OAuth Login Success]");
                SetFirebaseCredential();
                OnFirebaseSignIn(CheckLoginSuccess);
            }
            else
            {
                Debug.Log("[Login Failed] : LoginOAuth result is false");
            }
        }

        private void CheckLoginSuccess(bool success)
        {
            Debug.Log("[LoginManager] Start CheckLoginSuccess");
            Debug.Log(success ? "[Login Success]" : "[Login Failed]");
            
            _isSigned.Value = success;
        }
    }
}