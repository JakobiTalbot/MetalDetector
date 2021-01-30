using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    void ToggleAnimationOff()
    {
        GetComponent<Animator>().SetBool("Popup", false);
    }
}
