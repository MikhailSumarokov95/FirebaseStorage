using UnityEngine;
using Firebase.Database;
using System;

public class StorageFirebase
{
    public Action onUploadedTopScore;
    public Action<int> onDownloadedTopScore;
    private string _playerId;
    private int _topScoreWaitAuthPlayerForUpload = -2;
    private bool _topScoreWaitAuthPlayerForDownload;
    private AuthAnonimusFirebase _authAnonimusFirebase;
    readonly DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;


    public StorageFirebase()
    {
        _authAnonimusFirebase = new AuthAnonimusFirebase();
        _authAnonimusFirebase.onAuthenticated += SetAuthPlayer;
        _authAnonimusFirebase.AuthenticationPlayer();
    }

    public void UploadTopScore(int value)
    {
        if (_playerId == null)
        {
            _topScoreWaitAuthPlayerForUpload = value;
            return;
        }
        reference.Child("Players").Child(_playerId).SetValueAsync(value).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("error UploadTopScore");
            }
            onUploadedTopScore?.Invoke();
        });
    }

    public void DownloadTopScore()
    {
        if (_playerId == null)
        {
            _topScoreWaitAuthPlayerForDownload = true;
            return;
        }
        reference.Child("Players").Child(_playerId).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("error DownloadTopScore");
            }
            DataSnapshot data = task.Result;
            int topScore = Convert.ToInt32(data.Value);
            Debug.Log("DownloadTopScore" + topScore);
            onDownloadedTopScore?.Invoke(topScore);
        });
    }

    private void SetAuthPlayer()
    {
        _playerId = _authAnonimusFirebase.PlayerId;
        if (_topScoreWaitAuthPlayerForUpload > -1)
        {
            UploadTopScore(_topScoreWaitAuthPlayerForUpload);
            _topScoreWaitAuthPlayerForUpload = -2;
        }
        if (_topScoreWaitAuthPlayerForDownload)
        {
            DownloadTopScore();
            _topScoreWaitAuthPlayerForDownload = false;
        }
    }
}