using UnityEngine;

public class TimeHealthAdder: MonoBehaviour
{
    // If set true the health to add will be added to the maxTimeHealth.
    public bool isMaxTimeHealth;

    public float timeHealthToAdd;

    public void DestroyHealthAdder()
    {
        Destroy(this.gameObject);
    }
}
