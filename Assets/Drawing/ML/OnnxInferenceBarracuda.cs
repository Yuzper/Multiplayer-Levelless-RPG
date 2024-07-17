using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;

public class OnnxInferenceBarracuda : MonoBehaviour
{
    public NNModel modelAsset;
    private Model model;
    private IWorker worker;

    void Start()
    {
        // Load the ONNX model
        model = ModelLoader.Load(modelAsset);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);
    }

    public float[] RunInference(float[] inputData)
    {
        // Assuming input data is in the shape [1, 100, 100, 1]
        Tensor inputTensor = new Tensor(1, 100, 100, 1, inputData);

        // Execute the model with the input tensor
        worker.Execute(inputTensor);

        // Fetch the output tensor
        Tensor outputTensor = worker.PeekOutput();

        // Process the output as needed
        float[] predictedList = outputTensor.ToReadOnlyArray();
        Debug.Log("Prediction Output: " + string.Join(", ", predictedList));

        // Dispose the tensors when done
        inputTensor.Dispose();
        outputTensor.Dispose();
        return predictedList;
    }

    void OnDestroy()
    {
        // Dispose the worker when the script is destroyed
        worker.Dispose();
    }
}
