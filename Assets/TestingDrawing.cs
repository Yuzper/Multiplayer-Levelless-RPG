using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TestingDrawing : MonoBehaviour
{
    public RectTransform rectTransform;
    public RuneDrawer drawer;


    public int resolutionForDrawingX = 700;
    public int resolutionForDrawingY = 700;

    public int[,] matrix;

    private void Awake()
    {

        var (maxx, maxy) = ToOurMap(new Vector2(resolutionForDrawingX, resolutionForDrawingY));
        Debug.Log(maxx + " " +maxy);
        matrix = new int[resolutionForDrawingX, resolutionForDrawingY];
    }


    public void DoneDrawing()
    {
        var drawing = drawer.points;
        CreateMatrix(drawing);
        DrawMatrix();
    }


    (int, int) ToOurMap(Vector2 coord)
    {
        Vector2 drawingAreaSize = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
        return MapCoordinateToArrayIndex(coord, drawingAreaSize, resolutionForDrawingX, resolutionForDrawingY);
    }

    //void DrawMatrix()
    //{
    //    string arrayContents = "";

    //    for (int i = 0; i < matrix.GetLength(0); i++)
    //    {
    //        for (int j = 0; j < matrix.GetLength(1); j++)
    //        {
    //            arrayContents += matrix[i, j].ToString() + " ";
    //        }
    //        arrayContents += "\n"; // New line for each row
    //    }

    //    Debug.Log(arrayContents);
    //}

    void DrawMatrix()
    {
        string arrayContents = "";
        string path = Path.Combine(Application.persistentDataPath, "matrix.txt");

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                arrayContents += matrix[i, j].ToString() + " ";
            }
            arrayContents += "\n"; // New line for each row
        }

        File.WriteAllText(path, arrayContents);
        Debug.Log($"Matrix written to {path}");
    }



    private void CreateMatrix(List<Vector2> drawing)
    {
        // for each pixel
        for (int i = 0; i < drawing.Count; i++)
        {
            var (coordx, coordy) = ToOurMap(new Vector2(drawing[i].x, drawing[i].y));
            Debug.Log(drawing[i].x + " " + drawing[i].y);
            Debug.Log(coordx + " " + coordy);
            matrix[coordx, coordy] = 1;

        }
    }

    public (int, int) MapCoordinateToArrayIndex(Vector2 coordinate, Vector2 drawingAreaSize, int arrayWidth, int arrayHeight)
    {
        // Normalize the coordinates based on the drawing area size
        float normalizedX = coordinate.x / drawingAreaSize.x;
        float normalizedY = coordinate.y / drawingAreaSize.y;

        // Scale the normalized coordinates to array indices
        int i = (int)(normalizedX * arrayWidth);
        int j = (int)(normalizedY * arrayHeight);

        // Ensure the indices are within the bounds of the array
        i = Mathf.Clamp(i, 0, arrayWidth - 1);
        j = Mathf.Clamp(j, 0, arrayHeight - 1);

        return (i, j);
    }
}
