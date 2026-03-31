using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] private Material _dirtMaterial;
    [SerializeField] private Material _grassMaterial;
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
    }
    private void ClearField()
    {
        foreach (var obj in _field)
        {
            Destroy(obj);
        }
        _field.Clear();
    }
    public void GenerateField()
    {
        if (_field.Count != 0) ClearField();

        float height = 0.15f;
        if (_sizeMultiplier < 0.3f) return;
        for (int y = 0; y < 5; y++)
        {
            for (int z = 0; z < transform.localScale.z * (1 / _sizeMultiplier); z++)
            {
                for (int x = 0; x < transform.localScale.x * (1 / _sizeMultiplier); x++)
                {
                    var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.SetParent(transform);
                    cube.name = "Cube x" + x + ", y" + y + ", z" + z;
                    // Scale it properly
                    cube.transform.localScale = new Vector3(1 / transform.localScale.x, height, 1 / transform.localScale.z);
                    cube.transform.localScale *= _sizeMultiplier;
                    var cubeRenderer = cube.GetComponent<Renderer>();
                    if (y == 0)
                        cubeRenderer.material = _grassMaterial;
                    else
                        cubeRenderer.material = _dirtMaterial;
                    cubeRenderer.material.mainTextureOffset = new Vector2((float)x/10f, (float)z/10f);

                    cube.transform.localPosition = new Vector3(
                        x * cube.transform.localScale.x - transform.localScale.x / 2 * cube.transform.localScale.x / _sizeMultiplier + cube.transform.localScale.x / 2,
                        -y * (height/2-height/8),
                        z * cube.transform.localScale.z - transform.localScale.z / 2 * cube.transform.localScale.z / _sizeMultiplier + cube.transform.localScale.z / 2);
                    _field.Add(cube);
                }
            }
        }
    }
    public void Explode(Vector3 explosionPosition, float range)
    {
        foreach (var obj in _field)
        {
            var objPos = obj.transform.position;
            objPos.y *= 2;
            if ((objPos - explosionPosition).magnitude <= range)
            {
                obj.SetActive(false);
            }   
        }
    }
}
