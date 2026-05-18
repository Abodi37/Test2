using UnityEngine;

public class OppositeDoor : MonoBehaviour
{
  public Animator doorAnim;
    public bool isOpen = true;

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
