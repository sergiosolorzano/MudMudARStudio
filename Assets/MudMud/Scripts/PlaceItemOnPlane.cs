using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityARInterface;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlaceItemOnPlane : ARBase
{
    [SerializeField]
    public Transform instancedObjectT;
    private GameObject instancedGO;
    private bool objectInstanced;
    private ScaleGUI scaleGUIScript;
    private ARController m_ARController;

    private GameObject canvas;
    private Slider slider;
    private SliderInUse sliderInUseScript;

    public bool ObjectInstanced
    {
        get
        {
            return objectInstanced;
        }
    }
    public GameObject InstancedGO {
        get
        {
            return instancedGO;
        }
    }
    
    private void Start()
    {
        canvas = GameObject.FindWithTag("canvas");
        slider = canvas.GetComponentInChildren<Slider>();
        sliderInUseScript = canvas.GetComponent<SliderInUse>();
        m_ARController = GetFirstEnabledControllerInChildren();
    }
    
    void Update ()
    {    
        if (Input.GetMouseButton(0))
        {
                var camera = GetCamera();
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);

                int layerMask = 1 << LayerMask.NameToLayer("ARGameObject"); // Planes are in layer ARGameObject
                int layerMaskButton = 1 << LayerMask.NameToLayer("DestroyPlaneButton"); 

            RaycastHit rayHit;
                if (Physics.Raycast(ray, out rayHit, float.MaxValue, layerMask) && !Physics.Raycast(ray, out rayHit, float.MaxValue, layerMaskButton))
                {
                    bool UIInUse = sliderInUseScript.checkUIUse();
                    //Debug.Log("UIInUse " + UIInUse);
                    if (!UIInUse)
                    {
                        if (objectInstanced == false)
                        {
                        instancedObjectT.transform.position = rayHit.point;
                        instancedGO = instancedObjectT.gameObject;

                        Vector3 targetPostition = new Vector3(camera.transform.position.x,
                                        instancedGO.transform.position.y,
                                        camera.transform.position.z);

                        instancedGO.transform.LookAt(targetPostition);

                        instancedGO.SetActive(true);
                        //init pointOfInt and AlignWithPointOfInt
                        m_ARController.pointOfInterest = instancedGO.transform.position;
                        m_ARController.AlignWithPointOfInterest(rayHit.point);
                        objectInstanced = true;
                    }
                        else
                        {
                            m_ARController.pointOfInterest = instancedGO.transform.position;
                            m_ARController.AlignWithPointOfInterest(rayHit.point);
                        }
                    }
                    sliderInUseScript.SliderIsInUse = false;
                }
        }
    }
}
