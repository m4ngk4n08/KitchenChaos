using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] CuttingRecepieSO[] cuttingRecepieSOArray;

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
            if (player.HasKitchenObject())
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

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject())
        {
            // There's a kitchenObject here

            KitchenObjectScriptableObject outputKitchenObject = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

            GetKitchenObject().DestroySelf();

            KitchenObject.SpawnKitchenObject(outputKitchenObject, this);

        }
    }

    private KitchenObjectScriptableObject GetOutputForInput(KitchenObjectScriptableObject inputKitchenObjectSO)
    {
        foreach (var cuttingRecepieSO in cuttingRecepieSOArray)
        {
            if (cuttingRecepieSO.input.Equals(inputKitchenObjectSO))
            {
                return cuttingRecepieSO.output;
            }
        }

        return null;
    }
}
