using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralReplicantBot.PerceptronHandler
{
    [System.Serializable]
    public struct Medition
    {
        public List<float> inputs;
        public List<float> outputs;

        public void Init()
        {
            inputs = new List<float>();
            outputs = new List<float>();
        }
    }

    public abstract class HumanMonitor : MonoBehaviour
    {
        public float meditionPeriod = 0.2f;
        public float meditionCount = 30;

        float t;
        IEnumerator Start()
        {
            var medition = new Medition();
            medition.Init();

            while (t <= meditionCount)
            {

                Medition(ref medition);

                yield return new WaitForSeconds(meditionPeriod);
                t ++;
            }
            MeditionEnd(medition);
        }
        protected abstract void Medition(ref Medition m);
        protected abstract void MeditionEnd(Medition m);
    }
}