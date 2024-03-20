using System;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress {

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }


    [SerializeField] FryingRecepieSO[] fryingRecepieSOArray;
    [SerializeField] BurningRecepieSO[] burningRecepieSOArray;

    private float fryingTimer;
    private float burningTimer;
    private FryingRecepieSO fryingRecepieSO;
    private BurningRecepieSO burningRecepieSO;
    private State state;

    public enum State {
        Idle,
        Frying,
        Fried,
        Burned
    }

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:

                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecepieSO.fryingTimerMax
                    });

                    if (fryingTimer > fryingRecepieSO.fryingTimerMax)
                    {
                        // Fried
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(fryingRecepieSO.output, this);

                        state = State.Fried;
                        burningTimer = 0f;
                        burningRecepieSO = GetBurningRecepieSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = burningTimer / burningRecepieSO.burningTimerMax
                    });

                    if (burningTimer > burningRecepieSO.burningTimerMax)
                    {
                        // Burned
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(burningRecepieSO.output, this);

                        state = State.Burned;

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }

                    break;
                case State.Burned:
                    
                    break;
            }

            Debug.Log(state);

        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // There is no kitchenOjbect here
            if (player.HasKitchenObject())
            {
                // Player is carrying something
                // Player carrying something that can be Fried
                if (HasRecepieWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // player pass the item to the counter
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    fryingRecepieSO = GetFryingRecepieSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    state = State.Frying;
                    fryingTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecepieSO.fryingTimerMax
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
                // player is carrying something dont do anything
            }
            else
            {
                // give the object to player
                GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                }); 
                
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }
    private KitchenObjectScriptableObject GetOutputForInput(KitchenObjectScriptableObject inputKitchenObjectSO)
    {
        FryingRecepieSO fryingRecepieSO = GetFryingRecepieSOWithInput(inputKitchenObjectSO);
        if (fryingRecepieSO != null)
        {
            return fryingRecepieSO.output;
        }

        return null;
    }

    private bool HasRecepieWithInput(KitchenObjectScriptableObject inputKitchenObjectSO)
    {
        FryingRecepieSO fryingRecepieSO = GetFryingRecepieSOWithInput(inputKitchenObjectSO);

        return fryingRecepieSO != null;
    }

    private FryingRecepieSO GetFryingRecepieSOWithInput(KitchenObjectScriptableObject inputKitchenObjectSO)
    {
        foreach (var fryingRecepieSO in fryingRecepieSOArray)
        {
            if (fryingRecepieSO.input == inputKitchenObjectSO)
            {
                return fryingRecepieSO;
            }
        }

        return null;
    }
    private BurningRecepieSO GetBurningRecepieSOWithInput(KitchenObjectScriptableObject inputKitchenObjectSO)
    {
        foreach (var burningRecepieSO in burningRecepieSOArray)
        {
            if (burningRecepieSO.input == inputKitchenObjectSO)
            {
                return burningRecepieSO;
            }
        }

        return null;
    }
}
