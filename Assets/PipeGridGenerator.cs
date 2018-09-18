using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeGridGenerator : MonoBehaviour {

    public g.PipeType [,] GenerateRandomPuzzle(int width, int height)
    {
        System.Random random = new System.Random();
        
        Dictionary<int, List<PuzzleNode>> puzzleNodesByGroupID = new Dictionary<int, List<PuzzleNode>>();

        puzzleNodesByGroupID[int.MaxValue] = new List<PuzzleNode>();
        puzzleNodesByGroupID[0] = new List<PuzzleNode>();

        PuzzleNode[,] allPuzzleNodes = new PuzzleNode[height, width];

        // Initializes all unconnected points of the puzzle
        for(int x = 0; x < width; ++x)
        {
            for(int y = 0; y < height; ++y)
            {
                PuzzleNode currentNode = new PuzzleNode(new Vector2(x, y));
                puzzleNodesByGroupID[int.MaxValue].Add(currentNode);
                allPuzzleNodes[y, x] = currentNode;
            }
        }
        
        // Makes all possible connections the puzzle can have
        List<Connection> allConnections = new List<Connection>();
        for (int x = 0; x < width; ++x)
        {
            for(int y = 0; y < height; ++y)
            {
                if(x > 0)
                {
                    allConnections.Add(new Connection(allPuzzleNodes[y, x], allPuzzleNodes[y, x - 1],g.ConnectionType.Left,g.ConnectionType.Right));
                }
                if (x < width-1)
                {
                    allConnections.Add(new Connection(allPuzzleNodes[y, x], allPuzzleNodes[y, x + 1],g.ConnectionType.Right,g.ConnectionType.Left));
                }
                if (y > 0)
                {
                    allConnections.Add(new Connection(allPuzzleNodes[y, x], allPuzzleNodes[y-1, x],g.ConnectionType.Up,g.ConnectionType.Down));
                }
                if (y < height -1)
                {
                    allConnections.Add(new Connection(allPuzzleNodes[y, x], allPuzzleNodes[y+1, x], g.ConnectionType.Down,g.ConnectionType.Up));
                }
            }
        }

        // Shuffles connections
        g.ShuffleList<Connection>(ref allConnections);
        int connectionIndex = 0;
        int currentGroupID = 0;

        List<g.ConnectionType> allConnectionTypes = new List<g.ConnectionType>() { g.ConnectionType.Up, g.ConnectionType.Right, g.ConnectionType.Down, g.ConnectionType.Left };

        // Keep making connections until everything is connected
        while (puzzleNodesByGroupID[0].Count != width * height)
        {
            // WARNING: IF IT'S NOT POSSIBLE TO CREATE A CONNECTED GRAPH, THIS WILL FAIL.
            Connection currentConnection = allConnections[connectionIndex++];
            PuzzleNode fromNode = currentConnection.FromNode;
            PuzzleNode toNode = currentConnection.ToNode;
            
            if(fromNode.GroupID == int.MaxValue)
            {
                // Create the connection and assign it new group id
                if (toNode.GroupID == int.MaxValue)
                {
                    puzzleNodesByGroupID[currentGroupID] = new List<PuzzleNode>();
                    puzzleNodesByGroupID[currentGroupID].Add(fromNode);
                    puzzleNodesByGroupID[currentGroupID].Add(toNode);
                    puzzleNodesByGroupID[int.MaxValue].Remove(fromNode);
                    puzzleNodesByGroupID[int.MaxValue].Remove(toNode);

                    fromNode.GroupID = currentGroupID;
                    toNode.GroupID = currentGroupID++;
                    fromNode.Connections.Add(currentConnection.FromConnectionType);
                    toNode.Connections.Add(currentConnection.ToConnectionType);
                }
                // Create the connection and add fromNode to toNode's group
                else
                {
                    fromNode.GroupID = toNode.GroupID;
                    fromNode.Connections.Add(currentConnection.FromConnectionType);
                    toNode.Connections.Add(currentConnection.ToConnectionType);
                    puzzleNodesByGroupID[toNode.GroupID].Add(fromNode);
                    puzzleNodesByGroupID[int.MaxValue].Remove(fromNode);
                }
            }
            // Create the connection and add toNode to fromNode's group
            else if (toNode.GroupID == int.MaxValue)
            {
                toNode.GroupID = fromNode.GroupID;
                fromNode.Connections.Add(currentConnection.FromConnectionType);
                toNode.Connections.Add(currentConnection.ToConnectionType);
                puzzleNodesByGroupID[fromNode.GroupID].Add(toNode);
                puzzleNodesByGroupID[int.MaxValue].Remove(toNode);
            }
            // If they're both apart of groups already AND not the same, need to change ALL nodes with higher group ID to lower group ID
            else if (toNode.GroupID != fromNode.GroupID)
            {
                fromNode.Connections.Add(currentConnection.FromConnectionType);
                toNode.Connections.Add(currentConnection.ToConnectionType);

                int higherGroupID = Mathf.Max(fromNode.GroupID, toNode.GroupID);
                int lowerGroupID = Mathf.Min(fromNode.GroupID, toNode.GroupID);

                foreach(PuzzleNode node in puzzleNodesByGroupID[higherGroupID])
                {
                    node.GroupID = lowerGroupID;
                }

                puzzleNodesByGroupID[lowerGroupID].AddRange(puzzleNodesByGroupID[higherGroupID]);
                puzzleNodesByGroupID[higherGroupID] = new List<PuzzleNode>();
            }

        }
        g.PipeType[,] finalPuzzle = new g.PipeType[height, width];

        for(int x = 0; x < width; ++x)
        {
            for(int y = 0; y < height; ++y)
            {
                PuzzleNode node = allPuzzleNodes[y, x];
                switch (node.Connections.Count)
                {
                    case 1:
                        {
                            finalPuzzle[y, x] = g.PipeType.Pipe1;
                            break;
                        }
                    case 2:
                        {
                            if((node.Connections.Contains(g.ConnectionType.Up) && node.Connections.Contains(g.ConnectionType.Down)) || (node.Connections.Contains(g.ConnectionType.Left) && node.Connections.Contains(g.ConnectionType.Right)))
                            {
                                finalPuzzle[y, x] = g.PipeType.Pipe2a;
                            }
                            else
                            {
                                finalPuzzle[y, x] = g.PipeType.Pipe2b;
                            }
                            break;
                        }
                    case 3:
                        {
                            finalPuzzle[y, x] = g.PipeType.Pipe3;
                            break;
                        }
                    case 4:
                        {
                            finalPuzzle[y, x] = g.PipeType.Pipe4;
                            break;
                        }
                    default:
                        {
                            throw new System.Exception("Unrecognized pipe type: " + node.Connections);
                        }
                }
            }
        }
        
        return finalPuzzle;
    }

    class Connection
    {
        public PuzzleNode FromNode;
        public PuzzleNode ToNode;
        public g.ConnectionType FromConnectionType;
        public g.ConnectionType ToConnectionType;

        public Connection(PuzzleNode fromNode, PuzzleNode toNode, g.ConnectionType fromConnectionType, g.ConnectionType toConnectionType)
        {
            FromNode = fromNode;
            ToNode = toNode;
            FromConnectionType = fromConnectionType;
            ToConnectionType = toConnectionType;
        }
    }

    class PuzzleNode
    {
        public Vector2 Coordinates;
        public int GroupID;
        public List<g.ConnectionType> Connections;

        public PuzzleNode(Vector2 coordinates)
        {
            Coordinates = coordinates;
            GroupID = int.MaxValue;
            Connections = new List<g.ConnectionType>();
        }
    }

}
