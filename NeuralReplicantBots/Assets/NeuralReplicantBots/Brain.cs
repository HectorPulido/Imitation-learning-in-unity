using UnityEngine;
using LinearAlgebra;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace NeuralReplicantBot.PerceptronHandler
{
    public class Brain : MonoBehaviour
    {
        public int epoch = 5000;
        public int[] topology;
        public int lossShowRate;
        public int batchSize;
        public ActivationFunction activationFunction;
        public float learningRate = 0.8f;
        public bool useRelativeDataPath = true;
        public string dataPath;
        public bool save;

        [HideInInspector]
        public bool load = false;
        NeuralNetwork nn;

        private void Start()
        {
            if (dataPath == "" || dataPath == null)
                dataPath = useRelativeDataPath ? Application.dataPath + "/Data/data.nn" : "c://data.nn";
            else
                dataPath = useRelativeDataPath ? Application.dataPath + dataPath : dataPath;

            if (load)
            {
                try
                {
                    FileStream fs = new FileStream(dataPath, FileMode.Open);
                    BinaryFormatter formatter = new BinaryFormatter();
                    nn = new NeuralNetwork((Matrix[])formatter.Deserialize(fs), new System.Random(1), activationFunction);
                    fs.Close();
                }
                catch (SerializationException e)
                {
                    Debug.LogError(e.Message);
                    nn = new NeuralNetwork(topology, new System.Random(1), activationFunction);
                }
            }
            else
            {
                nn = new NeuralNetwork(topology, new System.Random(1), activationFunction);
            }

        }

        public void Learn(Matrix input, Matrix output)
        {           
            nn.Learn(input, output, learningRate, epoch, (s) => { Debug.Log(s); }, lossShowRate, batchSize);
            if (save)
            {
                try
                {
                    FileStream fs = new FileStream(dataPath, FileMode.OpenOrCreate);
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, nn.W);
                    fs.Close();
                }
                catch (SerializationException e)
                {
                    throw e;
                }
            }
        }

        public Matrix GetOutput(Matrix input)
        {
            return nn.GetOutput(input);
        }

    }
}