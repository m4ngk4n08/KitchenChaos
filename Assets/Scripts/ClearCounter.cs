using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter {

    [SerializeField] private KitchenObjectScriptableObject kitchenObjectScriptableObject;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // There is no kitchenOjbect here
            if (player.HasKitchenObject())
            {
                // Player is carrying something

                // player pass the item to the counter
                player.GetKitchenObject().SetKitchenObjectParent(this);
                
            }
            else
            {
                // Player not carrying anything
            }
        }
        else
        {
            // there is a kitchenObject
            if(player.HasKitchenObject())
            {
                // player is carrying something dont do anything
            }
            else
            {
                // give the object to player
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

}
