using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Players
{
    public static class GlobalData
    {
        public static GameObject[] playerList;

        public static void StartGame()
        {

            playerList = GameObject.FindGameObjectsWithTag("Player");
            AssignPlayers(playerList);
        }


        // This works for renaming players
        // I will probably need to assign player numbers to the player themselves too but not needed yet at least
        // Though, maybe I won't need it if I have the playerList list...
        public static void AssignPlayers(GameObject[] playerList)
        {
            int x = 1;
            foreach (GameObject player in playerList)
            {
                player.name = "Player " + x;
                x++;
            }

            Debug.Log("There is/are " + playerList.Length + " player(s)...");
        }

        public static void KickPlayer()
        {

        }
    }
}
