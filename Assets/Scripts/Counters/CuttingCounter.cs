using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public static event EventHandler OnAnyCut;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;


    [SerializeField] CuttingRecepieSO[] cuttingRecepieSOArray;

    private int cuttingProgress;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // There is no kitchenOjbect here
            if (player.HasKitchenObject())
            {
                // Player is carrying something
                // Player carrying something that can be cut
                if(HasRecepieWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // player pass the item to the counter
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    cuttingProgress = 0;

                    CuttingRecepieSO cuttingRecepieSO = GetCuttingRecepieSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs() { 
                        progressNormalized = (float)cuttingProgress / cuttingRecepieSO.CuttingProgressMax
                    });

                }

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
        if (HasKitchenObject() && HasRecepieWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            // There's a kitchenObject here and it can be cut

            cuttingProgress++;

            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            CuttingRecepieSO cuttingRecepieSO = GetCuttingRecepieSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecepieSO.CuttingProgressMax
            });

            if (cuttingProgress >= cuttingRecepieSO.CuttingProgressMax)
            {
                KitchenObjectScriptableObject outputKitchenObject = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

                GetKitchenObject().DestroySelf();

                KitchenObject.SpawnKitchenObject(outputKitchenObject, this);
            }

        }
    }

    private KitchenObjectScriptableObject GetOutputForInput(KitchenObjectScriptableObject inputKitchenObjectSO)
    {
        CuttingRecepieSO cuttingRecepieSO = GetCuttingRecepieSOWithInput(inputKitchenObjectSO);
        if (cuttingRecepieSO != null)
        {
            return cuttingRecepieSO.output;
        }

        return null;
    }

    private bool HasRecepieWithInput(KitchenObjectScriptableObject inputKitchenObjectSO)
    {
        CuttingRecepieSO cuttingRecepieSO = GetCuttingRecepieSOWithInput(inputKitchenObjectSO);

        return cuttingRecepieSO != null;
    }

    private CuttingRecepieSO GetCuttingRecepieSOWithInput(KitchenObjectScriptableObject inputKitchenObjectSO)
    {
        foreach (var cuttingRecepieSO in cuttingRecepieSOArray)
        {
            if (cuttingRecepieSO.input == inputKitchenObjectSO)
            {
                return cuttingRecepieSO;
            }
        }

        return null;
    }
}
