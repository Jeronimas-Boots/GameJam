using UnityEngine;

public class Cheerie : MonoBehaviour
{
    public float sinUpDownAmount;

    public float upDownAmountMultiplier;
    public float speedMultiplier = 1f;

    // Update is called once per frame
    void Update()
    {
        sinUpDownAmount += Time.deltaTime * speedMultiplier;
        transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Sin(sinUpDownAmount) * upDownAmountMultiplier, transform.localPosition.z);

    }
}
