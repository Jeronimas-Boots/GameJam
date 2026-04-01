using UnityEngine;

public class Cheerie : MonoBehaviour
{
    public float sinUpDownAmount;

    public float upDownAmountMultiplier;
    public float speedMultiplier = 1f;
    private void Start()
    {
        Color color = Random.ColorHSV();
        color.a = 1f;
        transform.Find("Body").GetComponent<Renderer>().material.color = color;
        transform.Find("Head").GetComponent<Renderer>().material.color = new Color(255f / 255f, 219f / 255f, 172f / 255f);
    }
    // Update is called once per frame
    void Update()
    {
        sinUpDownAmount += Time.deltaTime * speedMultiplier;
        transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Sin(sinUpDownAmount) * upDownAmountMultiplier, transform.localPosition.z);
    }
}
