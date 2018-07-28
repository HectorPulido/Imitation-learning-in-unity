# IMITATION LEARNING IN UNITY
What if we record all the information in a game, and we record the player actions, then we train a neural network with that data? Thats what this is! <b>This is a implementation of [Vectorized neural network](https://github.com/HectorPulido/Vectorized-multilayer-neural-network) </b>in unity, an open source project that uses neural networks and backpropagation in C#, and train it via stochastic gradient descend using as examples human meditions  <br/>

### This project is still under development and is highly experimental

## HOW IT WORKS
[![Banner](http://img.youtube.com/vi/nwqnGh2FiUo/0.jpg)](https://www.youtube.com/watch?v=nwqnGh2FiUo) <br/>

Artificial neural networks are a group of algorithm that can imitate almost any existing function, just like an universal stimator, it's a hierarchical matrix multiplication function that can imitate even the human behaviour.

With the human monitor script, the keys that human presses and the sensors of the bot are keeped to be used as a dataset for the Neural net script and it's signals are interpreted by the brain to make a human - like behaviour 

## TO DO
- More examples

## WHY (MOTIVATION)
[![Banner](http://img.youtube.com/vi/HRYYxJd9qiA/0.jpg)](https://www.youtube.com/watch?v=HRYYxJd9qiA) <br/>
This tutorial was made for <b>Hector Pulido</b> for his youtube channel <br/>
https://www.youtube.com/c/HectorAndresPulidoPalmar <br/>
And his Twitch Channel<br/>
https://www.twitch.tv/hector_pulido_<br/>

## HOW TO USE
Open it on unity 2018 or greater (sorry about that >-< ), and play around with the project.

### How make it works
The asset contais 2 important part, the first one is the <b>neural network part</b>, this piece is inside the brain component, what usually is used in a <b>BotHandler</b>, that component manage the training process and the prediction process, the other important part is the <b>human monitor</b>, this script must be inherited to save the correct meditions, both brain and human monitor contains tools to save and deploy the data. The basic workflow is to use an bot handler that manage a brain, and a human monitor that train the brain

### BOT HANDLER
The neural network is the base of this project, it's imposible to the bot to learn without this, the brain contais the neural network and the hyperparameters
```csharp

using NeuralReplicantBot.PerceptronHandler;
using LinearAlgebra;
public class LogicDoor : BotHandler //<- inherit from BotHandler
{
	protected override void Awake() //<- you can override the Awake
	{
		base.Awake(); // <- but remember to call the base Awake
	
		Foo();		
	}
	
	void Foo()
	{
		if(isTraining) // Training variable is pretty useful to deploy
		{
			brain.Learn(x, y); //<-- You can train, but also you can do it in the HumanMonitor.
		}
	}
	
	void GetOutput()
	{
		print(brain.GetOutput(x));
	}
}

```
### HUMAN MONITOR 
The bot is imitating the human, for this is important to register all important actions from the player before the training the class that do that is the HumanMonitor.
```csharp
using NeuralReplicantBot.PerceptronHandler;

public class ExampleMonitor : HumanMonitor //<- its super important to inherit from HumanMonitor
{
	protected override void Medition(ref Medition m)
	{          
		//It's important to add information to the medition object m
		//This part of the code is runned many times 
		
		m.outputs.AddRange(y); // Adding y array to the output count
		m.inputs.AddRange(x); // Adding x array to the output count
	}

	protected override void MeditionEnd(Medition m)
	{
		// This part of the code is runned once, when the medition count is over
		// so is important to decide what to do with the information 
		// eg. train the neural network
		
		brain.Learn(input, output);      //<- this is a good place to train the brain      
		
		Foo(); //<- you can set BotHandler's property isTraining as false
	}
}
```

## EXAMPLES

### LOGIC DOOR 
This is a super simple script to understand how the brain works, the neural network learns how to make some logic doors, this example does not contains a human monitor

### SELF DRIVING CAR
![Example](/Images/ExampleImage.gif) <br/>
At this moment there is only one complex example in the project, a self driving car with a lot of raycast as inputs and Horizontal and Vertical axis as output

## OTHER WORKS 
### Evolutionary Neural Networks on Unity For bots
This is a asset that train a neural networks using genetic algorithm in unity to make a bot that can play a game or just interact with the envoriment <br/>
https://github.com/HectorPulido/Evolutionary-Neural-Networks-on-unity-for-bots
### More Genetic algorithms on Unity
Those are three Genetics Algorithm using unity, The First one is a simple algorithm that Looks for the minimun of a function, The Second one is a solution for the Travelling Salesman Problem, The Third one is a Automata machine <br/>
https://github.com/HectorPulido/Three-Genetics-Algorithm-Using-Unity
### Vectorized Multilayer Neural Network from scratch
This is a simple MultiLayer perceptron made with Simple Linear Algebra for C# , is a neural network based on This Algorithm but generalized. This neural network can calcule logic doors like Xor Xnor And Or via Stochastic gradient descent backpropagation with Sigmoid as Activation function, but can be used to more complex problems. <br/>
https://github.com/HectorPulido/Vectorized-multilayer-neural-network

## LICENCE
This project contains a copy of:
* Unity Standar assets
* Simple linear algebra https://github.com/HectorPulido/Simple_Linear_Algebra

Everything else is MIT licensed

## Patreon
Please consider Support on Patreon<br/>
![Please consider support on patreon](/Images/Patreon.png)<br/>
https://www.patreon.com/HectorPulido
