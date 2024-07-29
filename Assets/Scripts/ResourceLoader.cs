using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ResourceLoader : MonoBehaviour
{
    RawImage _img;
    public string[] url;

    private void Start()
    {
        Transitioner.Instance._automaticallyTransitionIn = false;
    }

    private void OnEnable()
    {
        StartCoroutine(Loader());
    }

    public IEnumerator Loader()
    {
        _img = FindObjectOfType<RawImage>();

        string _url;

        int _urlNumber = Random.Range(0, url.Length);

        _url = url[_urlNumber];

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(_url);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            _img.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Transitioner.Instance.FinishTransition();
            Transitioner.Instance._automaticallyTransitionIn = true;
        }
    }
}
