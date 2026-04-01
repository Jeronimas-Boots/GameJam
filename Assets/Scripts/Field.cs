using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] private Material _blueColorMaterial;
    [SerializeField] private Material _redColorMaterial;
    [SerializeField] private Material _whiteLineMaterial;
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
                    if (y == 0 && ShouldColorWhite(x, z)) // If ground level and should be white, color white
                        cubeRenderer.material = _whiteLineMaterial;
                    else if (y == 0 && ShouldColorBlue(x, z))
                        cubeRenderer.material = _blueColorMaterial;
                    else if (y == 0 && ShouldColorRed(x, z))
                        cubeRenderer.material = _redColorMaterial;
                    else if (y == 0)
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
    private bool ShouldColorBlue(float x, float z)
    {
        /// Goal left
        if (x >= 2 && x <= 3)
        {
            if (z >= 11 && z <= 18) return true;
        }
        if (x >= 28 && x <= 31)
        {
            if (z >= 12 && z <= 17) return true;
        }
        return false;
    }
    private bool ShouldColorRed(float x, float z)
    {

        /// Goal right
        if (x >= 58 && x <= 59)
        {
            if (z >= 11 && z <= 18) return true;
        }
        if (x >= 32 && x <= 34)
        {
            if (z >= 12 && z <= 17) return true;
        }

        return false;
    }
    private bool ShouldColorWhite(float x, float z)
    {
        /// Outer lines
        if (x >= 4 && x <= 57)
        {
            if (z == 0 || z == 29) return true; // Outer lines
        }

        if (x == 31) return true; // Middle center line

        /// Goal left
        if (x == 4) return true; // Left goal line
        if (x == 1)
        {
            if (z >= 10 && z <= 19) return true; // left side ring
        }
        if (z == 10 || z == 19)
        {
            if (x >= 1 && x <= 4) return true; // top bottom side ring
        }
        if (z == 8 || z == 21)
        {
            if (x >= 4 && x <= 9)  return true; // inner ring top bottom
        }
        if (x == 9)
        {
            if (z >= 8 && z <= 21) return true; // inner ring right
        }

        /// Goal right
        if (x == 57) return true; // Right goal line
        if (x == 60)
        {
            if (z >= 10 && z <= 19) return true; // goal right side, right side ring
        }
        if (z == 10 || z == 19)
        {
            if (x >= 57 && x <= 60) return true; // goal right side, top bottom side ring
        }
        if (z == 8 || z == 21)
        {
            if (x >= 52 && x <= 57)  return true; // inner ring top bottom
        }
        if (x == 52)
        {
            if (z >= 8 && z <= 21) return true; // inner ring right
        }


        /// Inner circle
        /// x 27 row  z13 - 16
        /// x 35 row  z13 - 16
        if (x == 27 || x == 35)
        {
            if (z >= 13 && z <= 16) return true;
        }
        /// z 11 row  x29 - 33
        /// z 18 row  x29 - 33
        if (z == 11 || z == 18)
        {
            if (x >= 29 && x <= 33) return true;
        }
        /// x 28 z 17
        /// x 28 z 12
        /// x 34 z 17
        /// x 34 z 12
        if (x == 28 || x == 34)
        {
            if (z == 12 || z == 17) return true;
        }
        return false;
    }
    public bool Explode(Vector3 explosionPosition, float range)
    {
        bool hasExplodedSomething = false;
        foreach (var obj in _field)
        {
            var objPos = obj.transform.position;
            objPos.y *= 2;
            if ((objPos - explosionPosition).magnitude <= range)
            {
                obj.SetActive(false);
                hasExplodedSomething = true;
            }   
        }
        return hasExplodedSomething;
    }
}
