using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private DisplayScores _displayScores;
    [SerializeField] private int _team;
    [SerializeField] private GameObject _ball;
    [SerializeField] private AudioClip _goalSound;
    [SerializeField] private CheererManager _cheererManager;

    private bool _scored = false;
    private float _time = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ball" && _scored == false)
        {
            if(_team == 2)
            {
                _displayScores.Player1Scored();
            }
            else if(_team == 1)
            {
                _displayScores.Player2Scored();
            }

            SoundFXManager.Instance.PlaySoundFXClip(_goalSound, transform, 1f);
            _ball.GetComponent<MeshRenderer>().enabled = false;
            _cheererManager.GoalOccurred();
            _scored = true;
        }
    }

    private void Update()
    {
        if(_scored)
        {
            if(_time >= 3f)
            {
                _ball.GetComponent<MeshRenderer>().enabled = true;
                _ball.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                _ball.transform.position = new Vector3(0, 3, 0);
                _scored = false;
                _time = 0f;
            }
            else
            {
                _time += Time.deltaTime;
            }
        }
    }
}
