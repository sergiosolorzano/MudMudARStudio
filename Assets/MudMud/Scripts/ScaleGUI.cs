using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityARInterface;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScaleGUI : ARBase
{
    private GameObject canvas;
    private Slider scaleSlider;

    private Slider rotationSlider;
    private float rotationAngle=0.0f;

    private GameObject Item;

    private float scale;
    private GameObject rootGO;
    private ARController ARControllerScript;

    public void Start()
    {
        rootGO = GameObject.FindWithTag("root");
        ARControllerScript = rootGO.transform.GetComponent<ARController>();

        canvas = GameObject.FindWithTag("canvas");
        //ScaleSlider
        scaleSlider = GameObject.FindWithTag("ScaleSlider").GetComponent<Slider>();
        scaleSlider.value = ARControllerScript.scale;
        scaleSlider.onValueChanged.AddListener(delegate { SetScale(); });

        Item = GameObject.FindWithTag("Studio");

        //Debug.Log("initial " + InitialDistToCam());
        scaleSlider.minValue = 0.04f*InitialDistToCam();
        scaleSlider.maxValue = InitialDistToCam();
        scaleSlider.value = (scaleSlider.minValue + scaleSlider.maxValue) / 2;

        //RotateSlider
        rotationSlider = GameObject.FindWithTag("RotationSlider").GetComponent<Slider>();

        var camera = GetCamera();
        
        /*Vector3 targetPostition = new Vector3(camera.transform.position.x,
                                        transform.position.y,
                                        camera.transform.position.z);

        transform.LookAt(targetPostition);*/

        rotationSlider.value = rotationAngle;
        ARControllerScript.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.up);

        rotationSlider.onValueChanged.AddListener(delegate { SetRotation(); });

        //Debug.Log("InitialDistToCam " + InitialDistToCam() + " Initial min value " + slider.minValue);
        canvas.GetComponent<Canvas>().enabled = true;
        SetScale();
        SetRotation();
    }

    public float InitialDistToCam()
    {
        //Debug.Log("Initial m_ARController.pointOfInterest  " + ARControllerScript.pointOfInterest + " initial ARControllerScript.scale " + ARControllerScript.scale);
        var camera = GetCamera();
        
        var camPosAtRoot = camera.transform.parent;
        float distObjectToCam = Mathf.Sqrt(Mathf.Pow(camPosAtRoot.localPosition.x - Item.transform.localPosition.x, 2)
                        + Mathf.Pow(camPosAtRoot.localPosition.y - Item.transform.localPosition.y, 2)
                        + Mathf.Pow(camPosAtRoot.localPosition.z - Item.transform.localPosition.z, 2));
        return distObjectToCam;
    }

    /*public float RuntimeDistToCam()
    {
        var camera = GetCamera();
        var camPosAtRoot = camera.transform.parent;
        
        var distObjectToCam = Mathf.Sqrt(Mathf.Pow(camPosAtRoot.localPosition.x - ARControllerScript.pointOfInterest.x, 2)
                        + Mathf.Pow(camPosAtRoot.localPosition.y - ARControllerScript.pointOfInterest.y, 2)
                        + Mathf.Pow(camPosAtRoot.localPosition.z - ARControllerScript.pointOfInterest.z, 2));
        return distObjectToCam;
    }*/

    public void SetRotation()
    {
        //Debug.Log("BEFORE slider rotation " + rotationSlider.value + " rotationAngle-1 " + rotationAngle);
        if (rotationAngle!= rotationSlider.value)
        {
            ARControllerScript.rotation = Quaternion.AngleAxis(rotationSlider.value, Vector3.up);
            rotationAngle=rotationSlider.value;
            //Debug.Log("AFTER slider rotation " + rotationSlider.value + " rotationAngle-1 " + rotationAngle);
        }
    }

    public void SetScale()
    {
        //Debug.Log("Calling SetScale");
        //Debug.Log("slider value " + slider.value + " ARControllerScript.scale " + ARControllerScript.scale);
        scale = scaleSlider.value;
     
        if (scale != ARControllerScript.scale)
            ARControllerScript.scale = scale;
        
    }
}
