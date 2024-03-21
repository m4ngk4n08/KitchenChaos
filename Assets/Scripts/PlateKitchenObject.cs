using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject {


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
        return true;
    }
}
