using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
  public class PlayerController : NetworkBehaviour
  {
    public override void OnStartLocalPlayer()
    {
      var sprite = GetComponent<SpriteRenderer>();
      sprite.color = Color.blue;
    }

    void Start () {
		
    }

    void Update()
    {
      if (!isLocalPlayer)
      {
        return;
      }

      if (Input.GetKeyDown(KeyCode.Space))
      {
        CmdDoSomething(transform.position);
      }

      var x = Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f;
      var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;
      transform.Translate(x, z, 0);
    }

    [Command]
    public void CmdDoSomething(Vector3 orig)
    {
      var rock = FindObjectOfType<RockController>();
      rock.Push(orig);
    }
  }
}
