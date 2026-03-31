using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Field : MonoBehaviour
{
    public float timertemp;
    public float maxtimetoregenerate;

    [SerializeField]
    private float _sizeMultiplier = 1f;
    private List<GameObject> _field = new List<GameObject>(); 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ClearField();
        GenerateField();
    }
    private void Update()
    {
        timertemp += Time.deltaTime;
        if (timertemp > maxtimetoregenerate)
        {
            timertemp = 0f;
            //ClearField();
            //GenerateField();
            Explode(Vector3.zero, 1);
        }
    }
    void ClearField()
    {
        foreach (var obj in _field)
        {
            Destroy(obj);
        }
        _field.Clear();
    }
    void GenerateField()
    {
        if (_sizeMultiplier < 0.3f) return;
        for (int z = 0; z < transform.localScale.z * (1/_sizeMultiplier); z++)
        {
            for (int x = 0; x < transform.localScale.x * (1/_sizeMultiplier); x++)
            {
                var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.SetParent(transform);

                // Scale it properly
                cube.transform.localScale = new Vector3(1 / transform.localScale.x, 0.2f, 1 / transform.localScale.z);
                cube.transform.localScale *= _sizeMultiplier;
                cube.GetComponent<Renderer>().material.color = new Color(0.10f, 0.75f, 0.10f);
                cube.transform.localPosition = new Vector3(
                    x * cube.transform.localScale.x - transform.localScale.x / 2 * cube.transform.localScale.x / _sizeMultiplier + cube.transform.localScale.x / 2,
                    0,
                    z * cube.transform.localScale.z - transform.localScale.z / 2 * cube.transform.localScale.z / _sizeMultiplier + cube.transform.localScale.z / 2);

                _field.Add(cube);
            }
        }

    }
    void Explode(Vector3 position, float range)
    {
        foreach (var obj in _field)
        {
            if ((obj.transform.position + position).magnitude <= range)
            {
                obj.SetActive(false);
            }   
        }
    }
}
