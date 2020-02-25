using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
    [Category("GameObject")]
    public class AddForce : ActionTask
    {
        
        public BBParameter<Transform> parent;
        
        public BBParameter<Vector3> force;
        public BBParameter<Vector3> relativeForce;
        public BBParameter<Vector3> torque;
        public BBParameter<Vector3> relativeTorque;
        
        [BlackboardOnly]
        public BBParameter<GameObject> saveCloneAs;

        protected override string info {
            get { return "Add force " + agentInfo + " under " + ( parent.value ? parent.ToString() : "World" ) + " as " + saveCloneAs; }
        }

        protected override void OnExecute() {
#if UNITY_5_4_OR_NEWER

            var clone = (GameObject) agent.gameObject;

#else

            var clone = (GameObject) agent.gameObject;
            clone.transform.SetParent(parent.value);

#endif
            agent.gameObject.AddComponent<Rigidbody>();
            var agentConstantForce = agent.gameObject.AddComponent<ConstantForce>();
            agentConstantForce.force = force.value;
            agentConstantForce.relativeForce = relativeForce.value;
            agentConstantForce.torque = torque.value;
            agentConstantForce.relativeTorque = relativeTorque.value;
            
            saveCloneAs.value = clone;
            EndAction();
        }
    }
}