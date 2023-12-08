using System.Collections;
using System.Collections.Generic;
using Players;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponCore : NetworkBehaviour
{
    [SerializeField] public bool isTaken;
    [SerializeField] private Collider2D[] allColliders;

    public WeaponFollowPlayer weaponFollowPlayer;

    private void Awake()
    {
        weaponFollowPlayer = GetComponent<WeaponFollowPlayer>();
    }

    public override void OnNetworkSpawn()
    {
        allColliders = transform.GetComponents<BoxCollider2D>();
        isTaken = false;
    }
    
    // Update is called once per frame
    private void Update()
    {
        if (isTaken)
        {
            foreach (Collider2D collider in allColliders)
            {
                collider.enabled = false;
            }
            GetComponent<Rigidbody2D>().gravityScale = 0f;

            // WeaponCollectedServerRpc();
        } else if (!isTaken)
        {
        }
    }
    /*
    [ServerRpc(RequireOwnership = false)]
    private void WeaponCollectedServerRpc()
    {
        // A try/catch is used because of how unity updates real time objects
        // Because we have a script elsewhere that turns of 'isTaken' while this is updating,
        // unity becomes unable to locate the parent object's position
        try { weaponFollowPlayer.SetTargetTransform(PlayerWeaponPoint()); }
        catch
        {
            // Resets the object's variables and physics if the above fails
            isTaken = false;
            foreach (Collider2D collider in allColliders)
            {
                collider.enabled = true;
            }
            GetComponent<Rigidbody2D>().gravityScale = 1f;
        }
    }*/
}
