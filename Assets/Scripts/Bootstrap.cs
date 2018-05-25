using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.Networking;

public class Bootstrap : MonoBehaviour
{

  public NetMode DefaultMode = NetMode.Host;

	// Use this for initialization
	void Awake () {

    Setup();

	  var netMan = GetComponent<NetworkManager>();
    netMan.enabled = true;
	}

  private void Setup()
  {
    var args = System.Environment.GetCommandLineArgs();
    if (args.Length < 2)
    {
      NetworkController.NetMode = DefaultMode;
      return;
    }

    Debug.Log("start args " + string.Join("\n", args));

    switch (args[1])
    {
//      case "-server":
//	      StartAsServer();
//        break;
      case "-host":
        NetworkController.NetMode = NetMode.Host;
        break;
      case "-client":
        NetworkController.NetMode = NetMode.Client;
        break;
    }
  }

  // Update is called once per frame
	void Update () {
		
	}
}
