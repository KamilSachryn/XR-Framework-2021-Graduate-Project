



  <h3 align="center">VR Research - Networking</h3>

  <p align="center">
    2021 VR Summer Project 
    
<!-- TABLE OF CONTENTS -->
<details open="open">
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#Prerequisites for development">Development Prerequisites</a></li>
        <li><a href="#Prerequisites for usage">Usage Prerequisites</a></li>
      </ul>
    </li>
    <li>
	    <a href="#usage">Usage</a>
	 <ul>
        <li><a href="#Example Video">Example Video</a></li>
        <li><a href="#Editor View">Editor View</a></li>
        <li><a href="#Network settings">Network settings</a></li>
         <li><a href="#In-Game">In-Game</a></li>
      </ul>
    </li>
     <li>
	    <a href="#Benefits Of The Project">Benefits Of The Project</a>
	 <ul>
        <li><a href="#Scenes">Scenes</a></li>
        <li><a href="#Integration with other members work">Integration with other members work</a></li>
        <li><a href="#Animations">Animations</a></li>
        <li><a href="#Multiplayer Benefits">Multiplayer Benefits</a></li>
         <li><a href="#Creating New Scenes">Creating New Scenes</a></li>
         <li><a href="#Custom Player Characters">Custom Player Characters</a></li>
         <li><a href="#Client Security">Client Security</a></li>
      </ul>
    </li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>
 


<!-- ABOUT THE PROJECT -->
## About The Project

An easy to use solution to implementing networked multiplayer to the research project as a whole. By using MLAPI from Unity we are able to add multiplayer functionality to user designed environments, characters, and many other things. 

Benefits:
* Increased user engagement.
* Quick and seamless to use.
* Allows for collaboration and virtual meetings. 


### Built With

* [Unity3D](https://unity.com/)



<!-- GETTING STARTED -->
## Getting Started



### Prerequisites for development

* Unity 2021.1.13
  ```
  https://unity3d.com/get-unity/download
  ```
* Any IDE that supports C#

### Prerequisites for usage
* Requirements are limited by Unity game engine requirements, running on both Windows (7, 8, 10) and Mac OS X (10.9+). More information about Unity requirements can be found at: [https://unity3d.com/unity/system-requirements](https://unity3d.com/unity/system-requirements).

<!-- USAGE EXAMPLES -->
## Usage

### Example Video

[![Demo Video](https://i.imgur.com/9JTDc1t.png)](https://www.youtube.com/watch?v=nxV61dMYrmI)

By using simple Network transforms and a custom modified Character Controller we are able to have two or more players moving together in a single instance. The character controller has been specially crafted to fit as many models as possible to be easily interchangeable, with spaces for animations being an easy addition once created. Currently the model does not support any physics other than Collision and Gravity, though that can be changed easily in the future, once other features are added. 

### Editor View 

![](https://i.imgur.com/AA6hz63.png?raw=true)

The current scene is very simple, comprised of a blank ground as well as three obstacles. This is done on purpose to show that we can easily modify the playing field, and will suit our needs in the future to add multiplayer interactable objects. 

### Network settings

![](https://i.imgur.com/0dqJdsl.png?raw=true)

This shows the default MLAPI connection controller, giving us a lot of control over what each player can connect as or to. 
### In-Game
![](https://i.imgur.com/mxhTfpS.png?raw=true)

Adding multiplayer functionality to user-created objects is a simple matter of adding specific components onto each prefab. In this demonstration two different asset packs from the Unity Store were used and edited slightly to include multiplayer functionality. With this we are able to move and interact with objects on both the host player and each guest. The player characters use a specialized Controller script which shares information between clients while doing cleanup across each client to make sure that players cant force other clients to do actions. 

## Benefits Of The Project 
### Scenes

Scene changes require all of the clients to synchronize objects across the scene, causing several issues with loading into a new scene as the guest player. To circumvent this it is possible to de-sync prefab objects and then load them from the guest itself, rather than causing issues when the host and guest fail to synchronize. This is easily done via the MLAPI settings and causes simple scene switches via the host to be possible. 

### Integration with other members work

Remote Objects can be easily downloaded and be made compatible simply by changing their tag and attaching a Network component and any network oriented script via the editor or in-game via script. These objects can even be downloaded during runtime and given a simple script which attaches objects to it can be made seamless to use.  Further more, animations sync using a similar Network Animation script which lets us use any animation made with little issues, so long as it is correctly code. 

### Animations

Animations are controlled via a centralized animation controller so that every model functions the same way. Each action taken by a client will be replicated on every other client through the host. This animation controller can be scaled up to use the hundreds of animations available on the Mixamo website, and be easily edited to support all sorts of custom models. Once the animation controller is set up for a model MLAPI is able to seamlessly use it to synchronize animations without any further changes in the code or the stock animation. 

### Multiplayer Benefits

We can use multiplayer to show different VR interactions between users. Each player is able to synchronize their own additions to the application and share them with other connected users. This can take the form of downloaded assets or simply moving and creating specific objects around the game world. This intractability adds a large number of potential uses to the project, allowing it to not only be shared with other people but have them actively interact and learn with each other. This can be further extended to teacher and student interactions, with one user being in control of all the others and allow them to change scenes or any other kind of benefit. 

### Creating New Scenes

There are two ways to create new scenes. They can be made traditionally via the unity editor, which is the simplest way where we only need to add the relevant classes and objects to pre-made assets or create our own. The second method is to have a number of assets available for use or be either downloaded or uploaded dynamically by the user, and have them add them to an existing scene or a new one, allowing for creativity and cooperation if multiple users attempt to make a scene at the same time. 

### Custom Player Characters

Different clients are able to use unique character models to differentiate themselves from other players. These models are taken directly from Mixamo and require minimal changes to function. Once loaded in, each client can pick from the list of loaded models and be both synchronized and playable with standard Mixamo animations. 

![](https://i.imgur.com/QdA77WB.png?raw=true)

### Client Security

A password system has been implemented via sending an encrypted password along with connection data. When a client chooses which IP to connect to they also supply a password which is then sent alongside the connection request. The host then checks this against what the server creator set, and decides whether or not the connection should be allowed. 

![](https://i.imgur.com/ekTOck0.png?raw=true)
<!-- ROADMAP -->
## Roadmap


* Completed Features
	* Basic Networking
	*  Synchronized Multiplayer movement
	* Intractability between players
	* Hosting over Internet 
	* Multiple scenes
	* Menu Scene
	* Interactable objects
	* On-demand spawning of game objects
	* Extend all features to be seamless
	* Allow for player created characters and environments to be instantly networked
	* Client Security