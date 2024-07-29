using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{

    LobbyCharacter _lobbyCharacter;
    LobbyMonster _lobbyMonster;
    GPGSManager _gpgsManager;
    LobbyUIManager _lobbyUIManager;

    public bool _tutorial;

    // Start is called before the first frame update
    void Start()
    {
        _lobbyCharacter = FindObjectOfType<LobbyCharacter>();
        _lobbyMonster = FindObjectOfType<LobbyMonster>();
        _lobbyUIManager = FindObjectOfType<LobbyUIManager>();

        if (!_tutorial)
        {
            _lobbyCharacter.Attack();
        }
    }

    private void Update()
    {
        if (_gpgsManager == null && FindObjectOfType<GPGSManager>())
        {
            _gpgsManager = FindObjectOfType<GPGSManager>();
            _gpgsManager.SetValue(_lobbyUIManager._CoinText, _lobbyUIManager._CashText);
        }
    }

    public void SetCoin(string _coin)
    {
        _lobbyUIManager.SetCoinText(_coin);
    }

    public void SetCash(string _cash)
    {
        _lobbyUIManager.SetCashText(_cash);
    }

    public void CharacterAttack()
    {
        _lobbyCharacter.Attack();
    }

    public void MonsterHit()
    {
        _lobbyMonster.Hit();
    }

    public void ChangeData()
    {
        _gpgsManager.ChangeGoldCrystal(100, 100, _lobbyUIManager._CoinText, _lobbyUIManager._CashText);
    }

    public void EnterDungeon()
    {
        Transitioner.Instance.TransitionToScene(3);
    }
}
