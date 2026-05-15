using UnityEngine;

public class AutoDestroy : MonoBehaviour
{

    public float delay = 1.5f;

    public void destroyObject()
    {
        Destroy(gameObject, delay);
    }

    private void Start()
    {
        destroyObject();
    }

}
