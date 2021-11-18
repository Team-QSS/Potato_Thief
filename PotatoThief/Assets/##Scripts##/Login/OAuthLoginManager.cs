﻿using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

namespace Login
{
    public class OAuthLoginManager
    {
        public string AuthCode { get; set; }

        /// <summary>
        /// nextAction : Authenticate 완료 시 실행할 함수. bool 매개변수는 Authenticate 작업의 성공 여부를 뜻함.
        /// </summary>
        /// <param name="callback"></param>
        public void OnOAuthAuthenticate(System.Action<bool> callback)
        {
            Debug.Log("Start GooglePlayLogin");
            var config =
                new PlayGamesClientConfiguration.Builder()
                    .RequestServerAuthCode(false /* Don't force refresh */)
                    .Build();

            PlayGamesPlatform.InitializeInstance(config);
            var x = PlayGamesPlatform.Activate();

            UnityEngine.Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    AuthCode =
                        PlayGamesPlatform.Instance.GetServerAuthCode();
                    Debug.Log("[Google Login Success]");
                }
                else
                {
                    AuthCode = "";
                    Debug.Log("[Google Login Failed] : success is false");
                }

                callback(success);
            });
        }
    }
}