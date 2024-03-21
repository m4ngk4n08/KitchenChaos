using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatesIconSingleUI : MonoBehaviour 
{
    [SerializeField] private Image image;

    public void SetKitchenObjectSO(KitchenObjectScriptableObject kitchenObjectScriptable)
    {
        image.sprite = kitchenObjectScriptable.sprite;
    }
}
