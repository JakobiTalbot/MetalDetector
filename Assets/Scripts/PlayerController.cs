using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 10.0f;

    Camera cam;
    CharacterController characterController;

    private void Awake()
    {
        cam = Camera.main;
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveVector = (horizontal * cam.transform.right) + (vertical * cam.transform.forward);
        if(moveVector.sqrMagnitude > 1.0f)
        {
            moveVector.Normalize();
        }

        characterController.SimpleMove(moveVector * moveSpeed);
    }

}
