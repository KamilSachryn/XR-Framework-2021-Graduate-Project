using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MLAPI.SceneManagement;
using MLAPI;


using MLAPI.Spawning;
using MLAPI.Transports.UNET;
using UnityEngine.UI;
using MLAPI.Configuration;
using MLAPI.Messaging;


public class changeScene : NetworkBehaviour
{
   
    string nextScene;

    void Start()
    {
        //very simple example of switching between scenes

        if(SceneManager.GetActiveScene().name == "Scene 2")
        {
            nextScene = "Scene 1";
        }
        else
        {
            nextScene = "Scene 2";
        }

    }

    void Update()
    {
        
    }

    //when we enter a door, and we are tagged as a player, change the scene for everyone
    private void OnTriggerEnter(Collider other)
    {
        //if is player
        if (other.tag == "Player")
        {
            //if is host
            if (IsHost)
            {

                NetworkSceneManager.SwitchScene(nextScene);

            }
            //if we are not host this is done via a RPC in FristPersonController

        }

    }
   
}
