using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UnityARInterface
{
    public class SliderInUse : MonoBehaviour
    {
        GraphicRaycaster m_Raycaster;
        PointerEventData m_PointerEventData;
        EventSystem m_EventSystem;
        private bool sliderIsInUse;
        private bool renderPlanes;
        GameObject rootGO;
        ARPlaneVisualizer ARPlaneVisualizerScript;
        private GameObject RenderPlanesButton;
        private GameObject TargetAnim;
        private ARTargetMaterialSwitch targetAnimScript;
        //public Text enableTextGO;
        //public Text disableTextGO;

        public bool SliderIsInUse
        {
            get
            {
                return sliderIsInUse;
            }
            set
            {
                sliderIsInUse = value;
            }
        }

        void Start()
        {
            TargetAnim = GameObject.FindWithTag("TargetAnim");
            targetAnimScript = TargetAnim.transform.GetComponent<ARTargetMaterialSwitch>();
            //Any UI touch listener
            //Fetch the Raycaster from the GameObject (the Canvas)
            m_Raycaster = GetComponent<GraphicRaycaster>();
            //Fetch the Event System from the Scene
            m_EventSystem = GetComponent<EventSystem>();
            //Render Planes
            rootGO = GameObject.FindWithTag("root");
            ARPlaneVisualizerScript = rootGO.transform.GetComponent<ARPlaneVisualizer>();
            RenderPlanesButton = GameObject.FindWithTag("RenderPlanesButton");
            RenderPlanesButton.GetComponent<Button>().onClick.AddListener(() => OnClickRenderPlanes());
            renderPlanes = true;
        }

        public void OnClickRenderPlanes()
        {
            renderPlanes = !renderPlanes;

            //switch on target Anims
            if (renderPlanes)
                targetAnimScript.OnEnable();
            else
                targetAnimScript.OnDisable();
            //render planes or not
            RenderPlanes();
        }

        public void RenderPlanes()
        {
            List<string> idList = new List<string>(ARPlaneVisualizerScript.M_Planes.Keys);
            foreach (string planeID in idList)
            {
                GameObject go;

                if (idList == null)
                    return;
                if (ARPlaneVisualizerScript.M_Planes.TryGetValue(planeID, out go))
                {
                    if(renderPlanes)
                    {
                        go.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                        //disableTextGO.enabled = true;
                        //enableTextGO.enabled = false;
                    }
                    else
                    {
                        go.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                        //disableTextGO.enabled = false;
                        //enableTextGO.enabled = true;
                    }   
                }
            }
        }

        public bool checkUIUse()
        {
            //Check if the left Mouse button is clicked
            //Debug.Log("Slide is in use " + SliderIsInUse);
            if (Input.touchCount > 0 || Input.GetMouseButton(0))
            {
                //Set up the new Pointer Event
                m_PointerEventData = new PointerEventData(m_EventSystem);
                //Set the Pointer Event Position to that of the mouse position
                m_PointerEventData.position = Input.mousePosition;

                //Create a list of Raycast Results
                List<RaycastResult> results = new List<RaycastResult>();

                //Raycast using the Graphics Raycaster and mouse click position
                m_Raycaster.Raycast(m_PointerEventData, results);
                //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
                foreach (RaycastResult result in results)
                {
                    //if (result.gameObject.name.Equals("ScaleBackground") || result.gameObject.name.Equals("ScaleFill") || result.gameObject.name.Equals("ScaleHandle"))
                    SliderIsInUse = true;
                    //if (result.gameObject.name.Equals("RenderPlanesButton"))  
                }
                return SliderIsInUse;
            }
            else
            {
                SliderIsInUse = false;
                return SliderIsInUse;
            }
        }
    }
}