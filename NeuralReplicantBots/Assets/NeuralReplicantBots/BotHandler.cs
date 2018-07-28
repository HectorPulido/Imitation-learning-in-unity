using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralReplicantBot.PerceptronHandler
{
	[RequireComponent(typeof(Brain))]
	public class BotHandler : MonoBehaviour 
	{
		public bool isTraining;
		protected Brain brain;

		virtual protected void Awake () 
		{
			brain = GetComponent<Brain>();

			brain.load = !isTraining;
		}

	}
}
