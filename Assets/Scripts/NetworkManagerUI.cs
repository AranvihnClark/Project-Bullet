using System.Collections;
using System.Collections.Generic;
using Players;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;
    [SerializeField] private Button stopBtn;

    private void Awake()
    {
        // When a button is clicked, the appropriate action is performed by the Network Manager
        // Host Button
        hostBtn.onClick.AddListener(() => 
        {
            Debug.Log("Started hosting...");
            NetworkManager.Singleton.StartHost();
        });

        // Client Button
        clientBtn.onClick.AddListener(() => 
        {
            Debug.Log("Joining host...");
            NetworkManager.Singleton.StartClient();
        });

        // Stop hosting button
        stopBtn.onClick.AddListener(() =>
        {
            Debug.Log("Shutting down host...");
            NetworkManager.Singleton.Shutdown();
        });
    }
}
