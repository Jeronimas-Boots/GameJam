using System;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    private CameraAnimationStartEnd _cameraAnimationStartEnd;
    private GameObject _startScreenUI;
    private GameObject _ball1;
    private GameObject _ball2;
    private GameObject _movingBall;
    private PathFollower _pathFollower;
    private void Start()
    {
        GameObject.Find("PressAToJoinUI").SetActive(false);
        _startScreenUI = transform.Find("StartScreenUI").gameObject;
        _movingBall = transform.Find("MovingBall").gameObject;
        _ball1 = transform.Find("Ball1").gameObject;
        _ball2 = transform.Find("Ball2").gameObject;
        _ball1.GetComponent<Rigidbody>().useGravity = false;
        _ball2.GetComponent<Rigidbody>().useGravity = false;
        _movingBall.GetComponent<Rigidbody>().useGravity = false;
        _pathFollower = _movingBall.AddComponent<PathFollower>();
        _pathFollower.enabled = false;

        var pathWayObj = transform.Find("Pathway");
        List<GameObject> path = new List<GameObject>();
        for (int i = 0; i < pathWayObj.childCount; i++)
        {
            path.Add(pathWayObj.GetChild(i).gameObject);
        }
        _pathFollower._waypoints = path;
        _pathFollower._speed = 20f;
        _cameraAnimationStartEnd = GameObject.FindAnyObjectByType<CameraAnimationStartEnd>();
    }
    public void ClickStart()
    {
        _ball1.SetActive(false);
        _ball2.SetActive(false);
        _startScreenUI.SetActive(false);
        _pathFollower.enabled = true;
        // Wait 1s
        StartCoroutine(MoveCameraAfterABit());
    }
    IEnumerator<WaitForSeconds> MoveCameraAfterABit()
    {
        yield return new WaitForSeconds(0.5f);
        _cameraAnimationStartEnd.StartMove(false);

    }
}
