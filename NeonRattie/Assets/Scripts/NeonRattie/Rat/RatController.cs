using System;
using System.Net.NetworkInformation;
using Flusk.Controls;
using Flusk.Management;
using NeonRattie.Controls;
using NeonRattie.Management;
using NeonRattie.Objects;
using NeonRattie.Rat.Data;
using NeonRattie.Rat.RatStates;
using NeonRattie.Utility;
using UnityEngine;
using UnityEngine.AI;

namespace NeonRattie.Rat
{
    [RequireComponent( typeof(RatAnimator))]
    public class RatController : MonoBehaviour, IMovable
    {
        [SerializeField] protected float walkSpeed = 10;
        [SerializeField] protected float runSpeed = 15;

        [SerializeField] protected Vector3 gravity;
        public Vector3 Gravity { get { return gravity; } }

        [SerializeField] protected float jumpForce = 10;
        public float JumpForce { get { return jumpForce; } }
        [SerializeField] protected AnimationCurve jumpArc;
        public AnimationCurve JumpArc { get { return jumpArc; } }
        [SerializeField] protected AnimationCurve jumpAnimationCurve;

        [SerializeField] protected float mass = 1;
        public float Mass { get { return mass; } }

        [SerializeField] protected float rotateAmount = 300;

        [SerializeField] protected Transform ratPosition;

        [SerializeField] protected LayerMask collisionMask;

        [SerializeField]
        protected MoveHelperState moveHelperState;

        public LayerMask CollisionMask
        {
            get { return collisionMask; }
        }

        [SerializeField]
        protected LayerMask groundLayer;

        [SerializeField]
        protected LayerMask jumpLayer;

        [SerializeField] protected float rotationAngleMultiplier = 1;
        [SerializeField] protected RotateController rotateController;
        public RotateController RotateController {get { return rotateController; }}

        
        //climbing data
        [SerializeField] protected AnimationCurve climbUpCurve;

        public AnimationCurve ClimbUpCurve
        {
            get { return climbUpCurve; }
        }

        [SerializeField] protected AnimationCurve forwardMotion;

        public AnimationCurve ForwardMotion
        {
            get { return forwardMotion; }
        }

        [SerializeField] protected AnimationMotion jumpOffCurve;
        public AnimationMotion JumpOffCurve
        {
            get { return jumpOffCurve; }
        }

        public Transform RatPosition
        {
            get { return ratPosition; }
        }

        public Vector3 Velocity { get; protected set; }

        private Vector3 previousPosition;
        private Vector3 currentPosition;

        public RatAnimator RatAnimator { get; protected set; }
        public NavMeshAgent NavAgent { get; protected set; }

        public JumpBox JumpBox { get; private set; }

        //other rat effects...

        private Vector3 offsetRotation;

        public Vector3 LowestPoint { get; protected set; }


        #region Rotation Data
        protected Vector3 rotationAxis;
        protected float rotationAngle;
        protected float rotationTime = 0;
        protected Updater rotationUpdater = new Updater();
        #endregion
        
        
        //TODO: write editor script so these can be configurable!
        public Vector3 ForwardDirection
        {
            get { return (Vector3.forward); }
        }

        public Vector3 LocalForward
        {
            get { return (transform.forward); }
        }

        public Bounds Bounds
        {
            get { return RatCollider.bounds; }
            
        }
        public Collider RatCollider { get; private set; }

        public event Action DrawGizmos;
        
        public Vector3 WalkDirection { get; private set; }


#if UNITY_EDITOR
        [ReadOnly, SerializeField] protected Vector3 forwardDirection;
#endif

        #region State stuff
        private readonly RatStateMachine ratStateMachine = new RatStateMachine();

        public RatStateMachine StateMachine
        {
            get { return ratStateMachine; }
        }
#if UNITY_EDITOR
        [ReadOnly, SerializeField]
        protected string ratState;
#endif
        //states and keys
        protected RatActionStates
            idle = RatActionStates.Idle,
            jump = RatActionStates.Jump,
            climb = RatActionStates.Climb,
            walk = RatActionStates.Walk,
            jumpOff = RatActionStates.JumpOff;

        protected Idle idling;
        protected Jump jumping;
        protected Climb climbing;
        protected Walk walking;
        protected JumpOff jumpingOff;
        #endregion

        public void ChangeState (RatActionStates state)
        {
            StateMachine.ChangeState(state);
        }

        public bool TryMove (Vector3 position)
        {
            return TryMove(position, groundLayer);
        }

        public bool TryMove(Vector3 position, LayerMask surface)
        {
            var hits = Physics.OverlapBox(position, RatCollider.bounds.extents * 0.5f, transform.rotation,
                surface);
            var success = hits.Length == 0;
            if (success)
            {
                transform.position = position;
            }
            return success;
        }

        public bool ClimbValid()
        {
            var direction = LocalForward;
            RaycastHit info;
            bool success = Physics.Raycast(transform.position, direction, out info, 5f, 1 << LayerMask.NameToLayer("Interactable"));
            if (success)
            {
                JumpBox = info.transform.GetComponentInChildren<JumpBox>();
                return JumpBox != null;
            }
            if ( JumpBox != null )
            {
                JumpBox.Select(false);
            }
            JumpBox = null;
            return false; 
        }

        //TODO: make this process more secure/elegant, some event or something   
        public void NullifyJumpBox()
        {
            JumpBox = null;
        }

        public bool JumpOffValid()
        {
            var direction = LocalForward;
            float length = RatCollider.bounds.extents.z * 0.3f;
            Vector3 frontPoint = RatCollider.bounds.ClosestPoint(transform.position + direction) + direction * 0.1f;
            Vector3 extendedPoint = frontPoint + length * direction;
            float height = RatCollider.bounds.extents.y;
            RaycastHit closest;
            RaycastHit furtherest;
            bool close = Physics.Raycast(frontPoint, Vector3.down, out closest);
            bool far = Physics.Raycast(extendedPoint, Vector3.down, out furtherest);
            if (!close || !far)
            {
                return false;
            }
            float difference = (closest.point - furtherest.point).y;
            return difference > height;
        }

        public bool IsGrounded()
        {
            return GetGroundData(0.1f).transform != null;
        }
        
        public void Walk(Vector3 direction)
        {
            if (NavAgent == null)
            {
                Vector3 translate = transform.position + direction * walkSpeed * Time.deltaTime;
                TryMove(translate, collisionMask);
                return;
            }
            NavAgent.SetDestination(transform.position + direction * walkSpeed);
        }    


        public RaycastHit GetGroundData (float distance = 10000)
        {
            RaycastHit info;
            Physics.Raycast(transform.position, -transform.up, out info, distance, groundLayer);
            return info;
        }

        protected virtual void OnManagementLoaded()
        {
            SceneManagement.Instance.Rat = this;
            NavAgent = GetComponentInChildren<NavMeshAgent>();
            Init();
            RatCollider = GetComponent<Collider>();
        }

        
        private void Init()
        {
            RatAnimator = GetComponent<RatAnimator>();

            ratStateMachine.Init(this);

            idling = new Idle();
            walking = new Walk();
            jumping = new Jump();
            climbing = new Climb();
            jumpingOff = new JumpOff();

            idling.Init(this, ratStateMachine);
            walking.Init(this, ratStateMachine);
            jumping.Init(this, ratStateMachine);
            climbing.Init(this, ratStateMachine);
            jumpingOff.Init(this, ratStateMachine);

            ratStateMachine.AddState(idle, idling);
            ratStateMachine.AddState(walk, walking);
            ratStateMachine.AddState(jump, jumping);
            ratStateMachine.AddState(climb, climbing);
            ratStateMachine.AddState(jumpOff, jumpingOff);
            ratStateMachine.ChangeState(idle);
            
            //update helpers
            rotationUpdater.Add(UpdateRotation);
        }

        /// <summary>
        /// Sets the rats target rotation to angle, around an axis
        /// </summary>
        public virtual void RotateRat(float angle, Vector3 axis)
        {
            rotationAxis = axis;
            rotationAngle = angle;
        }

        public virtual void RotateRat(float angle)
        {
            RotateRat(angle, Vector3.up);
        }

        public void AddDrawGizmos (Action action)
        {
            DrawGizmos += action;                   
        }

        public void RemoveDrawGizmos (Action action)
        {
            if (DrawGizmos != null)
            {
                DrawGizmos -= action;
            }
        }

        protected virtual void Update()
        {
            ratStateMachine.Tick();
            ClimbValid();
            rotationUpdater.Update(Time.deltaTime);
            if  (JumpBox != null )
            {
                JumpBox.Select();
            }
#if UNITY_EDITOR
            forwardDirection = ForwardDirection;
            ratState = ratStateMachine.CurrentState.ToString();
#endif
        }
        
        protected virtual void LateUpdate()
        {
            UpdateVelocity(Time.deltaTime);
            FindLowestPoint();
        }
        

        protected virtual void OnEnable()
        {
            MainPrefab.ManagementLoaded += OnManagementLoaded;
            PlayerControls.Instance.Walk += OnWalk;
        }

        protected virtual void OnDisable()
        {
            MainPrefab.ManagementLoaded -= OnManagementLoaded;
            PlayerControls.Instance.Walk -= OnWalk;
        }

       
        
        protected virtual void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, transform.position + LocalForward * 10);
            if ( DrawGizmos != null )
            {
                DrawGizmos();
            }
        }

        private void UpdateVelocity(float deltaTime)
        {
            currentPosition = transform.position;
            Vector3 difference = previousPosition - currentPosition;
            Velocity = difference / deltaTime;
            previousPosition = currentPosition;
        }

        private void FindLowestPoint ()
        {
            Vector3 point = transform.position - Vector3.down * 10;
            LowestPoint = Bounds.ClosestPoint(point);
        }

        private void OnWalk(float axis)
        {
            KeyboardControls keyboard;
            PlayerControls player;
            if (!KeyboardControls.TryGetInstance(out keyboard) || !PlayerControls.TryGetInstance(out player))
            {
                return;
            }
            Vector3 forward = SceneObjects.Instance.CameraControls.GetFlatForward();
            Vector3 right = SceneObjects.Instance.CameraControls.GetFlatRight();
            WalkDirection = Vector3.zero;
            WalkDirection += keyboard.CheckKey(player.Forward) ? forward : Vector3.zero;
            WalkDirection += keyboard.CheckKey(player.Back) ? -forward : Vector3.zero;
            WalkDirection += keyboard.CheckKey(player.Right) ? right : Vector3.zero;
            WalkDirection += keyboard.CheckKey(player.Left) ? -right : Vector3.zero;
            WalkDirection.Normalize();
        }
        
        protected void UpdateRotation(float time)
        {
            Quaternion next = Quaternion.AngleAxis(rotationAngle * rotationAngleMultiplier, rotationAxis);
            next = transform.rotation * next;
            rotationTime += time;
            rotationTime %= 1;
            transform.rotation = Quaternion.Slerp(transform.rotation, next, rotationTime);
        }
    }
}
