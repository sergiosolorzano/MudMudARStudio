using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityARInterface
{
    public class DisableCloudPointsButton : MonoBehaviour
    {


        public void Disable()
        {
            GameObject root = GameObject.FindWithTag("root");
            ARPointCloudVisualizer ARPointCloudVisualizerScript = root.transform.GetComponent<ARPointCloudVisualizer>();
            ARPointCloudVisualizerScript.OnDisable();
            ARPointCloudVisualizerScript.enabled = false;
            gameObject.SetActive(false);
        }
    }
}
