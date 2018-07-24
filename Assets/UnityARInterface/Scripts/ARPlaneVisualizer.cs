using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityARInterface
{
    public class ARPlaneVisualizer : ARBase
    {
        [SerializeField]
        private GameObject m_PlanePrefab;

        [SerializeField]
        private int m_PlaneLayer;

        private GameObject instancedPlaneGO;
        Transform destroyButton;
        private bool planeDetected;
        public bool PlaneDetected { get { return planeDetected; } }

        public int planeLayer { get { return m_PlaneLayer; } }
        public int planeCreatedNumber = 0;

        private Dictionary<string, GameObject> m_Planes = new Dictionary<string, GameObject>();
        public Dictionary<string, GameObject> M_Planes
        {
            get
            {
                return m_Planes;
            }
        }
        public GameObject InstancedPlaneGO
        {
            get
            {
                return instancedPlaneGO;
            }
        }

        void OnEnable()
        {
            m_PlaneLayer = LayerMask.NameToLayer ("ARGameObject");
            ARInterface.planeAdded += PlaneAddedHandler;
            ARInterface.planeUpdated += PlaneUpdatedHandler;
            ARInterface.planeRemoved += PlaneRemovedHandler;
        }

        void OnDisable()
        {
            ARInterface.planeAdded -= PlaneAddedHandler;
            ARInterface.planeUpdated -= PlaneUpdatedHandler;
            ARInterface.planeRemoved -= PlaneRemovedHandler;
        }

        protected virtual void CreateOrUpdateGameObject(BoundedPlane plane)
        {
            if (!m_Planes.TryGetValue(plane.id, out instancedPlaneGO))
            {
                //Debug.Log("We create/update plane " + plane.id);
                instancedPlaneGO = Instantiate(m_PlanePrefab, GetRoot());
                instancedPlaneGO.name = string.Concat(instancedPlaneGO.name, planeCreatedNumber);
                planeCreatedNumber++;
                planeDetected = true;
                // Make sure we can pick them later
                foreach (var collider in instancedPlaneGO.GetComponentsInChildren<Collider>())
                    collider.gameObject.layer = m_PlaneLayer;

                m_Planes.Add(plane.id, instancedPlaneGO);
                EventManager.TriggerEvent("PlaneInstanced");
            }

            instancedPlaneGO.transform.localPosition = plane.center;
            instancedPlaneGO.transform.localRotation = plane.rotation;
            instancedPlaneGO.transform.localScale = new Vector3(plane.extents.x, 1f, plane.extents.y);
            //Debug.Log("Now at CreateOrUpdate - plane id " + plane.id + " has center " + plane.center + " with x " + plane.extents.x + " ABS(Center)-ABS(X)= " + (Mathf.Abs(plane.center.x)-Mathf.Abs(plane.extents.x)));
        }

        protected virtual void PlaneAddedHandler(BoundedPlane plane)
        {
            //Debug.Log("Now calling PlaneAddedHandler and we Add plane " + plane.id);
            if (m_PlanePrefab)
                CreateOrUpdateGameObject(plane);
        }

        protected virtual void PlaneUpdatedHandler(BoundedPlane plane)
        {
            //Debug.Log("Now calling PlaneUpdateHandler and we update plane " + plane.id);
            if (m_PlanePrefab)
                CreateOrUpdateGameObject(plane);
        }

        protected virtual void PlaneRemovedHandler(BoundedPlane plane)
        {
            GameObject go;
            if (m_Planes.TryGetValue(plane.id, out go))
            {
                //Debug.Log("We Destroy plane " + plane.id);
                Destroy(go);
                m_Planes.Remove(plane.id);
            }
        }
        public void Remove(string key, GameObject go)
        {
            m_Planes.Remove(key);
        }

    }
}
