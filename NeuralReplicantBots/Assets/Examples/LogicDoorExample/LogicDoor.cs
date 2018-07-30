using UnityEngine;

namespace NeuralReplicantBot.Examples.LogicDoors
{
	using NeuralReplicantBot.PerceptronHandler;
	using LinearAlgebra;
	public class LogicDoor : BotHandler 
	{
		void Start()
		{
			Matrix x = new double[,]{{0,0}, 
									 {0,1}, 
									 {1,0}, 
									 {1,1} };
			Matrix y = new double[,]{{0,0,0,1}, 
									 {1,0,1,0}, 
									 {1,0,1,0}, 
									 {1,1,0,1} };

			if(isTraining)
			{
				brain.Learn(x, y);
			}

			print(brain.GetOutput(x));

		}
	}
}
