using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public class TestingDrawing : MonoBehaviour
{
    public RuneDrawer drawer;
    public OnnxInferenceBarracuda onnxModel;

    private float canvasWidth = 700;
    private float canvasHeight = 700;
    public int gridWidth = 100;
    public int gridHeight = 100;


    public int[,] matrix;
    private float[] predictedList;

    public static PlayerInputManager instance;

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

        // Example input data (should match the expected input shape and size)
        float[] inputData = new float[matrix.GetLength(0) * matrix.GetLength(1)];
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                int index = i * gridWidth + j;
                inputData[index] = matrix[i, j];
            }
        }

        // Call the inference (prediction) method
        predictedList = onnxModel.RunInference(inputData);
        int maxIndex = 0;
        float maxValue = predictedList[0];

        for (int i = 1; i < predictedList.Length; i++)
        {
            if (predictedList[i] > maxValue)
            {
                maxValue = predictedList[i];
                maxIndex = i;
            }
        }

        PlayerInputManager.instance.player.playerSpellManager.CastMostLikelySpell(maxIndex);
        drawer.ClearDrawing();
    }



    (int, int) ToOurMap(Vector2 coord)
    {
        return TranslateCoordinates(coord);
    }

    public (int, int) TranslateCoordinates(Vector2 point)
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
        //    Debug.Log($"Matrix written to {path}");
    }



    private void CreateMatrix(List<Vector2> drawing)
    {
        // for each pixel
        for (int i = 0; i < drawing.Count; i++)
        {
            var (coordx, coordy) = ToOurMap(new Vector2(drawing[i].x, drawing[i].y));
            //Debug.Log(drawing[i].x + " " + drawing[i].y);
            //Debug.Log(coordx + " " + coordy);
            matrix[coordx, coordy] = 1;

        }
    }
}
