using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnermyCoinText : MonoBehaviour
{
    [SerializeField] private Text _coin;

    // Start is called before the first frame update
    void Start()
    {
        transform.DOMove(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), 1f);
        Destroy(this.gameObject, 2f);
    }

    public void SetCoin(float _gold)
    {
        _coin.text = "+" + _gold;
    }
}
