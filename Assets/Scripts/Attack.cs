using UnityEngine;

public class Attack : MonoBehaviour
{
    private Renderer visibility;
    private Controls manager;
    private float speed;

    void Start()
    {
        manager = GameObject.FindWithTag("Player").GetComponent<Controls>();
        visibility = GetComponent<Renderer>();
    }

    void Update()
    {
        if (!manager.paused) {
            speed = manager.projectileSpeed;
            if (visibility.isVisible) transform.position += (Vector3.back * speed * Time.deltaTime);
            else Destroy(gameObject);
        }
    }
}