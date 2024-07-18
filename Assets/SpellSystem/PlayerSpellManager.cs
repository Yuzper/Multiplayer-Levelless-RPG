using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpellManager : CharacterSpellManager
{

    private int[,] drawingGrid;
    private int gridWidth = 100; // Set grid width
    private int gridHeight = 100; // Set grid height
    private int drawAreaSize = 700;
    private int drawAreaHalfSize = 350;

    protected override void Awake()
    {
        base.Awake();
        drawingGrid = new int[gridWidth, gridHeight];
    }

    protected override void Update()
    {
        if(IsOwner)
        {
            if (PlayerInputManager.instance.isDrawing)
            {
                Draw();
            }
            else if(PlayerInputManager.instance.doneDrawing)
            {
                PlayerInputManager.instance.doneDrawing = false;
                FinishedDrawingRune();
            }
        }



        //if(IsOwner)
        //{
        //    if (PlayerInputManager.instance.inSpellMode)
        //    {
        //        if(character.characterSpellManager.equippedSpell.GetType() == typeof(BeamSpell)) 
        //        {
        //            if (PlayerInputManager.instance.castSpellHold)
        //            {
        //                //TODO continue here
        //            } 
        //        }
        //        else if (PlayerInputManager.instance.castSpell)
        //        {
        //            PlayerInputManager.instance.castSpell = false;
        //            if (character.characterNetworkManager.currentMana.Value < equippedSpell.manaCost) return;
        //            character.characterNetworkManager.currentMana.Value -= equippedSpell.manaCost;
        //            equippedSpell.UseSpell(character);
        //        }
        //    }
        //}
    }

    public override void SpawnHandVFX()
    {
        base.SpawnHandVFX();
    }

    public override void RemoveHandVFX()
    {
        base.RemoveHandVFX();
    }

    private void FinishedDrawingRune()
    {
        //PrintDrawingGrid();

        // Example input data (should match the expected input shape and size)
        float[] inputData = new float[drawingGrid.GetLength(0) * drawingGrid.GetLength(1)];
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                int index = i * gridWidth + j;
                inputData[index] = drawingGrid[i, j];
            }
        }

        // Call the inference (prediction) method
        var predictedList = PlayerInputManager.instance.onnxModel.RunInference(inputData);
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

        EquipMostLikelySpell(maxIndex);




        drawingGrid = new int[gridWidth, gridHeight];
    }

    private void Draw()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector2Int gridPosition = GetGridPosition(mousePosition);

        if (IsValidGridPosition(gridPosition))
        {
            drawingGrid[gridPosition.x, gridPosition.y] = 1;
            // Add visual feedback for drawing here (e.g., draw a pixel or sprite at gridPosition)
        }
    }

    private Vector2Int GetGridPosition(Vector2 mousePosition)
    {
        // Calculate the center of the screen
        float screenCenterX = Screen.width / 2f;
        float screenCenterY = Screen.height / 2f;

        // Check if the mouse position is within the drawing area
        if (mousePosition.x >= screenCenterX - drawAreaHalfSize && mousePosition.x <= screenCenterX + drawAreaHalfSize &&
            mousePosition.y >= screenCenterY - drawAreaHalfSize && mousePosition.y <= screenCenterY + drawAreaHalfSize)
        {
            // Convert mouse position to grid coordinates relative to the drawing area
            int x = Mathf.FloorToInt((mousePosition.x - (screenCenterX - drawAreaHalfSize)) * gridWidth / drawAreaSize);
            int y = Mathf.FloorToInt((mousePosition.y - (screenCenterY - drawAreaHalfSize)) * gridHeight / drawAreaSize);
            return new Vector2Int(x, y);
        }

        // Return an invalid position if the mouse is outside the drawing area
        return new Vector2Int(-1, -1);
    }

    private bool IsValidGridPosition(Vector2Int position)
    {
        return position.x >= 0 && position.x < gridWidth && position.y >= 0 && position.y < gridHeight;
    }

    private void PrintDrawingGrid()
    {
        string gridOutput = "";
        for (int y = gridHeight - 1; y >= 0; y--) // Print from top to bottom
        {
            for (int x = 0; x < gridWidth; x++)
            {
                gridOutput += drawingGrid[x, y].ToString() + " ";
            }
            gridOutput += "\n";
        }
        Debug.Log(gridOutput);
    }

}
