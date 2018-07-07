using UnityStandardAssets.Vehicles.Car;
using UnityEngine;

namespace NeuralReplicantBot.Examples.CarExample
{
    using NeuralReplicantBot.PerceptronHandler;
    public class NeuralCarControl : MonoBehaviour
    {
        public Transform eyes;
        public int rayCount;
        public int angles;
        public float maxDistance;

        float[] d;
        Ray r;
        RaycastHit rh;

        CarController controller;
        Brain brain;
        CarMedition med;

        private void Awake()
        {
            controller = GetComponent<CarController>();
            med = GetComponent<CarMedition>();
            brain = GetComponent<Brain>();

            d = new float[rayCount];
        }

        private void Update()
        {
            if (!med.ready)
                return;

            LinearAlgebra.Matrix input = new double[1, rayCount];

            var m = GetSensors();

            for (int i = 0; i < m.Length; i++)
            {
                input[0, i] = m[i];
            }


            double[,] outputs = brain.GetOutput(input);


            controller.Move((float)outputs[0, 1],
                            (float)outputs[0, 0],
                           0,//(float)outputs[2] * 2f - 1,//(float)outputs[2] * 2f - 1,
                           0);//(float)outputs[3] * 2f - 1);

        }

        public float[] GetSensors()
        {
            for (int i = 0; i < rayCount; i++)
            {
                float angle = angles * (i - rayCount / 2.0f) / rayCount;
                angle += transform.eulerAngles.y;
                angle *= Mathf.Deg2Rad;

                r = new Ray(eyes.position, new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)));

                Physics.Raycast(r, out rh, maxDistance);

                if (rh.collider != null)
                {
                    d[i] = rh.distance / maxDistance;
                }
                else
                {
                    d[i] = 1;
                }

                Debug.DrawRay(r.origin, r.direction * d[i] * maxDistance, Color.green);
            }

            return d;
        }
    }
}