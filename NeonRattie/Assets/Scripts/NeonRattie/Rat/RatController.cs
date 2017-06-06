using System;
using Flusk.Controls;
using Flusk.Extensions;
using Flusk.Management;
using NeonRattie.Controls;
using NeonRattie.Management;
using NeonRattie.Objects;
using NeonRattie.Rat.RatStates;
using NeonRattie.Shared;
using UnityEngine;
using UnityEngine.AI;

namespace NeonRattie.Rat
{
    [RequireComponent( typeof(RatAnimator))]
    public class RatController : NeonRattieBehaviour, IMovable
    {
        [SerializeField] protected float walkSpeed = 10;
        [SerializeField] protected float runSpeed = 15;

        [SerializeField] protected Vector3 gravity;
        public Vector3 Gravity { get { return gravity; } }

        [SerializeField] protected float jumpForce = 10;
        public float JumpForce { get { return jumpForce; } }
        [SerializeField] protected AnimationCurve jumpArc;
        public AnimationCurve JumpArc { get { return jumpArc; } }
        [SerializeField] protected AnimationCurve JumpAnimationCurve;

        [SerializeField] protected float mass = 1;
        public float Mass { get { return mass; } }

        [SerializeField] protected float rotateAmount = 300;

        [SerializeField] protected Transform ratPosition;

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

        //TODO: right editor script so these can be configurable!
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
        private RatStateMachine ratStateMachine = new RatStateMachine();

        public RatStateMachine StateMachine
        {
            get { return ratStateMachine; }
        }

        [ReadOnly, SerializeField]
        protected string RatState;

        //states and keys
        protected RatActionStates
            idle = RatActionStates.Idle,
            jump = RatActionStates.Jump,
            climb = RatActionStates.Climb,
            walk = RatActionStates.Walk;

        protected Idle idling;
        protected Jump jumping;
        protected Climb climbing;
        protected Walk walking;
        #endregion

        public void ChangeState (RatActionStates state)
        {
            StateMachine.ChangeState(state);
        }

        public void Move(Vector3 position)
        {
            transform.position = position;
        }

        public bool TryMove (Vector3 position)
        {
            return TryMove(position, groundLayer);
        }

        public bool TryMove(Vector3 position, LayerMask? surface)
        {
            if (surface == null)
            {
                surface = LayerMask.NameToLayer("Everything");
            }
            Vector3 point = RatCollider.bounds.ClosestPoint(position);
            float distance = (position - point).magnitude;
            Vector3 direction = (position - point).normalized;
            Ray ray = new Ray(point, direction);
            Debug.DrawRay(point, direction, Color.red);
            RaycastHit hit;
            bool success = Physics.Raycast(ray, out hit, distance, surface.Value);
            Collider[] hits = Physics.OverlapBox(position, RatCollider.bounds.extents, transform.rotation,
                surface.Value);
            success = hits.Length == 0;
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

        public bool IsGrounded()
        {
            return GetGroundData(0.1f).transform != null;
        }
        
        public void Walk(Vector3 direction)
        {
            if (NavAgent == null)
            {
                transform.Translate(direction * walkSpeed * Time.deltaTime, Space.World);
                return;
            }
            NavAgent.SetDestination(transform.position + direction * walkSpeed);
        }


        public RaycastHit GetGroundData (float distance = 10000)
        {
            RaycastHit info;
            bool hit = Physics.Raycast(transform.position, -transform.up, out info, distance, groundLayer);
            return info;
        }

        protected virtual void OnManagementLoaded()
        {
            SceneManagement.Instance.Rat = this;
            NavAgent = GetComponentInChildren<NavMeshAgent>();
            Init();
            offsetRotation = new Vector3(-1, 0, 1);
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

            idling.Init(this, ratStateMachine);
            walking.Init(this, ratStateMachine);
            jumping.Init(this, ratStateMachine);
            climbing.Init(this, ratStateMachine);

            ratStateMachine.AddState(idle, idling);
            ratStateMachine.AddState(walk, walking);
            ratStateMachine.AddState(jump, jumping);
            ratStateMachine.AddState(climb, climbing);
            ratStateMachine.ChangeState(idle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="axis"></param>
        public virtual void RotateRat(float angle, Vector3 axis)
        {
            transform.RotateAround(transform.position, axis, angle * rotationAngleMultiplier);
        }

        public virtual void RotateRat(float angle)
        {
            RotateRat(angle, Vector3.up);
        }

        public override void Destroy()
        {
            throw new System.NotImplementedException();
        }

        public override void Initialise()
        {
            throw new System.NotImplementedException();
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
            if  (JumpBox != null )
            {
                JumpBox.Select(true);
            }
#if UNITY_EDITOR
            forwardDirection = ForwardDirection;
            RatState = ratStateMachine.CurrentState.ToString();
#endif
        }
        
        protected virtual void LateUpdate()
        {
            UpdateVelocity(Time.deltaTime);
            FindLowestPoint();
            WalkDirection = Vector3.zero;
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
    }
}
