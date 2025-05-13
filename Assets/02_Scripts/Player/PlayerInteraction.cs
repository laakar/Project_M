using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private RenderTexture renderTexture;
    private void Update()
    {
        if(!GameManager.instance.canPlay){return;}
        if (Input.GetMouseButtonDown(0))
        {
            SelectObject();
        }
    }
    void SelectObject()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 adjustedPosition = new Vector2(
            mousePosition.x * (renderTexture.width / (float)Screen.width),
            mousePosition.y * (renderTexture.height / (float)Screen.height)
        );
        
        Ray ray = camera.ScreenPointToRay(adjustedPosition );
        if (Physics.Raycast(ray.origin, ray.direction * 10f, out RaycastHit hit))
        {
            IInteractauble interactauble = hit.collider.GetComponent<IInteractauble>();
            if (interactauble != null)
            {
                interactauble.SelectCard();
            }
        }
    }
}
