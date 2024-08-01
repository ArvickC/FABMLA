# FABMLA
> A **F**ramework for **A**gent-**B**ased Modeling with **M**achine **L**earning **A**gents

FABMLA is a framework that allows for Agent-Based Modeling powered by Machine Learning in [Unity3D](https://unity.com/)

FABMLA creates a real-time, 3D, ABM training environment and offers three key advantages: 

(1), its [Unity Machine Learning Agents Toolkit](https://github.com/Unity-Technologies/ml-agents) (ML-Agents) implementation provides deep learning capabilities. Additionally, the properties of ML-Agents are exposed within the framework, encouraging the creation of complex, environment-reactive, and interactive agents. 

(2), FABMLA supports generalization — its abstract nature obfuscates internal methods, enabling compatibility with any agent behavior or type. 

(3), its integration with Unity3D provides unique features, allowing utilization of in-built Unity features such as 3D rendering, AI pathfinding, and physics engines which are the backbones of 3D simulations. Additionally, those familiar with the Unity3D environment and `C#` should find FABMLA straightforward.

The inner workings of FABMLA are inspired by the architecture of [ABMU](https://github.com/cheliotk/unity_abm_framework); the core concept we introduce is _Steps_, an approach that enables behavior to be repeated throughout an episode, a single iteration of a simulation. For example, an agent could move in a certain direction each _Step_ based on a custom algorithm. 

# Installation and Getting Started
### By Cloning Github
Clone this github repository and open the project in Unity3D
### Or By Cloning Core Components
Navigate to the [latest release](https://github.com/ArvickC/FABMLA/releases). Download and import them into your Unity3D project.
#### Logger (Optional)
Additionally, you can import the [Logger](https://github.com/ArvickC/FABMLA/wiki/Logger) class into your Unity3D project.
## Installing ML-Agents
This framework is dependent on [ML-Agents](https://github.com/Unity-Technologies/ml-agent), follow the installation instructions [here](https://github.com/Unity-Technologies/ml-agents/blob/latest_release/docs/Installation.md).

# Documentation
Detailed documentation of FABMLA can be found on our [Wiki](https://github.com/ArvickC/FABMLA/wiki).

# License
This project is licensed under [MIT License](https://github.com/ArvickC/FABMLA/blob/main/LICENSE).
