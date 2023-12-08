using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

namespace Players
{
    public class ItemCollector : NetworkBehaviour
    {
        [SerializeField] private float selfTimer;
        [SerializeField] private bool timerOn = false;
        [SerializeField] private bool canCollectWeapon;
        private Transform weaponCollected;

        public override void OnNetworkSpawn()
        {
            canCollectWeapon = false;
            selfTimer = 1;
            timerOn = true;
        }
        // Update is called once per frame
        private void Update()
        {
            if (!IsOwner) return;
            if (timerOn)
            {
                // If there is still time left, we want to reduce that time per frame per second.
                if (selfTimer > 0)
                {
                    selfTimer -= Time.deltaTime;
                }
                else 
                {
                    canCollectWeapon = true;
                    timerOn = false;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!IsOwner) return;

            MessageServerRpc("" + OwnerClientId);

            if (!collider.tag.Equals("Boundary"))
            {
                if (canCollectWeapon && !collider.GetComponent<WeaponCore>().isTaken)
                {
                    if (collider.CompareTag("Sword") || collider.CompareTag("Gun"))
                    {
                        // This is 'equipping' the weapon
                        weaponCollected = collider.transform;
                        CollectWeaponServerRpc(weaponCollected.GetComponent<NetworkObject>(), new ServerRpcParams());
                    }
                }
            
                // Reset components
                collider.GetComponent<WeaponCore>().isTaken = true;
                canCollectWeapon = false;
                selfTimer = 1;
                MessageServerRpc("SelfTimer Reset");
                timerOn = true;
            }
        }

        // Need to figure out how to let client collect weapons.
        [ServerRpc(RequireOwnership = false)]
        private void CollectWeaponServerRpc(NetworkObjectReference weaponObjectReference, ServerRpcParams serverRpcParams)
        {
            // We'll create a temp variable to hold the tag name so that we can verify the weapon against our old weapons
            // string tagName = "";
            // if (serverRpcParams.Receive.SenderClientId.Equals(OwnerClientId))
            // {
            //     tagName = weaponCollected.tag;
            // }
            // MessageServerRpc("Picked up a " + weaponCollected.tag + "!");

            // We then need to get the Network Object based off of our reference
            weaponObjectReference.TryGet(out NetworkObject weaponObject);
            
            // First we check if this is the player's first weapon
            if (transform.childCount == 1)
            {
                CollectWeaponClientRpc(weaponObject);
            }

            if (transform.childCount > 1)
            {

            }
        } 

        [ClientRpc]
        private void CollectWeaponClientRpc(NetworkObjectReference weaponObjectReference)
        {
            weaponObjectReference.TryGet(out NetworkObject weaponObject);
            weaponObject.GetComponent<WeaponCore>().weaponFollowPlayer.SetTargetTransform(transform.GetChild(0));
        }

        [ServerRpc]
        private void MessageServerRpc(FixedString128Bytes message)
        {
            Debug.Log("Client ID: " + OwnerClientId + " --> " + message);
        }
    }
}
