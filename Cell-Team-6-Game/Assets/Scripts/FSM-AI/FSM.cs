using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains all fms states to be implimented in any FSM of this project
/// </summary>
public enum FSMStateID
{
    none = 0,
    PlaceholderState,
}

public abstract class FSM : MonoBehaviour
{
    public Transform playerTransform;

    //Leaving following block empty for implimentation of navmesh once that is present
    //[Navmesh Implimentation Here]

    protected abstract void Initalize();
    protected abstract void FSMUpdate();
    protected abstract void FSMFixedUpdate();
    
    private FSMState currentState;
    public FSMState CurrentState { get { return currentState; } }

    private List<FSMState> FSMStates = new List<FSMState>();

    public void AddFSMState(FSMState state)
    {
        foreach (var item in FSMStates)
        {
            if(item.StateID == state.StateID)
            {
                Debug.LogError("Error: You can not add a state that is already present in this FSM");
                return;
            }
        }

        //First state added is treated as inital starting state for purposes of this FSM
        if(FSMStates.Count == 0)
        {
            FSMStates.Add(state);
            currentState = state;
        }
        else
        {
            FSMStates.Add(state);
        }
    }

    public void RemoveFSMState(FSMState state)
    {
        foreach (var item in FSMStates)
        {
            if(item.StateID == state.StateID)
            {
                FSMStates.Remove(item);
                return;
            }
        }
        Debug.LogError("Error: " + state.StateID.ToString() + " Is not present within FSMStates");
    }

    public void SetState(FSMState state)
    {
        foreach (var item in FSMStates)
        {
            if(item.StateID == state.StateID)
            {
                currentState = item;
                return;
            }
        }
        Debug.LogError("Error: " + state.StateID.ToString() + " Is not present within FSMStates");
    }

    // Start is called before the first frame update
    void Start()
    {
        Initalize();
    }

    // Update is called once per frame
    void Update()
    {
        FSMUpdate();
    }

    void FixedUpdate()
    {
        FSMFixedUpdate();
    }
}
