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
        [SerializeField]
        public double learningRate = 0.8;
        public string path;
        public bool save;
        public bool load;
        NeuralNetwork nn;

        private void Start()
        {
            if (load)
            {
                try
                {
                    FileStream fs = new FileStream(path, FileMode.Open);
                    BinaryFormatter formatter = new BinaryFormatter();
                    nn = new NeuralNetwork((Matrix[])formatter.Deserialize(fs));
                    fs.Close();

                }
                catch (SerializationException e)
                {
                    print(e.Message);
                }
            }
            else
            {
                nn = new NeuralNetwork(topology, new System.Random(1));
            }

        }

        public void Learn(Matrix input, Matrix output)
        {
            nn.Learn(input, output, learningRate, epoch, (s) => { print(s); });
            if (save)
            {
                try
                {
                    FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
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