using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject 
{

    public static event EventHandler OnAnyObjectMovedToPlate;
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectScriptableObject kitchenObjectSO;
    }

    private List<KitchenObjectScriptableObject> kitchenObjectList;
    [SerializeField] private List<KitchenObjectScriptableObject> validKitchenObjectSOList;

    private void Start()
    {
        kitchenObjectList = new List<KitchenObjectScriptableObject>();
    }

    public bool TryAddIngredient(KitchenObjectScriptableObject kitchenObjectScriptableObject)
    {
        if     (kitchenObjectList.Contains(kitchenObjectScriptableObject) 
            || !validKitchenObjectSOList.Contains(kitchenObjectScriptableObject))
        {
            // Aleardy has this type
            return false;
        }

        kitchenObjectList.Add(kitchenObjectScriptableObject);

        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
        {
            kitchenObjectSO = kitchenObjectScriptableObject
        });

        OnAnyObjectMovedToPlate.Invoke(this, EventArgs.Empty);

        return true;
    }

    public List<KitchenObjectScriptableObject> GetKitchenObjectScriptableObjectsList()
    {
        return kitchenObjectList;
    }
}
