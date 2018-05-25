using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
  class RockController : NetworkBehaviour
  {
    public void Push(Vector3 orig)
    {
      var force = (transform.position - orig).normalized;
      transform.position = transform.position + new Vector3(force.x, force.y, 0);
    }
  }
}
