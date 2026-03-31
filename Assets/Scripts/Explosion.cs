using UnityEngine;

public class Explosion : MonoBehaviour
{
    float timer = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer == -1) return;

        timer += Time.deltaTime;
        if (timer > 1)
        {
            foreach (var obj in FindObjectsByType<Rigidbody>())
            {
                obj.AddExplosionForce(1500, new Vector3(), 10);
            }
            timer = -1;
        }
    }
}
