using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCharacter : MonoBehaviour
{
    Animator _animator;
    LobbyManager _lobbyManager;

    public Animator Animator { get => _animator; set => _animator = value; }

    private void Awake()
    {
        _animator = transform.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _lobbyManager = FindObjectOfType<LobbyManager>();
    }

    public void MobHit()
    {
        _lobbyManager.MonsterHit();
    }

    public void Attack()
    {
        _animator.SetTrigger("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
