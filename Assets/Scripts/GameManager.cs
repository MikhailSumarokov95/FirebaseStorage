using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Text _textTopScore;
    private int _score = 300;
    private StorageFirebase _storageFirebase;

    private void Start()
    {
        _storageFirebase = new StorageFirebase();
        _storageFirebase.UploadTopScore(_score);
        _storageFirebase.onDownloadedTopScore += SetTopScoreText;
    }

    public void UpScore()
    {
        _score += 1;
        _storageFirebase.UploadTopScore(_score);
    }

    public void DownloadTopScore()
    {
        _storageFirebase.DownloadTopScore();
    }

    public void SetTopScoreText(int topScore)
    {
        _textTopScore.text = topScore.ToString();
        Debug.Log(_textTopScore.text);
        Debug.Log("SetTopScoreText: " + topScore);
    }
}
