using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectScriptableObject kitchenObjectScriptableObject;

    private ClearCounter clearCounter;

    public KitchenObjectScriptableObject KitchenObjectScriptableObject()
    {
        return kitchenObjectScriptableObject;
    }

    public void SetClearCounter(ClearCounter clearCounter)
    {
        // clear the old object on last clearCounter
        if (this.clearCounter != null)
        {
            this.clearCounter.ClearKitchenObject();
        }

        if (clearCounter.HasKitchenObject())
        {
            Debug.LogError("Counter already has a KitchenObject!");
        }

        // set the an object to new clearCounter
        this.clearCounter = clearCounter;
        clearCounter.SetKitchenObject(this);

        // update the visual
        transform.parent = clearCounter.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public ClearCounter GetClearCounter()
    {
        return clearCounter;
    }
}
