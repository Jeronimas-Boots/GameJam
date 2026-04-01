using UnityEngine;

public class Cheerie : MonoBehaviour
{
    public float sinUpDownAmount;

    public float upDownAmountMultiplier;
    public float speedMultiplier = 1f;
    private void Start()
    {
        Color color = new Color(Random.Range(0.2f, 0.7f), Random.Range(0.2f, 0.7f), Random.Range(0.2f, 0.7f));
        
        transform.Find("Body").GetComponent<Renderer>().material.color = color;
        transform.Find("Head").GetComponent<Renderer>().material.color = new Color(150f / 255f, 130f / 255f, 122f / 255f);
    }
    // Update is called once per frame
    void Update()
    {
        sinUpDownAmount += Time.deltaTime * speedMultiplier;
        transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Sin(sinUpDownAmount) * upDownAmountMultiplier, transform.localPosition.z);
    }
}
