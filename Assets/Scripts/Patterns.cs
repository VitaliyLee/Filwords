using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patterns
{
    private static List<Vector2Int[,]> patternsList = new List<Vector2Int[,]>()
    {
        new Vector2Int[,]
        {
            { new Vector2Int(0, 3), new Vector2Int(0, 2), new Vector2Int(0, 1), new Vector2Int(0, 0), new Vector2Int(1, 0) },
            { new Vector2Int(0, 4), new Vector2Int(1, 4), new Vector2Int(1, 3), new Vector2Int(1, 2), new Vector2Int(1, 1) },
            { new Vector2Int(2, 0), new Vector2Int(3, 0), new Vector2Int(4, 0), new Vector2Int(4, 1), new Vector2Int(4, 2) },
            { new Vector2Int(4, 3), new Vector2Int(3, 3), new Vector2Int(3, 2), new Vector2Int(3, 1), new Vector2Int(2, 1) },
            { new Vector2Int(2, 2), new Vector2Int(2, 3), new Vector2Int(2, 4), new Vector2Int(3, 4), new Vector2Int(4, 4) }
        }
    };

    public static List<Vector2Int[,]> PatternsList { get => patternsList;}

    public static Vector2Int[,] Rotate90(Vector2Int[,] matrix)
    {
        int n = matrix.GetLength(0);
        int m = matrix.GetLength(1);
        Vector2Int[,] rotated = new Vector2Int[m, n];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                rotated[m - j - 1, i] = matrix[i, j];
            }
        }

        return rotated;
    }
}
