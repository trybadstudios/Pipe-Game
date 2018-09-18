using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    public void RotateNinetyDegrees()
    {
        this.transform.Rotate(0, 0, 270);
    }

}
