using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    public Animator doorAnim;

    public void OpenDoor()
    {
        doorAnim.SetBool("IsOpen", true);
    }
}
