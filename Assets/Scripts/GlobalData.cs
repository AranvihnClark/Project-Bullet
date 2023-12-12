using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Players
{
    public class GlobalData : NetworkBehaviour
    {
        private GameObject[] playerObjectArray;
        public static NetworkObjectReference[] playerList;

        public void StartGame()
        {
            // playerObjectArray = GameObject.FindGameObjectsWithTag("Player");
            
            // for (int i = 0; i < playerObjectArray.Length; i++)
            // {
            //     // playerList[i] = playerObjectArray[i].GetComponent<NetworkObject>();
            // }
            AssignPlayersServerRpc(playerList);
        }

        // This works for renaming players
        // I will probably need to assign player numbers to the player themselves too but not needed yet at least
        // Though, maybe I won't need it if I have the playerList list...
        [ServerRpc]
        public void AssignPlayersServerRpc(NetworkObjectReference[] playerList)
        {
            if (playerList.Length != 0)
            {
                AssignPlayersClientRpc(playerList);
            }
        }

        [ClientRpc]
        private void AssignPlayersClientRpc(NetworkObjectReference[] playerList)
        {
            int x = 1;
            foreach (GameObject player in playerList)
            {
                player.name = "Player " + x;
                x++;
            }
            Debug.Log("There is/are " + playerList.Length + " player(s)...");
        }
    }
}
