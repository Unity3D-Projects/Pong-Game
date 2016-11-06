namespace PongBrain.Pong.AI.Neural {
    
/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class Layer {
    public float Bias;
    public Layer PreviousLayer;

    public Neuron[] Neurons;

    private static readonly Random s_Random = new Random();


    public Layer(Neuron[] neurons) {
        foreach (var neuron in neurons) {
            neuron.Layer = this;
        }

        Bias = (float)s_Random.NextDouble();
    }
}

public class Neuron {
    public float Error;
    public Layer Layer;
    public readonly Neuron[] InputNeurons;
    public readonly float[] InputWeights;

    private static readonly Random s_Random = new Random();

    public float Output { get; set; }

    public Neuron(Neuron[] inputs) {
        if (inputs == null) {
            return;
        }

        InputNeurons = inputs;
        InputWeights = new float[inputs.Length];

        RandomizeWeights();
    }

    public void CalcOutput() {
        if (InputNeurons == null) {
            return;
        }

        var r = Layer.Bias*1.0f;

        for (var i = 0; i < InputNeurons.Length; i++) {
            InputNeurons[i].CalcOutput();
            r += InputNeurons[i].Output * InputWeights[i];
        }

        Output = 1.0f / (1.0f + (float)Math.Pow(Math.E, -r));
    }

    private void RandomizeWeights() {
        for (var i = 0; i < InputWeights.Length; i++) {
            InputWeights[i] = (float)s_Random.NextDouble();
        }
    }
}

public class NeuralNetwork {
    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/

    private readonly Neuron[] m_InputLayer;
    private readonly Neuron[] m_OutputLayer;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public NeuralNetwork(int numInputs, int numOutputs) {
        m_InputLayer  = new Neuron[numInputs];
        for (var i = 0; i < numInputs; i++) {
            m_InputLayer[i] = new Neuron(null);
        }


        var layer1 = CreateLayer(2, m_InputLayer);

        m_OutputLayer = new Neuron[numOutputs];
        for (var i = 0; i < numOutputs; i++) {
            m_OutputLayer[i] = new Neuron(layer1);
        }

        new Layer(m_InputLayer);
        new Layer(layer1);
        new Layer(m_OutputLayer);
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public float[] Calc(params float[] inputs) {
        for (var i = 0; i < m_InputLayer.Length; i++) {
            m_InputLayer[i].Output = inputs[i];
        }

        var outputs = new float[m_OutputLayer.Length];
        for (var i = 0; i < m_OutputLayer.Length; i++) {
            m_OutputLayer[i].CalcOutput();
            outputs[i] = m_OutputLayer[i].Output;
        }

        return outputs;
    }

    public void Train(float[] inputs, float[] targets) {
        var rate = 0.5f;

        Calc(inputs);

        var eT = 0.0f;
        for (var i = 0; i < targets.Length; i++) {
            var e  = targets[i] - m_OutputLayer[i].Output;
            var es = e*e;
            var hes = 0.5f*es;
            eT += hes;

            m_OutputLayer[i].Error = hes;
        }
        Console.WriteLine("total error is " + eT);
    }

    private Neuron[] CreateLayer(int numNeurons, Neuron[] previousLayer=null) {
        var neurons = new Neuron[numNeurons];

        for (var i = 0; i < numNeurons; i++) {
            neurons[i] = new Neuron(previousLayer);
        }

        return neurons;
    }
}

}
