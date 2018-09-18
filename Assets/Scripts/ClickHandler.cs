using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {

                #region Left click
                if (Input.GetMouseButtonDown(0))
                {
                    switch (hit.transform.tag)
                    {
                        case "Pipe Hit Box":
                            {
                                hit.transform.GetComponentInParent<PipeManager>().RotateNinetyDegrees();
                                GameObject pipeGridManager = GameObject.FindGameObjectWithTag("Pipe Grid Manager");
                                pipeGridManager.GetComponent<PipeGridManager>().RefreshColors();
                                break;
                            }
                        default:
                            {
                                // Do nothing... ??? !!!
                                break;
                            }
                    }
                }
                #endregion

                #region Right click
                else if (Input.GetMouseButtonDown(1))
                {
                    switch (hit.transform.tag)
                    {

                    }
                }
                #endregion

                #region Middle mouse click
                else if (Input.GetMouseButtonDown(2))
                {
                    switch (hit.transform.tag)
                    {

                    }
                }
                #endregion
            }
        }
    }
}