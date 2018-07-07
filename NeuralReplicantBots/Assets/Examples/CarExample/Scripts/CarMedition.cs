using UnityEngine;

namespace NeuralReplicantBot.Examples.CarExample
{
    using NeuralReplicantBot.PerceptronHandler;
    using UnityStandardAssets.Vehicles.Car;

    public class CarMedition : HumanMonitor
    {
        public bool ready = false;

        Brain brain;
        CarUserControl userControl;
        NeuralCarControl neuralControl;

        Vector3 startPoint;
        Quaternion startRotation;

        private void Awake()
        {
            brain = GetComponent<Brain>();
            userControl = GetComponent<CarUserControl>();
            neuralControl = GetComponent<NeuralCarControl>();

            userControl.enabled = true;

            startPoint = transform.position;
            startRotation = transform.rotation;

        }

        protected override void Medition(ref Medition m)
        {
            m.inputs.AddRange(neuralControl.GetSensors());
            
            float[] o = new float[] { Input.GetAxis("Vertical"), Input.GetAxis("Horizontal") };
            m.outputs.AddRange(o);
        }
        
        protected override void MeditionEnd(Medition m)
        {
            LinearAlgebra.Matrix input = new double[m.inputs.Count / neuralControl.rayCount, neuralControl.rayCount];
            LinearAlgebra.Matrix output = new double[m.outputs.Count / 2, 2];

            var k = 0;
            for (int i = 0; i < input.x; i++)
            {
                for (int j = 0; j < input.y; j++)
                {
                    input[i, j] = m.inputs[k];
                    k++;
                }
            }

            k = 0;
            for (int i = 0; i < output.x; i++)
            {
                for (int j = 0; j < output.y; j++)
                {
                    output[i, j] = m.outputs[k];
                    k++;
                }
            }

            userControl.enabled = false;
            brain.Learn(input, output   );
                        
            transform.position = startPoint;
            transform.rotation = startRotation;

            ready = true;

        }
    }
}