using UnityEngine;
namespace SmartAI.Doors
{
    public class Switch : MonoBehaviour
    {
        [SerializeField] private BaseDoor[] triggerDoor;
        public bool Activated => !toggle;
        
        private bool toggle = true;
        private Transform LeverBase;
        private Transform onPos;
        private Transform offPos;

        private void Awake()
        {
            LeverBase = transform.Find("LeverBase");
            onPos = transform.Find("OnPos");
            offPos = transform.Find("OffPos");
            ToggleSwitch();
        } 

        [Header("Debugging")]
        [SerializeField] private bool debug;
        private void Update()
        {
            // rotates the lever to the the on or off position
            Quaternion rot = toggle ? offPos.rotation : onPos.rotation;
            LeverBase.rotation = Quaternion.Lerp(LeverBase.rotation, rot, 2 * Time.deltaTime);

            foreach(BaseDoor door in triggerDoor) door.OpenDoor(!toggle);
            
            // debugging only
            if(debug)
            {
                ToggleSwitch();
                debug = false;
            }
        }

        /// <summary> When called will
        /// toggle the switch on or off </summary>
        public void ToggleSwitch()
        {
            toggle = !toggle;
        }
            

        /// <summary> Debugging only </summary>
        private void OnDrawGizmos()
        {
            foreach(BaseDoor door in triggerDoor)
            {
                if(door != null)
                {
                    var posA = transform.position;
                    var posB = door.transform.position;
                 
                    Gizmos.color = Activated? Color.red : Color.green;
                    Gizmos.DrawLine(posA, posB);
                }
            }        
        }
    }
}