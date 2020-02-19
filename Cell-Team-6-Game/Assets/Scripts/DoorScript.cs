using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private Animator DoorAnim;
    
    private void Start()
    {
        DoorAnim = gameObject.GetComponent<Animator>();
    }

    public virtual void Open() {
        DoorAnim.SetBool("isopen", true);
    }

    public virtual void Close() {
        DoorAnim.SetBool("isopen", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
            Open();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            Close();
    }
}
