using PolyNav;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Animator DoorAnim;

    private PolyNavObstacle _navObstacle;
    private PolyNavObstacle navObstacle {
        get {
            if (!_navObstacle)
            {
                _navObstacle = GetComponent<PolyNavObstacle>();
            }

            return _navObstacle;
        }
    }
    
    protected virtual void Start()
    {
        DoorAnim = gameObject.GetComponent<Animator>();
    }

    public virtual void Open() {
        navObstacle.enabled = false;
        DoorAnim.SetBool("isopen", true);
    }

    public virtual void Close() {
        navObstacle.enabled = true;
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
