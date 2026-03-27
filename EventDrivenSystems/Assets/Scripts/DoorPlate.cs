using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum PlateState { Active, Inactive, Prepared}

public class doorPlate : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent<PlateState> DoorToggle;

    public PlateState plateState = PlateState.Inactive;
    void Awake()
    {

        if (DoorToggle == null)
        {
            DoorToggle = new UnityEvent<PlateState>();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //the first time one of the door plates is triggered,
        //switches to the prepared state, then the second time,
        //switches to the active state, which opens the door
        if (plateState != PlateState.Prepared)
        {
            plateState = PlateState.Prepared;
        }
        else if (plateState  == PlateState.Prepared)
        {
            plateState = PlateState.Active;
        }
        

        DoorToggle.Invoke(plateState);
        Debug.Log(plateState);

        
        
    }
}
