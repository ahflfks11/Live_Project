using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorGame : MonoBehaviour
{
    public void endGame()
    {
        BackEnd.Backend.BMember.Logout();
        Transitioner.Instance.TransitionToScene(0);
    }
}
