using UnityEngine;

namespace NeuralReplicantBot.Examples.CarExample
{
    using NeuralReplicantBot.PerceptronHandler;
    using UnityStandardAssets.Vehicles.Car;

    public class CarMedition : HumanMonitor
    {
        Brain brain;
        CarUserControl cuc;
        NeuralCarControl ncc;
        Rigidbody rb;

        Vector3 startPoint;
        Quaternion startRotation;

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
            m.inputs.AddRange(ncc.GetSensors());

            float[] o = new float[] { Input.GetAxis("Vertical"), Input.GetAxis("Horizontal") };
            m.outputs.AddRange(o);
        }

        protected override void MeditionEnd(Medition m)
        {
            var medcount = m.outputs.Count / 2;


            LinearAlgebra.Matrix input = new double[medcount, ncc.rayCount + 3];
            LinearAlgebra.Matrix output = new double[medcount, 2];

            var k = 0;
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

            cuc.enabled = false;

            Debug.Log("Please wait, training...");

            brain.Learn(input, output);

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.position = startPoint;
            transform.rotation = startRotation;

            ncc.enabled = true;
            Debug.Log("Thanks for wait...");

        }
    }
}