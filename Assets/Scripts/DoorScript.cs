using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Animator doorAnim;
    public bool isOpen = false;

    void Start()
    {
        if (doorAnim == null)
        {
            doorAnim = GetComponent<Animator>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleDoor();
        }
    }

    void ToggleDoor()
    {
        isOpen = !isOpen;
        doorAnim.SetBool("isOpen", isOpen);
    }
}
