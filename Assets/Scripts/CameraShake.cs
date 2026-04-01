using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private float _shake;
    private float _shakeAmount;
    private Camera _camera;
    private float _shakeReductionMultiplier;

    // Update is called once per frame
    void Update()
    {
        if (_shake > 0)
        {
            _shake -= Time.deltaTime * _shakeReductionMultiplier;
        }
        else
        {
            _shake = 0;
            transform.position = Vector3.zero;

        }
    }
}
