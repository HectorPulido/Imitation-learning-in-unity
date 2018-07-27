using System;
using LinearAlgebra;

namespace NeuralReplicantBot.PerceptronHandler
{
    public enum ActivationFunction { ReLU, Sigmoid }

    public class NeuralNetwork
    {
        Random r;
        ActivationFunction a;

        public int[] NeuronCount;
        public int LayerCount { get { return NeuronCount.Length; } }

        public Matrix[] W;

        public NeuralNetwork(int[] NeuronCount, Random r, ActivationFunction a)
        {
            this.NeuronCount = NeuronCount;
            this.r = r;
            this.a = a;

            W = new Matrix[LayerCount - 1];
            for (int i = 0; i < W.Length; i++)
            {
                W[i] = Matrix.Random(NeuronCount[i] + 1, NeuronCount[i + 1], r) * 2 - 1;
            }
        }

        public NeuralNetwork(Matrix[] W, Random r, ActivationFunction a)
        {
            this.W = W;
            this.r = r;
            this.a = a;

            NeuronCount = new int[W.Length + 1];

            NeuronCount[0] = W[0].X;

            for (int i = 1; i < W.Length; i++)
            {
                NeuronCount[i] = W[i].Y;
            }
        }


        public void Learn(Matrix InputValue, Matrix OutputValue, 
            double LearningRate, int epoch, 
            Action<string> LossAction, int s = 10, int batchsize = 50)
        {
            var ExampleCount = InputValue.X;

            for (int e = 0; e < epoch; e++)
            {
                var k = ShuffleMultiplesRows(new Matrix[] { InputValue, OutputValue }, r);

                InputValue = k[0];
                OutputValue = k[1];

                if(batchsize == 0)
                    batchsize = InputValue.X;

                for (int i = 0; i < InputValue.X / batchsize; i++)
                {                    
                    //BATCH
                    var xbatch = InputValue.Slice(batchsize * i, 0, batchsize * (i + 1), InputValue.Y);
                    var ybatch = OutputValue.Slice(batchsize * i, 0, batchsize * (i + 1), OutputValue.Y);

                    //FORWARDPROPAGATION
                    Matrix[] Z, A;
                    ForwardPropagation(out Z, out A, batchsize, xbatch);

                    Matrix Zlast = Z[LayerCount - 1].Slice(0, 1, Z[LayerCount - 1].X, Z[LayerCount - 1].Y);
                    Matrix output = A[A.Length - 1].Slice(0, 1, A[A.Length - 1].X, A[A.Length - 1].Y);

                    //BACKPROPAGATION
                    Matrix[] error, delta;
                    BackPropagation(out delta, out error, output, ybatch, Zlast, Z, A);
                    Matrix LastError = error[LayerCount - 1];

                    //Gradient Descend
                    GradientDescend(A, delta, LearningRate);

                    if(i == 0 && e % s == 0)
                        LossAction("Loss: " + LastError.Pow(2).Avg);
                }                
            }
        }
        public Matrix GetOutput(Matrix InputValue)
        {
            var ExampleCount = InputValue.X;
            Matrix[] Z, A;
            ForwardPropagation(out Z, out A, ExampleCount, InputValue);

            return A[A.Length - 1].Slice(0, 1, A[A.Length - 1].X, A[A.Length - 1].Y);
        }

        void ForwardPropagation(out Matrix[] Z, out Matrix[] A, int ExampleCount,Matrix InputValue)
        {
            Z = new Matrix[LayerCount];
            A = new Matrix[LayerCount];

            Z[0] = InputValue.AddColumn(Matrix.Ones(ExampleCount, 1));
            A[0] = Z[0];

            for (int i = 1; i < LayerCount; i++)
            {
                Z[i] = (A[i - 1] * W[i - 1]).AddColumn(Matrix.Ones(ExampleCount, 1));
                A[i] = Activation(Z[i]);
            }
            A[A.Length - 1] = Z[Z.Length - 1];
        }
        void BackPropagation(out Matrix[] delta, out Matrix[] error, Matrix output, Matrix OutputValue,
                                    Matrix Zlast, Matrix[] Z, Matrix[] A)
        {
            error = new Matrix[LayerCount];
            delta = new Matrix[LayerCount];

            error[LayerCount - 1] = output - OutputValue;
            delta[LayerCount - 1] = error[LayerCount - 1];// * Activation(Zlast, true);

            for (int i = LayerCount - 2; i >= 0; i--)
            {
                error[i] = delta[i + 1] * W[i].T;
                delta[i] = error[i] * Activation(Z[i], true);
                delta[i] = delta[i].Slice(0, 1, delta[i].X, delta[i].Y);
            }
        }
        void GradientDescend(Matrix[] A,Matrix[] delta, double LearningRate)
        {
            for (int i = 0; i < W.Length; i++)
            {
                W[i] -= (A[i].T * delta[i + 1]) * LearningRate;
            }
        }

        Matrix Activation(Matrix m, bool derivated = false)
        {
            if (a == ActivationFunction.ReLU)
            {
                return Relu(m, derivated);
            }
            else if (a == ActivationFunction.Sigmoid)
            {
                return Sigmoid(m, derivated);
            }
            else
            {
                return null;
            }
        }

        Matrix Sigmoid(Matrix m, bool derivated = false)
        {
            double[,] output = m;
            Matrix.MatrixLoop((i, j) =>
            {
                if (derivated)
                {
                    double aux = 1 / (1 + Math.Exp(-output[i, j]));
                    output[i, j] = aux * (1 - aux);
                }
                else
                {
                    output[i, j] = 1 / (1 + Math.Exp(-output[i, j]));
                }

            }, m.X, m.Y);
            return output;
        }
        Matrix Relu(Matrix m, bool derivated = false)
        {
            double[,] output = m;
            Matrix.MatrixLoop((i, j) =>
            {
                if (derivated)
                {
                    output[i, j] = output[i, j] > 0 ? 1 : 0.00001;
                }
                else
                {
                    output[i, j] = output[i, j] > 0 ? output[i, j] : 0;
                }

            }, m.X, m.Y);
            return output;
        }

        //Helper

        public static Matrix[] ShuffleMultiplesRows(Matrix[] m, Random r)
        {
            Matrix[] temp = new Matrix[m.Length];
            for (int i = 0; i < m.Length; i++)
            {
                temp[i] = m[i].ToMatrix;
            }

            var indexes = Matrix.RandomIndexes(m[0].X, r);

            for (int i = 0; i < indexes.Length; i++)
            {
                for (int k = 0; k < temp.Length; k++)
                {
                    for (int j = 0; j < m[k].Y; j++)
                    {
                        temp[k][i, j] = m[k][indexes[i], j];
                    }                   
                }                
            }
            return temp;
        }

    }
}