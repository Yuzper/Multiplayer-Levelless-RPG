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

    public void RunInference(float[] inputData)
    {
        // Assuming input data is in the shape [1, 100, 100, 1]
        Tensor inputTensor = new Tensor(1, 100, 100, 1, inputData);

        // Execute the model with the input tensor
        worker.Execute(inputTensor);

        // Fetch the output tensor
        Tensor outputTensor = worker.PeekOutput();

        // Process the output as needed
        float[] outputData = outputTensor.ToReadOnlyArray();
        Debug.Log("Inference Output: " + string.Join(", ", outputData));

        // Dispose the tensors when done
        inputTensor.Dispose();
        outputTensor.Dispose();
    }

    void OnDestroy()
    {
        // Dispose the worker when the script is destroyed
        worker.Dispose();
    }
}
