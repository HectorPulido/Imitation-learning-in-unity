using UnityEngine;

namespace NeuralReplicantBot.Examples.CarExample
{
    using NeuralReplicantBot.PerceptronHandler;
    using UnityStandardAssets.Vehicles.Car;
    public class NeuralCar : MonoBehaviour
    {
        CarMedition cm;
        NeuralCarControl ncm;
        Brain b;
        CarUserControl cuc;

        public bool training;

        void Awake()
        {
            cm = GetComponent<CarMedition>();
            ncm = GetComponent<NeuralCarControl>();
            b = GetComponent<Brain>();
            cuc = GetComponent<CarUserControl>();

            b.enabled = true;
            cm.enabled = training;
            cuc.enabled = training;
            ncm.enabled = !training;

            b.load = !training;

        }
    }
}
