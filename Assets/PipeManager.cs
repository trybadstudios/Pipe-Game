using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PipeManager : MonoBehaviour
{
    public List<g.ConnectionType> Connections;

    public void SetColor(Color color)
    {
        GetComponent<HighlightManager>().SetHighlight(color);
    }

    public void RotateNinetyDegrees()
    {
        this.transform.Rotate(0, 0, 270);
        List<g.ConnectionType> newConnections = new List<g.ConnectionType>();
        foreach(g.ConnectionType connection in Connections)
        {
            switch (connection)
            {
                case g.ConnectionType.Up:
                    {
                        newConnections.Add(g.ConnectionType.Right);
                        break;
                    }
                case g.ConnectionType.Right:
                    {
                        newConnections.Add(g.ConnectionType.Down);
                        break;
                    }
                case g.ConnectionType.Down:
                    {
                        newConnections.Add(g.ConnectionType.Left);
                        break;
                    }
                case g.ConnectionType.Left:
                    {
                        newConnections.Add(g.ConnectionType.Up);
                        break;
                    }
                default:
                    {
                        throw new System.Exception("Unrecognized connection type: " + connection);
                    }
            }
        }
        Connections = newConnections;
    }
}

