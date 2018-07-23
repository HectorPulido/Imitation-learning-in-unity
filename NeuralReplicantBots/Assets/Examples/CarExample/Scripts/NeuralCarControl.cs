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

        Rigidbody rb;
        CarController cc;
        Brain b;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            cc = GetComponent<CarController>();
            b = GetComponent<Brain>();

            d = new float[rayCount + 3];
        }

        private void Update()
        {
            var m = GetSensors();

            LinearAlgebra.Matrix input = new double[1, m.Length];

            for (int i = 0; i < m.Length; i++)
            {
                input[0, i] = m[i];
            }

            double[,] outputs = b.GetOutput(input);

            cc.Move((float)outputs[0, 1],
                    (float)outputs[0, 0],
                    (float)outputs[0, 0], 
                    0);

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

            d[rayCount + 0] = cc.CurrentSpeed / cc.MaxSpeed;
            d[rayCount + 1] = cc.CurrentSteerAngle;
            d[rayCount + 2] = rb.angularVelocity.magnitude / rb.maxAngularVelocity;
            return d;
        }
    }
}