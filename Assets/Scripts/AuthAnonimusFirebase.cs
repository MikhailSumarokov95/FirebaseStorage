using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using System;

public class AuthAnonimusFirebase
{
    public string PlayerId { get; private set; }
    public Action onAuthenticated;
    private FirebaseAuth auth = FirebaseAuth.DefaultInstance;


    public void AuthenticationPlayer()
    {
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }
            FirebaseUser newUser = task.Result;
            PlayerId = newUser.UserId;
            onAuthenticated?.Invoke();
        });
    }
}
