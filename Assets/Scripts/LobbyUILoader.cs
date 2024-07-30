using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LobbyUILoader : MonoBehaviour
{
    public Image _img;
    public string url;

    private void Start()
    {
        StartCoroutine(Loader());
    }

    public IEnumerator Loader()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D _texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            _img.sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(0f, 0f));
            this.gameObject.SetActive(false);
        }
    }
}
