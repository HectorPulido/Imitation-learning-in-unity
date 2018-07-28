using UnityEngine;

namespace NeuralReplicantBot.Examples.CarExample
{
    using NeuralReplicantBot.PerceptronHandler;
    using UnityStandardAssets.Vehicles.Car;
    using LinearAlgebra;

    public class CarMonitor : HumanMonitor
    {
        Brain brain;
        CarUserControl cuc;
        NeuralCarControl ncc;
        Rigidbody rb;

        private void Awake()
        {
            brain = GetComponent<Brain>();
            cuc = GetComponent<CarUserControl>();
            ncc = GetComponent<NeuralCarControl>();
            rb = GetComponent<Rigidbody>();

            startPoint = transform.position;
            startRotation = transform.rotation;
        }

        protected override void Medition(ref Medition m)
        {
            float[] o = { Input.GetAxis("Vertical"), Input.GetAxis("Horizontal") };            
            float[] i = ncc.GetSensors();

            m.outputs.AddRange(o);
            m.inputs.AddRange(i);
        }
        
        Vector3 startPoint;
        Quaternion startRotation;
        protected override void MeditionEnd(Medition m)
        {
            int medcount  = m.outputs.Count / 2;

            Matrix input  = new Matrix(medcount, ncc.rayCount + 2);
            Matrix output = new Matrix(medcount, 2);

            int k = 0;
            for (int i = 0; i < input.X; i++)
            {
                for (int j = 0; j < input.Y; j++)
                {
                    input[i, j] = m.inputs[k];
                    k++;
                }
            }

            k = 0;
            for (int i = 0; i < output.X; i++)
            {
                for (int j = 0; j < output.Y; j++)
                {
                    output[i, j] = m.outputs[k];
                    k++;
                }
            }

            print(input);
            print(output);

            Debug.Log("Please wait, training...");

            cuc.enabled = false; // No more human control
            ncc.isTraining = false; // Bot control

            //Reset everything
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.position = startPoint;
            transform.rotation = startRotation;

            brain.Learn(input, output);
            Debug.Log("Thanks for wait...");
        }
    }
}