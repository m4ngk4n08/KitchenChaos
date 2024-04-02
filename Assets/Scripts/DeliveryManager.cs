using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeliveryManager : MonoBehaviour {

    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecepieListSO recipieListSO;


    private List<RecepieSO> waitingRecepieSOList;
    private float spawnRecepieTimer;
    private float spawnRecepieTimerMax = 4f;
    private int waitingRecipesMax = 4;
    public int successfulRecipesAmount;

    private void Awake()
    {
        Instance = this;
        waitingRecepieSOList = new List<RecepieSO>();
    }

    private void Update()
    {
        spawnRecepieTimer -= Time.deltaTime;

        if (spawnRecepieTimer <= 0f)
        {
            spawnRecepieTimer = spawnRecepieTimerMax;

            if (waitingRecepieSOList.Count < waitingRecipesMax)
            {
                RecepieSO waitingRecepieSO = recipieListSO.recepieSOList[UnityEngine.Random.Range(0, recipieListSO.recepieSOList.Count)];

                waitingRecepieSOList.Add(waitingRecepieSO);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecepie(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecepieSOList.Count; i++)
        {
            RecepieSO waitingRecepieSO = waitingRecepieSOList[i];

            if (waitingRecepieSO.recepieSOList.Count == plateKitchenObject.GetKitchenObjectScriptableObjectsList().Count)
            {
                // Has the same number of indredients
                bool plateContentsMatchesRecipe = true;

                foreach (KitchenObjectScriptableObject recipeKitchenObjectSO in waitingRecepieSO.recepieSOList)
                {
                    // Cycling through all ingredients in the Recepie
                    bool ingredientFound = false;
                    foreach (KitchenObjectScriptableObject plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectScriptableObjectsList())
                    {
                        // Cycling through all ingredients in the Plate
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            // Ingredients matches
                            ingredientFound = true;
                            break;
                        }
                    }

                    if (!ingredientFound)
                    {
                        // This Recipe ingredient was found on the Plate
                        plateContentsMatchesRecipe = false;
                    }
                }
                if (plateContentsMatchesRecipe)
                {
                    // Player delivered the correct recipe
                    waitingRecepieSOList.RemoveAt(i);
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);

                    successfulRecipesAmount ++;

                    return;
                }
            }
        }

        // No matches found.
        // Player did not deliver the recepie.
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);

    }

    public List<RecepieSO> GetWaitingRecipeSOList()
    {
        return waitingRecepieSOList;
    }

    public int GetSuccessfulRecipesAmount()
    {
        return successfulRecipesAmount;
    }
}
