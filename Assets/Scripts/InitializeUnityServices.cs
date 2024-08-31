using System;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

public class InitializeUnityServices : MonoBehaviour
{
    const string k_Environment = "production";

    void Awake()
    {
        // Uncomment this line to initialize Unity Gaming Services.
        Initialize(OnSuccess, OnError);
    }

    void Initialize(Action onSuccess, Action<string> onError)
    {
        try
        {
            var options = new InitializationOptions().SetEnvironmentName(k_Environment);

            UnityServices.InitializeAsync(options).ContinueWith(task => onSuccess());
        }
        catch (Exception exception)
        {
            onError(exception.Message);
        }
    }

    void OnSuccess()
    {
        Debug.Log("Success");
    }

    void OnError(string message)
    {
        Debug.LogError("Error");
    }

    void Start()
    {
        if (UnityServices.State == ServicesInitializationState.Uninitialized)
        {

        }
    }
}
