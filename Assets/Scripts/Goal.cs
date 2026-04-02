using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private GameManager _displayScores;
    [SerializeField] private int _team;
    public GameObject ball;
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
            ball.GetComponent<MeshRenderer>().enabled = false;
            ball.transform.position = new Vector3(0, -90, 0);
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
                ball.GetComponent<MeshRenderer>().enabled = true;
                ball.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                ball.transform.position = new Vector3(0, 3, 0);
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
