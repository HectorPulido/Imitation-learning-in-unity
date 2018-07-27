# IMITATION LEARNING IN UNITY
<b>This is a implementation of [Vectorized neural network](https://github.com/HectorPulido/Vectorized-multilayer-neural-network) </b>in unity, an open source project that uses neural networks and backpropagation in C#, and train it via stochastic gradient descend  <br/>
### This project is still under development and is highly experimental

## HOW IT WORKS
[![Banner](http://img.youtube.com/vi/HRYYxJd9qiA/0.jpg)](https://www.youtube.com/watch?v=HRYYxJd9qiA) <br/>
Artificial neural networks are a group of algorithm that can imitate almost any existing function, just like a universal stimator, it's a hierarchical matrix multiplication function that can imitate even a human behaviour.

With the human monitor script the keys that human presses and the sensors of the bot are keeped to be used as a dataset for the Neural net script and it's signals are interpreted by the brain to make a human - like behaviour 

## TO DO
- More examples

## WHY (MOTIVATION)
This tutorial was made for <b>Hector Pulido</b> for his youtube channel <br/>
https://www.youtube.com/c/HectorAndresPulidoPalmar <br/>
And his Twitch Channel<br/>
https://www.twitch.tv/hector_pulido_<br/>

## HOW TO USE
Open it on unity 2018 or greater (sorry about that >-< ), and play around with the project.

## Neural network
The neural network is the base of this project, it's imposible to the bot to learn without this, the brain contais the neural network and the hyperparameters
```csharp
using NeuralReplicantBot.PerceptronHandler;

[RequireComponent(typeof(Brain))] //IMPORTANT
public class NeuralNetworkTest : MendelMachine
{
  Brain brain;

	//Init all variables
	protected override void Start()
  {
      brain = GetComponent<Brain>(); //You need to get the brain component from the gameobject
      
      //TRAIN
      brain.Learn(input, output); //To train the model, you need 2 matrix (the shape depends of the brain)
      
      //Forward propagation
      var outputs = brain.GetOutput(input); //To get info from the neural network you need to set input
  }	
}
```
## Meditions 
The bot is imitating the human, for this is important to register all important actions from the player before the training the class that do that is the HumanMonitor.
```csharp
using NeuralReplicantBot.PerceptronHandler;

public class ExampleMonitor : HumanMonitor
{
  protected override void Medition(ref Medition m)
  {          
      //It's important to add information to the medition object m
      m.outputs.AddRange(y); // Adding y array to the output count
      m.inputs.AddRange(x); // Adding x array to the output count
  }

  protected override void MeditionEnd(Medition m)
  {
      // here the medition is over, so is important to decide what to do with the information eg. train the neural network
      Debug.Log("Please wait, training...");
      brain.Learn(input, output);            
      //AMAZINGDOSTUFF
      Debug.Log("Thanks for wait...");
  }
}
```

## EXAMPLES

### SELF DRIVING CAR
![Example](/Images/ExampleImage.gif) <br/>
At this moment there is only one example in the project, a self driving car with a lot of raycast as inputs and Horizontal and Vertical axis as output

## OTHER WORKS 
### Evolutionary Neural Networks on Unity For bots
This is a asset that train a neural networks using genetic algorithm in unity to make a bot that can play a game or just interact with the envoriment <br/>
https://github.com/HectorPulido/Evolutionary-Neural-Networks-on-unity-for-bots
### More Genetic algorithms on Unity
Those are three Genetics Algorithm using unity, The First one is a simple algorithm that Looks for the minimun of a function, The Second one is a solution for the Travelling Salesman Problem, The Third one is a Automata machine <br/>
https://github.com/HectorPulido/Three-Genetics-Algorithm-Using-Unity
## Vectorized Multilayer Neural Network from scratch
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
