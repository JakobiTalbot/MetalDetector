using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbleBody : MonoBehaviour
{

    public float bobbleAmount = 10.0f;

    public float bounciness = 1.0f;
    public float bouncySpeed = 1.0f;

    Vector3 moveVec;
    Vector3 rotSpeed;
    Quaternion rotQuat;

    private void Update()
    {
        Vector3 destRot = transform.rotation * (new Vector3(moveVec.z, 0.0f, moveVec.x) * bobbleAmount);
        destRot.z *= -1.0f;

        Quaternion destQuaternion = Quaternion.Euler(destRot);

        Quaternion difQuaternion = destQuaternion * Quaternion.Inverse(transform.localRotation);
        Quaternion bPart = Quaternion.Lerp(Quaternion.identity, difQuaternion, bouncySpeed);
        rotQuat = Quaternion.SlerpUnclamped(rotQuat, bPart, bounciness * Time.deltaTime);

        transform.localRotation *= rotQuat;
    }

    public void SetMoveVector(Vector3 newVec)
    {
        moveVec = newVec;
    }

}
