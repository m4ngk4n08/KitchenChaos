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
                // player is carrying something 
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Player is holding a plate {refactored to the method TryGetPlate}
                    //PlateKitchenObject plateKitchenObject = player.GetKitchenObject() as PlateKitchenObject;

                    // Pass the kitchenObject to plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        // Destroy kitchenObject on the counter
                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    // Player is not carrying Plate but something else
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        // Counter is holding a Plate
                        // try give the object to the plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            // then destroy the players object.
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }

            }
            else
            {
                // give the object to player
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

}
