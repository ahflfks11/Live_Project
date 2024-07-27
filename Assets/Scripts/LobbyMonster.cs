using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LobbyMonster : MonoBehaviour
{

    Animator _animator;
    LobbyManager _lobbyManager;
    public DOTweenAnimation _doFadetAni;
    public DOTweenAnimation _doMoveAni;
    SpriteRenderer _myspr;
    Color _myColor;
    Transform _myPos;
    public Animator Animator { get => _animator; set => _animator = value; }

    private void Awake()
    {
        _animator = transform.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _lobbyManager = FindObjectOfType<LobbyManager>();
        _myspr = transform.GetComponent<SpriteRenderer>();
        _myColor = _myspr.color;
        _myPos = transform;
    }

    public void CharacterAttack()
    {
        _myspr.color = _myColor;
        _animator.Play("Idle");
        _lobbyManager.CharacterAttack();
    }

    public void Hit()
    {
        _doFadetAni.DORestart();
        _doMoveAni.DORestart();
        _animator.SetTrigger("Hit");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
