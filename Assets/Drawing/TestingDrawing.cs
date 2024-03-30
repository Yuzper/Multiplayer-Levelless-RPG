using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TestingDrawing : MonoBehaviour
{
    public RuneDrawer drawer;

    private float canvasWidth = 700;
    private float canvasHeight = 700;
    public int gridWidth = 100;
    public int gridHeight = 100;


    public int[,] matrix;

    private void Awake()
    {
        matrix = new int[gridWidth, gridHeight];
        canvasWidth = drawer.rectTransform.rect.width;
        canvasHeight = drawer.rectTransform.rect.height;
    }


    public void DoneDrawing()
    {
        var drawing = drawer.drawingCoordinates;
        CreateMatrix(drawing);
        DrawMatrix();
        drawer.ClearDrawing();
    }



    (int, int) ToOurMap(Vector2 coord)
    {
        return TranslateCoordinates(coord);
    }

    public (int,int) TranslateCoordinates(Vector2 point)
    {

            float xRatio = gridWidth / (float)canvasWidth;
            float yRatio = gridHeight / (float)canvasHeight;

            int cellX = Mathf.FloorToInt(point.x * xRatio);
            int cellY = Mathf.FloorToInt(point.y * yRatio);

            return (cellX, cellY);
    }

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
}