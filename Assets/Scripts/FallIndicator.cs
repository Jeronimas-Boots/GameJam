using UnityEngine;

public class FallIndicator : MonoBehaviour
{
    public Material _fallIndicatorMaterial;
    public GameObject _fallIndicator;
    private float _maxDistance = 100f;
    public LayerMask groundLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _fallIndicator = Instantiate(_fallIndicator, null);
        _fallIndicator.GetComponent<Renderer>().material = _fallIndicatorMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _maxDistance, groundLayer))
        {
            if (hit.distance > 1f)
                _fallIndicator.transform.position = hit.point + Vector3.up * 0.05f;
            else
                _fallIndicator.transform.position = new Vector3(0, -100, 0);
        }
    }
}
