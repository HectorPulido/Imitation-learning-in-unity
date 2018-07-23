using UnityEngine;

namespace NeuralReplicantBot.Examples.LogicDoors
{
	using NeuralReplicantBot.PerceptronHandler;
	using LinearAlgebra;

	[RequireComponent(typeof(Brain))]
	public class LogicDoor : MonoBehaviour {

		Brain brain;
		void Start () 
		{
			brain = GetComponent<Brain>();

			Matrix x = new double[,]{{0,0}, 
									 {0,1}, 
									 {1,0}, 
									 {1,1} };
			Matrix y = new double[,]{{0,0,0,-1}, 
									 {1,0,1,0}, 
									 {1,0,1,0}, 
									 {1,1,0,1} };

			brain.Learn(x, y);

			print(brain.GetOutput(x));

		}
	}
}
