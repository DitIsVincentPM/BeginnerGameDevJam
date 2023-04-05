using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ControlButton : MonoBehaviour
{
    public InputActionReference actionToRebind;
    public TMP_Text buttonText;
    
    public void RemapButtonClicked()
    {
        // Start the interactive rebinding operation
        var rebindOperation = actionToRebind.action.PerformInteractiveRebinding().Start();

        // Update the text of the button to indicate that the remapping process has started
        buttonText.text = "Press a key or button...";

        // Listen for the user to press a new key or button
        rebindOperation.OnMatchWaitForAnother(0.1f)
            .OnComplete(operation =>
            {
                // Get the new binding and update the text of the button
                InputBinding newBinding = operation.action.bindings[0];
                buttonText.text = newBinding.ToDisplayString();

                // Update the InputActionReference with the new binding
                actionToRebind.action.ApplyBindingOverride(0, newBinding);
            });
    }
}
