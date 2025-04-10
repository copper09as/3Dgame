using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SteelPlate : MonoBehaviour
{
    public List<HeavyObject> heavyObjects = new List<HeavyObject>();
    [SerializeField] private float heavy;
    public float Heavy
    {
        get
        {
            return heavy;
        }
    }
    public void AddHeavy(float heavy)
    {
        this.heavy += heavy;
    }

}
