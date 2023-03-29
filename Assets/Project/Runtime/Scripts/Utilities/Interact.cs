using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Interact : MonoBehaviour
{

    public float raycastDistance = 10f;

    public TMP_Text interactText;

    void Update()
    {
        //Raycast waar muis richt
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //Draws ray
        Vector3 forward = transform.TransformDirection(Vector3.forward) * raycastDistance;
        Debug.DrawRay(transform.position, forward, Color.green);

        //checked of de ray object met tag Interact raakt
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.NameToLayer("Interact")))
        {
            interactText.text = "Press [E] to read the note.";
            Debug.Log("Hit");
        }
    }
}
