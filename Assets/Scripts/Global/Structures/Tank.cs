using UnityEngine;

public class Tank : MonoBehaviour
{
    [SerializeField] private int _firstRangeNumber = 4;
    [SerializeField] private int _secondRangeNumber = 8;
    [SerializeField] private Sprite _destroiedTank;

    private bool hasBeenDestroyed;

    public void DestroyTank()
    {
        if (!hasBeenDestroyed)
        {
            TimeHealthAdder healthAdder = Resources.Load<TimeHealthAdder>("HealthAdder");

            int rendomNumber = Random.Range(4, 8);

            for (int i = 0; i < rendomNumber - 1; i++)
            {
                float x = transform.position.x + Random.Range(-1.5f, 1.5f);
                float y = transform.position.y + Random.Range(-1.5f, 1.5f);
                Vector3 pos = new Vector3(x, y, -1);

                Instantiate(healthAdder, pos, Quaternion.identity);
            }

            gameObject.GetComponent<SpriteRenderer>().sprite = _destroiedTank;

            hasBeenDestroyed = true;
        }
    }

    public bool HasAlreadyBeenDestroyed()
    {
        return hasBeenDestroyed;
    }
}
