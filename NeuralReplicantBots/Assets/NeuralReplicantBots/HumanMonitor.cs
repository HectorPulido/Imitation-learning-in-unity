using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
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
        public bool saveData;
        public bool loadData;
        public bool useRelativeDataPath;
        public string dataPath = "";

        float t;
        IEnumerator Start()
        {
            yield return null;
            if (dataPath == "" || dataPath == null)
                dataPath = useRelativeDataPath ? Application.dataPath + "/Data/data.nn" : "c://data.nn";
            else
                dataPath = useRelativeDataPath ? Application.dataPath + dataPath : dataPath;

            var medition = new Medition();

            if (loadData)
            {
                try
                {
                    FileStream fs = new FileStream(dataPath, FileMode.Open);
                    BinaryFormatter formatter = new BinaryFormatter();
                    medition = (Medition)formatter.Deserialize(fs);
                    fs.Close();
                }
                catch (SerializationException e)
                {
                    Debug.LogError(e.Message);
                }

            }
            else
            {                
                medition.Init();
                while (t <= meditionCount)
                {
                    Medition(ref medition);
                    yield return new WaitForSeconds(meditionPeriod);
                    t ++;
                }
                if(saveData)
                {
                    try
                    {
                        FileStream fs = new FileStream(dataPath, FileMode.OpenOrCreate);
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(fs, medition);
                        fs.Close();
                    }
                    catch (SerializationException e)
                    {
                        Debug.LogError(e.Message);
                    }
                }
            }
            Debug.Log("Medition ended");
            MeditionEnd(medition);
        }
        protected abstract void Medition(ref Medition m);
        protected abstract void MeditionEnd(Medition m);
    }
}