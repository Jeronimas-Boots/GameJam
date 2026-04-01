using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private float _shake;
    [SerializeField] private float _shakeAmount = 5f;
    [SerializeField] private float _shakeReductionMultiplier = 0.5f;

    // Update is called once per frame
    void Update()
    {
        if (_shake > 0)
        {
            _shake -= Time.deltaTime * _shakeReductionMultiplier * Mathf.Clamp(_shake, 1f, float.MaxValue);
            transform.localPosition = new Vector3(1,1,1) * Random.Range(-_shakeAmount, _shakeAmount) * _shake;
            transform.localEulerAngles = new Vector3(1,1,1) * Random.Range(-_shakeAmount, _shakeAmount) * _shake;
        }
        else
        {
            _shake = 0;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

        }
    }
    public void Shake(float intensity)
    {
        _shake += intensity;
    }
}
