using UnityEngine;

namespace UnityARInterface
{
    public class TargetParticles : MonoBehaviour
    {
        public GameObject targetObject;
        private ParticleSystem targetParticleSystem;
        private Renderer targetRenderer;

        //for editor version
        public float maxRayDistance = 30.0f;
        public LayerMask collisionLayerMask;
        public float findingScreenDist = 0.5f;

        ARPlaneVisualizer planeScript;

        public void Start()
        {
            planeScript = GameObject.FindWithTag("root").GetComponent<ARPlaneVisualizer>();
            targetParticleSystem = targetObject.GetComponent<ParticleSystem>();
            targetRenderer = targetParticleSystem.GetComponent<Renderer>();

            int layerIndex = LayerMask.NameToLayer("ARGameObject");
            collisionLayerMask = 1 << layerIndex;
        }
        //Called from PlayerAttack script
        public void Fire()
        {
        }

        void Update()
        {
            //use center of screen for focusing
            Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, findingScreenDist);
            Ray ray = Camera.main.ScreenPointToRay(center);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxRayDistance, collisionLayerMask))
            {
                Debug.Log("I am hitting plane");
                //we're going to get the position from the contact point
                targetParticleSystem.transform.position = hit.point;
                targetParticleSystem.transform.rotation = hit.transform.rotation;
                targetRenderer.material.SetColor("_TintColor", Color.green);
            }
            else
            {
                targetRenderer.material.SetColor("_TintColor", Color.red);
            }

            if (planeScript.PlaneDetected)
            {
                Debug.Log("No plane Detected");
                targetRenderer.material.SetColor("_TintColor", Color.green);
                //check camera forward is facing downward
                if (Vector3.Dot(Camera.main.transform.forward, Vector3.down) > 0)
                {
                    Debug.Log("No plane detected and in camera realignment");
                    //position the focus finding square a distance from camera and facing up
                    targetParticleSystem.transform.position = Camera.main.ScreenToWorldPoint(center);

                    //vector from camera to focussquare
                    Vector3 vecToCamera = targetParticleSystem.transform.position - Camera.main.transform.position;

                    //find vector that is orthogonal to camera vector and up vector
                    Vector3 vecOrthogonal = Vector3.Cross(vecToCamera, Vector3.up);

                    //find vector orthogonal to both above and up vector to find the forward vector in basis function
                    Vector3 vecForward = Vector3.Cross(vecOrthogonal, Vector3.up);

                    targetParticleSystem.transform.rotation = Quaternion.LookRotation(vecForward, Vector3.up);
                }
                else
                {
                    //we will not display finding square if camera is not facing below horizon
                    targetRenderer.material.SetColor("_TintColor", Color.red);
                }
            }
        }
    }
}

