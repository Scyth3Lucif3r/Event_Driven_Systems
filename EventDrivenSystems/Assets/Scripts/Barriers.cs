using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barriers : MonoBehaviour
{
    public Vector3 upTarget = new Vector3(0, -1, 0);
    public Vector3 downTarget = new Vector3(0, -1, 0);
    public float speed = 1.0f;

    bool moveDownCommand = false;
    bool moveUpCommand = false;

    Vector3 initialPosition;
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (moveUpCommand)
        {
            Debug.Log("Up");
            bool inPosition = MoveToPosition(upTarget);
            if (inPosition)
            {
                moveUpCommand = false;
            }
        }
        else if (moveDownCommand)
        {
            Debug.Log("Down");
            bool inPosition = MoveToPosition(downTarget);
            if (inPosition)
            {
                moveUpCommand = false;
            }
        }
    }

    bool MoveToPosition(Vector3 target)
    {
        // the target is effectively in local space
        // add the barrier's initial position to get it in global space
        Vector3 globalTarget = target + initialPosition;
        Debug.Log("moving");
        transform.position = Vector3.MoveTowards(
            transform.position,
            globalTarget,
            speed * Time.deltaTime
            );

        return Vector3.Distance(transform.position, target) < 0.01f;
    }

    public void Move(bool down)
    {
        //Should be called whenever the the pressure plate for them is triggered.
        //Unfortunately the debug log was never played, and so someway or somehow
        //the listener never caught the invoked event from the pressure plate.
        Debug.Log("bingus");
        if (down)
        {
            moveDownCommand = true;
            moveUpCommand = false;
        }
        else
        {
            moveDownCommand = false;
            moveUpCommand = true;
        }
    }
}
