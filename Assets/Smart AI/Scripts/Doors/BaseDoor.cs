using UnityEngine;
namespace SmartAI.Doors
{
    public class BaseDoor : MonoBehaviour
    {
        [SerializeField] private Transform panel;
        [SerializeField] private float smoothing;
        public bool open;
    
        [SerializeField] private Transform openPos;
        [SerializeField] private Transform closedPos;

        protected virtual void Awake()
        {
            openPos = transform.Find("OpenPos");
            closedPos = transform.Find("ClosedPos");
        }
        
        public virtual void OpenDoor(bool _open)
        {
            Transform endpos = _open ? openPos : closedPos;
            panel.position = Vector3.Lerp(panel.position, endpos.position, (smoothing * Time.deltaTime));

            open = _open;
        }
    }
}