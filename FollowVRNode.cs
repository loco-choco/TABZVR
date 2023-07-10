using UnityEngine;
using UnityEngine.VR;

namespace TABZVR
{
    public class FollowVRNode : MonoBehaviour
    {
        private VRNode node = VRNode.Head;

        public void SetVRNode(VRNode vrNode) 
        {
            node = vrNode;
        }

        public void Update() 
        {
            try
            {
                transform.localPosition = InputTracking.GetLocalPosition(node);
                transform.localRotation = InputTracking.GetLocalRotation(node);
            }
            catch { }
        }
    }
}
