using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeGridManager : MonoBehaviour {

    public GameObject Pipe1GameObject;
    public GameObject Pipe2aGameObject;
    public GameObject Pipe2bGameObject;
    public GameObject Pipe3GameObject;
    public GameObject Pipe4GameObject;

    public Color HighlightColor;
    public Color NonhighlightColor;

    private GameObject[,] allPipes;

    Vector2 sourcePipeCoordinates = new Vector2(0, 0);

    // Use this for initialization
    void Start () {
        g.PipeType[,] testBoard = new g.PipeType[2,3];
        testBoard[0, 0] = g.PipeType.Pipe2b;
        testBoard[0, 1] = g.PipeType.Pipe3;
        testBoard[0, 2] = g.PipeType.Pipe1;
        testBoard[1, 0] = g.PipeType.Pipe1;
        testBoard[1, 1] = g.PipeType.Pipe2b;
        testBoard[1, 2] = g.PipeType.Pipe1;

        testBoard = GetComponent<PipeGridGenerator>().GenerateRandomPuzzle(10, 10);


        initializeBoard(testBoard);
        RefreshColors();
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void initializeBoard(g.PipeType[,] board)
    {
        int height = board.GetLength(0);
        int width = board.GetLength(1);

        allPipes = new GameObject[height, width];

        for(int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                GameObject pipe;
                Vector3 offset = new Vector3(x * 3, y * -3, 0);
                switch (board[y, x])
                {
                    case g.PipeType.Pipe1:
                        {
                            pipe = GameObject.Instantiate(Pipe1GameObject, this.transform.position + offset, Quaternion.identity);
                            break;
                        }
                    case g.PipeType.Pipe2a:
                        {
                            pipe = GameObject.Instantiate(Pipe2aGameObject, this.transform.position + offset, Quaternion.identity);
                            break;
                        }
                    case g.PipeType.Pipe2b:
                        {
                            pipe = GameObject.Instantiate(Pipe2bGameObject, this.transform.position + offset, Quaternion.identity);
                            break;
                        }
                    case g.PipeType.Pipe3:
                        {
                            pipe = GameObject.Instantiate(Pipe3GameObject, this.transform.position + offset, Quaternion.identity);
                            break;
                        }
                    case g.PipeType.Pipe4:
                        {
                            pipe = GameObject.Instantiate(Pipe4GameObject, this.transform.position + offset, Quaternion.identity);
                            break;
                        }
                    default:
                        {
                            throw new System.Exception("Unrecognized Pipe Type: " + board[y, x]);
                        }
                }

                pipe.GetComponent<PipeManager>().SetColor(NonhighlightColor);
                allPipes[y, x] = pipe;
            }
        }
        
    }

    public void RefreshColors()
    {
        // Turns all pipes back to nonhighlight color
        for(int y = 0; y < allPipes.GetLength(0); ++y)
        {
            for(int x = 0; x < allPipes.GetLength(1); ++x)
            {
                allPipes[y, x].GetComponent<PipeManager>().SetColor(NonhighlightColor);
            }
        }
        
        fillPipeAndAllConnections((int)sourcePipeCoordinates.x,(int)sourcePipeCoordinates.y, HighlightColor);
    }

    // Called once on the source pipe, which then calls the recursive function for each of its connections (and then their connections, etc.)
    private void fillPipeAndAllConnections(int pipeX, int pipeY, Color colorToSet)
    {
        GameObject pipe = allPipes[pipeY, pipeX];
        pipe.GetComponent<PipeManager>().SetColor(colorToSet);
        foreach (g.ConnectionType connection in pipe.GetComponent<PipeManager>().Connections)
        {
            switch (connection)
            {
                case g.ConnectionType.Up:
                    {
                        if (areConnected(pipeX, pipeY, pipeX, pipeY - 1))
                        {
                            fillAllConnectionsRecursive(pipeX, pipeY - 1, g.ConnectionType.Down, HighlightColor);
                        }
                        break;
                    }
                case g.ConnectionType.Right:
                    {
                        if (areConnected(pipeX, pipeY, pipeX+1, pipeY))
                        {
                            fillAllConnectionsRecursive(pipeX+1, pipeY, g.ConnectionType.Left, HighlightColor);
                        }
                        break;
                    }
                case g.ConnectionType.Down:
                    {
                        if (areConnected(pipeX, pipeY, pipeX, pipeY + 1))
                        {
                            fillAllConnectionsRecursive(pipeX, pipeY + 1, g.ConnectionType.Up, HighlightColor);
                        }
                        break;
                    }
                case g.ConnectionType.Left:
                    {
                        if (areConnected(pipeX, pipeY, pipeX-1, pipeY))
                        {
                            fillAllConnectionsRecursive(pipeX-1, pipeY, g.ConnectionType.Right, HighlightColor);
                        }
                        break;
                    }
                default:
                    {
                        throw new System.Exception("Unrecognized direction: " + connection);
                    }
            }
        }
    }

    private void fillAllConnectionsRecursive(int pipeX, int pipeY, g.ConnectionType directionToIgnore, Color colorToSet)
    {
        GameObject pipe = allPipes[pipeY, pipeX];
        pipe.GetComponent<PipeManager>().SetColor(colorToSet);
        foreach (g.ConnectionType connection in pipe.GetComponent<PipeManager>().Connections)
        {
            if(connection != directionToIgnore)
            {
                switch (connection)
                {
                    case g.ConnectionType.Up:
                        {
                            if (areConnected(pipeX, pipeY, pipeX, pipeY - 1))
                            {
                                fillAllConnectionsRecursive(pipeX, pipeY - 1, g.ConnectionType.Down, HighlightColor);
                            }
                            break;
                        }
                    case g.ConnectionType.Right:
                        {
                            if (areConnected(pipeX, pipeY, pipeX + 1, pipeY))
                            {
                                fillAllConnectionsRecursive(pipeX + 1, pipeY, g.ConnectionType.Left, HighlightColor);
                            }
                            break;
                        }
                    case g.ConnectionType.Down:
                        {
                            if (areConnected(pipeX, pipeY, pipeX, pipeY + 1))
                            {
                                fillAllConnectionsRecursive(pipeX, pipeY + 1, g.ConnectionType.Up, HighlightColor);
                            }
                            break;
                        }
                    case g.ConnectionType.Left:
                        {
                            if (areConnected(pipeX, pipeY, pipeX- 1, pipeY))
                            {
                                fillAllConnectionsRecursive(pipeX - 1, pipeY, g.ConnectionType.Right, HighlightColor);
                            }
                            break;
                        }
                    default:
                        {
                            throw new System.Exception("Unrecognized direction: " + connection);
                        }
                }
            }
        }
    }

    private bool areConnected(int pipe1X, int pipe1Y, int pipe2X, int pipe2Y)
    {
        int width = allPipes.GetLength(1);
        int height = allPipes.GetLength(0);

        if (pipe1X < 0 || pipe2X < 0 || pipe1X >= width || pipe2X >= width || pipe1Y < 0 || pipe1Y >= height || pipe2Y < 0 || pipe2Y >= height)
        {
            return false;
        }

        GameObject pipe1 = allPipes[pipe1Y, pipe1X];
        GameObject pipe2 = allPipes[pipe2Y, pipe2X];

        // If up connection
        if (pipe1X == pipe2X && pipe1Y -1 == pipe2Y)
        {
            return pipe1.GetComponent<PipeManager>().Connections.Contains(g.ConnectionType.Up) && pipe2.GetComponent<PipeManager>().Connections.Contains(g.ConnectionType.Down);
        }
        // If right connection
        else if(pipe1Y == pipe2Y && pipe1X + 1 == pipe2X)
        {
            return pipe1.GetComponent<PipeManager>().Connections.Contains(g.ConnectionType.Right) && pipe2.GetComponent<PipeManager>().Connections.Contains(g.ConnectionType.Left);
        }
        // If down connection
        else if (pipe1X == pipe2X && pipe1Y + 1 == pipe2Y)
        {
            return pipe1.GetComponent<PipeManager>().Connections.Contains(g.ConnectionType.Down) && pipe2.GetComponent<PipeManager>().Connections.Contains(g.ConnectionType.Up);
        }
        // If left connection
        else if (pipe1Y == pipe2Y && pipe1X - 1 == pipe2X)
        {
            return pipe1.GetComponent<PipeManager>().Connections.Contains(g.ConnectionType.Left) && pipe2.GetComponent<PipeManager>().Connections.Contains(g.ConnectionType.Right);
        }
        return false;
    }

}
