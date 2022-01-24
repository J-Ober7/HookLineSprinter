using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// handles the changing color of the sky and the movement of the sun and moon
public class DayNightCycle : MonoBehaviour
{
    // the speed at which the day/night cycle progresses
    public float daySpeed = 0.1f;

    public List<Material> affectedByLight;
    public Color hueColor;


    // for changing the color of the sky
    private Material skymat;
    private Vector2 matTiling;
    private float matstartx;

    void Start()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();

        skymat = mr.material;
        matTiling = skymat.mainTextureScale;
        matstartx = skymat.mainTextureScale.x;
    }

    void Update()
    {
        ChangeSkyColor();
        UpdateMaterialColors();
        MoveSun();
        MoveMoon();

    }

    void MoveSun()
    {

    }

    void MoveMoon()
    {

    }


    void ChangeSkyColor()
    {
        if (matTiling.x < matstartx * 3)
        {
            matTiling.x += daySpeed * Time.deltaTime;
        }
        else
        {
            matTiling.x = matstartx;
        }

        skymat.mainTextureScale = matTiling;
    }

    void UpdateMaterialColors()
    {
        // first we need to get the color that the ambient light is?

        float H, S, B;

        Color.RGBToHSV(hueColor, out H, out S, out B);

        foreach (Material m in affectedByLight)
        {
            m.SetFloat("_Hue", H * 360);
        }
    }
}
