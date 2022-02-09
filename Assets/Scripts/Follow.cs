using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    //reference https://raw.githubusercontent.com/Nova840/Miscellaneous/master/Follow.cs
    //Explaination https://www.youtube.com/watch?v=NFBEgKd1mSc

    public Transform follow_child = null;

    private Vector3 originalLocalPosition;
    private Quaternion originalLocalRotation;

    private void Awake()
    {
        if (follow_child != null)
        {
            originalLocalPosition = follow_child.localPosition;
            originalLocalRotation = follow_child.localRotation;
        }
    }


    public void update_location_and_rotation()
    {
        if (follow_child != null)
        {
            originalLocalPosition = follow_child.localPosition;
            originalLocalRotation = follow_child.localRotation;
        }
    }

    public void move_parent_relative_toChild()
    {
        if (follow_child != null)
        {
            //move the parent to child's position
            transform.position = follow_child.position;

            //HAS TO BE IN THIS ORDER
            //sort of "reverses" the quaternion so that the local rotation is 0 if it is equal to the original local rotation
            follow_child.RotateAround(follow_child.position, follow_child.forward, -originalLocalRotation.eulerAngles.z);
            follow_child.RotateAround(follow_child.position, follow_child.right, -originalLocalRotation.eulerAngles.x);
            follow_child.RotateAround(follow_child.position, follow_child.up, -originalLocalRotation.eulerAngles.y);

            //rotate the parent
            transform.rotation = follow_child.rotation;

            //moves the parent by the child's original offset from the parent
            transform.position += -transform.right * originalLocalPosition.x;
            transform.position += -transform.up * originalLocalPosition.y;
            transform.position += -transform.forward * originalLocalPosition.z;

            //resets local rotation, undoing step 2
            follow_child.localRotation = originalLocalRotation;

            //reset local position
            follow_child.localPosition = originalLocalPosition;

        }
    }

}