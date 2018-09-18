using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class g
{
    public static System.Random Random = new System.Random();

    public enum PipeType { Pipe1, Pipe2a, Pipe2b, Pipe3, Pipe4 }

    public enum ConnectionType { Up, Down, Left, Right }


    public static void ShuffleList<T>(ref List<T> inputList)
    {
        List<T> inputListCopy = new List<T>(inputList);
        List<T> shuffledList = new List<T>();
        for(int i = 0; i < inputList.Count; ++i)
        {
            int randomIndex = Random.Next(inputListCopy.Count);
            shuffledList.Add(inputListCopy[randomIndex]);
            inputListCopy.RemoveAt(randomIndex);
        }
        inputList = shuffledList;
    }
}
