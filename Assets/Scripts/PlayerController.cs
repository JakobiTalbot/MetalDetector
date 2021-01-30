using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum Tool
{
    Detector,
    Shovel
}

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 10.0f;
    public float rotateSpeed = 10.0f;

    public Tool CurrentTool { get; private set; } = Tool.Detector;
    public Action<Tool> OnToolChange;

    [SerializeField]
    GameObject metalDetector;
    [SerializeField]
    GameObject shovel;

    Camera cam;
    CharacterController characterController;
    FootstepCreator testFootstepCreator;
    float lastFootstepTime = 0.0f;

    Quaternion targetRotation;

    private void Awake()
    {
        cam = Camera.main;
        characterController = GetComponent<CharacterController>();
        testFootstepCreator = GetComponent<FootstepCreator>();

        targetRotation = transform.rotation;
    }

    private void Start()
    {
        SetTool(Tool.Detector);
    }

    private void Update()
    {
        DoMovement();
        DoLook();

        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetMouseButtonDown(1))
            ToggleEquipment();
    }

    void ToggleEquipment()
    {
        Tool newTool = CurrentTool+1;
        if ((int)newTool >= Enum.GetValues(typeof(Tool)).Length)
            newTool = (Tool)0;

        SetTool(newTool);
    }

    void SetTool(Tool tool)
    {
        CurrentTool = tool;

        metalDetector.SetActive(CurrentTool == Tool.Detector);
        shovel.SetActive(CurrentTool == Tool.Shovel);

        OnToolChange?.Invoke(CurrentTool);
    }

    void DoMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveVector = (horizontal * cam.transform.right) + (vertical * cam.transform.forward);
        if(moveVector.sqrMagnitude > 1.0f)
        {
            moveVector.Normalize();
        }

        if(moveVector.sqrMagnitude > float.Epsilon)
        {
            if(Time.time - lastFootstepTime > 0.2f)
            {
                testFootstepCreator?.CreateFootstep();
                lastFootstepTime = Time.time;
            }
        }

        characterController.SimpleMove(moveVector * moveSpeed);
    }

    void DoLook()
    {
        Ray mouseRay = cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(mouseRay, out var hit, float.PositiveInfinity, LayerMask.GetMask("Terrain")))
        {
            Vector3 posDif = hit.point - transform.position;
            float lookAngle = Mathf.Rad2Deg * Mathf.Atan2(posDif.x, posDif.z);
            Vector3 oldAngle = transform.rotation.eulerAngles;
            oldAngle.y = lookAngle;
            targetRotation = Quaternion.Euler(oldAngle);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
    }

}
