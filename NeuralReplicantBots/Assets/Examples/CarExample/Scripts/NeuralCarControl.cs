using UnityStandardAssets.Vehicles.Car;
using UnityEngine;

namespace NeuralReplicantBot.Examples.CarExample
{
    using NeuralReplicantBot.PerceptronHandler;
    public class NeuralCarControl : BotHandler
    {
        public Transform eyes;
        public int rayCount;
        public int angles;
        public float maxDistance;

        CarMonitor cm;
        CarUserControl cuc;
        Rigidbody rb;
        CarController cc;
        protected override void Awake()
        {
            base.Awake();

            cm = GetComponent<CarMonitor>();
            cuc = GetComponent<CarUserControl>();
            rb = GetComponent<Rigidbody>();
            cc = GetComponent<CarController>();

            cm.enabled = isTraining;
            cuc.enabled = isTraining;

            d = new float[rayCount + 2];
        }

        private void Update()
        {
            if(isTraining)
                return;
            var m = GetSensors();
            var input = new double[1, m.Length];

            for (int i = 0; i < m.Length; i++)            
                input[0, i] = m[i];

            var outputs = brain.GetOutput(input);
            var v = (float)outputs[0, 0];
            var h = (float)outputs[0, 1];
            cc.Move(h,v,v,0);
        }
        float[] d;
        Ray r;
        RaycastHit rh;
        float angle;
        public float[] GetSensors()
        {
            for (int i = 0; i < rayCount; i++)
            {
                angle = angles * (i - rayCount / 2.0f) / rayCount;
                angle += transform.eulerAngles.y;
                angle *= Mathf.Deg2Rad;
                r = new Ray(eyes.position, new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)));
                Physics.Raycast(r, out rh, maxDistance);
                if (rh.collider != null)
                    d[i] = rh.distance / maxDistance;                
                else                
                    d[i] = 1;                

                Debug.DrawRay(r.origin, r.direction * d[i] * maxDistance, Color.green);
            }
            d[rayCount + 0] = cc.CurrentSpeed / cc.MaxSpeed;
            d[rayCount + 1] = rb.angularVelocity.magnitude / rb.maxAngularVelocity;
            return d;
        }
    }
}