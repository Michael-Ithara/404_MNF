using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // Import the Input System namespace

public class HoldToLoadLevel : MonoBehaviour
{
    public float holdDuration = 1f;
    public Image fillCircle;
    private float holdTimer = 0f;

    public GameController gameController; // Reference to the GameController to call LoadNextLevel
    private bool isHolding = false;


    public void Update()
    {
        if (isHolding)
        {
            holdTimer += Time.deltaTime; // Increment the hold timer
            fillCircle.fillAmount = holdTimer / holdDuration; // Update the fill amount of the circle
            if (holdTimer >= holdDuration)
            {
                ResetHold(); // Reset hold after loading
                gameController.LoadNextLevel(); // Call the method to load the next level
            }
        }
    }

    public void OnHold(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isHolding = true; // Set holding state to true when the action is performed
            holdTimer = 0f; // Reset the hold timer
        }
        else if (context.canceled)
        {
            ResetHold(); // Reset hold when the action is canceled
        }
    }
    public void ResetHold()
    {
        isHolding = false;
        holdTimer = 0f;
        fillCircle.fillAmount = 0f;

    }



    public void ShowLoading(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            ResetHold(); // Reset hold if the action is not performed
            isHolding = false; // Reset holding state
        }
        else
        {
            isHolding = true; // Set holding state to true when the action is performed
            holdTimer += Time.deltaTime;
            if (holdTimer >= holdDuration)
            {
                ResetHold(); // Reset hold after loading
                gameController.LoadNextLevel(); // Call the method to load the next level
            }
        }
    }


}