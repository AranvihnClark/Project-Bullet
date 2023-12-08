using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Players
{
    public class PlayerController : NetworkBehaviour
    {
        // Testing
        [SerializeField] List<GameObject> networkPrefabWeapons;
        [SerializeField] Rigidbody2D rb;
        [SerializeField] float movementSpeed = 15f;
        [SerializeField] float jumpVelocityFactor = 28f;
        [SerializeField] LayerMask jumpableGround;
        // [SerializeField] private GameObject gun;
        // [SerializeField] public GameObject sword;


        // To determine which weapon above is being held at the moment (for tossing and attacking)
        // 0 = gun, 1 = sword
        [SerializeField] private static int activeWeapon = 2;
            
        // Misc. variables
        private int doubleJump = 0;

        // Child Objects
        public GameObject gun;
        public GameObject sword;

        public override void OnNetworkSpawn()
        {/*
            activeWeapon.OnValueChanged += (int old, int next) =>
            {
                Debug.Log("Client " + OwnerClientId + ": Weapon Changed!");
            };*/
        }

        private void Update()
        {
            if (!IsOwner) return;

            // Checks in case if a sword or gun was just added to not show both icons right away.
            
            if (activeWeapon > 1)
            {
                try 
                {
                    string tagName = transform.GetChild(0).tag;
                    if (tagName.Equals("Gun")) 
                    {
                        activeWeapon = 0;
                    }
                    else activeWeapon = 1;
                } catch {}
            } 

            // Toss a weapon
            if (Input.GetKeyDown(KeyCode.T)) 
            {
                TestSpawnGunServerRpc();
            }

            // Switch weapons
            if (Input.GetKeyDown(KeyCode.R)) 
            {
                SwitchWeaponsServerRpc(activeWeapon, new ServerRpcParams());
            }

            // Test Key
            if (Input.GetKeyDown(KeyCode.Return))
            {
                GlobalData.KickPlayer();
            }

            // Movement
            MovePlayer();

            // Checks if the player is on the ground.
            if (IsPlayerOneGround())
            {
                doubleJump = 0;
            }
        }

        private void MovePlayer()
        {
            Vector2 movement = new Vector2(0, 0);
            if (Input.GetAxisRaw("Horizontal") > 0) movement.x = 1f;
            if (Input.GetAxisRaw("Horizontal") < 0) movement.x = -1f;

            rb.velocity = new Vector2(movement.x * movementSpeed, rb.velocity.y);

            if (Input.GetButtonDown("Jump") && doubleJump < 1) 
            {
                doubleJump++;
                rb.velocity = new Vector2(rb.velocity.x, jumpVelocityFactor);
            }
        }

        // Currently only works on the server to actually change a weapon
        [ServerRpc]
        private void SwitchWeaponsServerRpc(int currentWeapon, ServerRpcParams param)
        {
            if (transform.childCount > 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).CompareTag("Gun"))
                    {
                        gun = transform.GetChild(i).gameObject;
                    } else if (transform.GetChild(i).CompareTag("Sword"))
                    {
                        sword = transform.GetChild(i).gameObject;
                    }
                }
            }
            // Equipping guns or swords
            if (currentWeapon == 1 && param.Receive.SenderClientId == OwnerClientId && gun != null) 
            {
                sword.SetActive(false);
                gun.SetActive(true);
                activeWeapon = 0;
                Debug.Log("Client " + OwnerClientId + ": Weapon changed to a gun!");
            }
            else if (currentWeapon == 0 && param.Receive.SenderClientId == OwnerClientId && sword != null)
            {
                gun.SetActive(false);
                sword.SetActive(true);
                activeWeapon = 1;
                Debug.Log("Client " + OwnerClientId + ": Weapon changed to a sword!");
            }
        }
    /*
        [ServerRpc]
        private void TossItemServerRpc(GameObject weapon)
        {
            GameObject tempWeapon;
            tempWeapon = Instantiate(weapon);
            tempWeapon.GetComponent<NetworkObject>().Spawn(true);
            tempWeapon.transform.position = gun.transform.position;
        }*/

        private bool IsPlayerOneGround()
        {
            // This creates a cast that covers what our player's box collider is.
            // It also moves the cast down just a smidge lower than our current box collider.
            // This allows us to check if the cast overlaps another box collider below our player only.
            
            return Physics2D.Raycast(transform.position, Vector2.down, 0.96f, jumpableGround);
        }
        
        // This currently works in that both server and client can spawn a gun!!!
        [ServerRpc]
        private void TestSpawnGunServerRpc()
        {
            foreach (GameObject weaponObject in networkPrefabWeapons)
            {
                Transform tempWeapon;
                tempWeapon = Instantiate(weaponObject.transform);
                tempWeapon.GetComponent<NetworkObject>().Spawn(true);
            }
        }
    }
}
