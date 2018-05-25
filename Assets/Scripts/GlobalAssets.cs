using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
  public enum PrefabType
  {
    Player,
    Rock,
  }

  public class GlobalAssets : MonoBehaviour
  {
    public static GlobalAssets Instance { get; private set; }

    public GameObject Player;
    public GameObject Rock;

    private void Awake()
    {
      Instance = this;
    }
  }
}
