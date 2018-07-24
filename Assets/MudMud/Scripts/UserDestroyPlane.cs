using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityARInterface;
using UnityEngine.UI;
using System.Linq;

namespace UnityARInterface
{
    public class UserDestroyPlane : ARBase
    {
        private GameObject rootGO;
        ARPlaneVisualizer ARPlaneVisualizerScript;
        PlaceItemOnPlane placeItemOnPlaceScript;
        private GameObject planeToDestroy;
        private int layerName;
        private SliderInUse sliderInUseScript;
        private GameObject canvas;

        //listeners
        private UnityAction planeInstancedListener;
        void Awake()
        {
                  planeInstancedListener = new UnityAction (PlaneInstanced);
        }
        private void OnEnable()
        {
            EventManager.StartListening("PlaneInstanced", planeInstancedListener);
        }
        private void OnDisable()
        {
            EventManager.StopListening("PlaneInstanced", planeInstancedListener);
        }

        public void Start()
        {
            rootGO = GameObject.FindWithTag("root");
            ARPlaneVisualizerScript = rootGO.transform.GetComponent<ARPlaneVisualizer>();
            placeItemOnPlaceScript = rootGO.transform.GetComponent<PlaceItemOnPlane>();
            canvas = GameObject.FindWithTag("canvas");
            sliderInUseScript = canvas.GetComponent<SliderInUse>();
        }

        private void PlaneInstanced()
        {
            GameObject closeButton = ARPlaneVisualizerScript.InstancedPlaneGO.transform.GetChild(0).GetChild(0).gameObject;
            closeButton.layer = LayerMask.NameToLayer("DestroyPlaneButton");
            //stop render of new plane
            sliderInUseScript.RenderPlanes();
        }

        public void Update()
        {
            if (Input.GetMouseButton(0))
            {
                var camera = GetCamera();
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);

                int layerMask = 1 << LayerMask.NameToLayer("DestroyPlaneButton");

                RaycastHit rayHit;
                if (Physics.Raycast(ray, out rayHit, float.MaxValue, layerMask))
                {
                    planeToDestroy = rayHit.transform.parent.parent.gameObject;
                    //Debug.Log("plane hit " + planeToDestroy);

                    List<string> idList = new List<string>(ARPlaneVisualizerScript.M_Planes.Keys);
                    foreach (string planeID in idList)
                    {
                        GameObject go;

                        if (ARPlaneVisualizerScript.M_Planes.TryGetValue(planeID, out go))
                        {
                            if (go.name.Equals(planeToDestroy.name))
                            {
                                //Debug.Log("Now checking plane key " + planeID + " and out planetodestroy " + planeToDestroy);
                                //Debug.Log("Plane " + plane.Value + " which therefore has plane.id " + plane.Key);
                                Destroy(planeToDestroy);
                                ARPlaneVisualizerScript.Remove(planeID, go);
                            }
                        }
                    }
                    /*foreach (KeyValuePair<string, GameObject> plane in ARPlaneVisualizerScript.M_Planes)
                    {
                        Debug.Log("At END - Plane " + plane.Value + " which therefore has plane.id " + plane.Key);
                    }*/   
                }
            }
        }
    }
}


