using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }
    public void SetRecipeSO(RecepieSO recepieSO)
    {
        recipeNameText.text = recepieSO.recepieName;

        foreach (Transform child in iconContainer)
        {
            if(child == iconTemplate) continue;
            Destroy(child);
        }

        foreach (KitchenObjectScriptableObject kitchenObjectSO in recepieSO.KitchenObjectSOList())
        {
            Transform iconTransform = Instantiate(iconTemplate, iconContainer);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }
    }
}
