using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

namespace Assets.Scripts
{
  public enum NetMode
  {
    Host,
    Client,
    Server
  }

  public class NetworkController : NetworkManager
  {

    public static NetMode NetMode = NetMode.Host;

    private GameObject _rock;

    // Use this for initialization
    void Start ()
    {
      switch (NetMode)
      {
        case NetMode.Host:
          StartHost();
          break;
        case NetMode.Client:
          StartClient();
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    // Update is called once per frame
    void Update () {
		
    }

    public override void OnServerSceneChanged(string sceneName)
    {
      base.OnServerSceneChanged(sceneName);

      _rock = Instantiate(GlobalAssets.Instance.Rock);
      NetworkServer.Spawn(_rock);
    }




    #region misc callbacks

    public override void OnServerError(NetworkConnection conn, int errorCode)
    {
      Debug.Log("OnServerError: " + (NetworkError) errorCode);
    }

    public override void OnStartHost()
    {
      Debug.Log("OnStartHost");
    }

    public override void OnStartServer()
    {
      Debug.Log("OnStartServer");
    }

    public override void OnStopServer()
    {
      Debug.Log("OnStopServer");
    }

    public override void OnStopHost()
    {
      Debug.Log("OnStopHost");
    }

    // Client callbacks

    public override void OnClientConnect(NetworkConnection conn)
    {
      base.OnClientConnect(conn);
      Debug.Log("OnClientConnect");
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
      StopClient();
      if (conn.lastError != NetworkError.Ok)
      {
        if (LogFilter.logError)
        {
          Debug.LogError("ClientDisconnected due to error: " + conn.lastError);
        }
      }
      Debug.Log("Client disconnected from server: " + conn);
    }

    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
      Debug.Log("OnClientError: " + (NetworkError) errorCode);
    }

    public override void OnStartClient(NetworkClient client)
    {
      Debug.Log("OnStartClient");
    }

    public override void OnStopClient()
    {
      Debug.Log("OnStopClient");
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
      base.OnClientSceneChanged(conn);
      Debug.Log("OnClientSceneChanged");
    }

    #endregion

  }
}
