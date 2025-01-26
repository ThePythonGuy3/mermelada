using UnityEngine;
using System;

public class TimeHealthAdder: MonoBehaviour
{
    // If set true the health to add will be added to the maxTimeHealth.
    public bool isMaxTimeHealth;

    public float timeHealthToAdd;

    [SerializeField] private bool destroy = true;

    public Action onDestroy;

    public void DestroyHealthAdder()
    {
        if (destroy)
            Destroy(this.gameObject);
        else
            onDestroy.Invoke();
    }
}
