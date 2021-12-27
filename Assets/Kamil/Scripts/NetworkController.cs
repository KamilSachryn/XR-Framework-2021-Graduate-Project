using UnityEngine;
using MLAPI;
using MLAPI.Transports.UNET;
using UnityEngine.UI;
using MLAPI.SceneManagement;


public class NetworkController : NetworkBehaviour
{
    public Canvas canvas;
    public InputField fld_ip;
    public InputField fld_pswd;
    public Text txt_badPassword;
    public GameObject model2;

    public string ipAddr = "192.168.1.2";
    public string defaultScene = "Scene 1";
    public string defaultPassword = "Password";
    string password = "";
    
    

    UNetTransport transport;

    public void Start()
    {
        //put default ip into textbox
        fld_ip.text = ipAddr;
    }

    public void Host()
    {
        //remove join canvas
        canvas.gameObject.SetActive(false);
        //set password and client model
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        //Start host at random position in scene
        NetworkManager.Singleton.StartHost(RandomVector3(), Quaternion.identity);
        //change the scene from menu to the 1st game scene
        NetworkSceneManager.SwitchScene(defaultScene);
        
    }

    private void ApprovalCheck(byte[] connection, ulong id, MLAPI.NetworkManager.ConnectionApprovedDelegate callback)
    {

        //set password
        bool approve = System.Text.Encoding.ASCII.GetString(connection) == fld_pswd.text;
        //set model and position, then connect
        callback(true, model2.GetComponent<NetworkObject>().PrefabHash, approve, RandomVector3(), Quaternion.identity);
        
    }

    public void Join()
    {
        //set ip we want to connect to via text box
        ipAddr = fld_ip.text;
        //create transport with gievn IP
        transport = NetworkManager.Singleton.GetComponent<UNetTransport>();
        transport.ConnectAddress = ipAddr;

        //set password
        password = fld_pswd.text;
        NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes(password);

        //attempt to connect
        NetworkManager.Singleton.StartClient();

        //if ip and pass were good, we connect
        if(NetworkManager.Singleton.StartClient().Success)
        {

            canvas.gameObject.SetActive(false);
            txt_badPassword.enabled = false;
        }
        //password or ip were bad
        else
        {
            txt_badPassword.enabled = true;
        }



    }

    public void ReJoin()
    {
        //easy scene change via rejoining the game at desired scene
        //turns out this is how alot of games do it
        transport = NetworkManager.Singleton.GetComponent<UNetTransport>();
        transport.ConnectAddress = ipAddr;

        NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes(password);
        NetworkManager.Singleton.StartClient();
    }

    //random rector within 4 squares of origin
    Vector3 RandomVector3()
    {
        float x = Random.Range(-4f, 4);
        float y = 1;
        float z = Random.Range(-4f, 4);

        return new Vector3(x, y, z);
        
    }



}
