using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] private KitchenObjectScriptableObject kitchenObjectScriptableObject;

    public override void Interact(Player player)
    {
        #region refactored
        // This code will set a object on top of the prefab when kitchenObject is null else give it to player
        //if (!HasKitchenObject())
        //{
        //    Transform kitchenObjectTransform = Instantiate(kitchenObjectScriptableObject.prefab);
        //    kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
        //}
        //else
        //{
        //    // Give object to player
        //    kitchenObject.SetKitchenObjectParent(player);

        //}
        #endregion

        Transform kitchenObjectTransform = Instantiate(kitchenObjectScriptableObject.prefab);
         kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);

        OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
    }

}
