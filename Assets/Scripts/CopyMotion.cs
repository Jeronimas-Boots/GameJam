using UnityEngine;

public class CopyMotion : MonoBehaviour
{
    public Transform targetLimb;

    ConfigurableJoint cj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cj = GetComponent<ConfigurableJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cj != null)
        {
            cj.targetRotation = Quaternion.Inverse(targetLimb.localRotation);
        }
    }
}
