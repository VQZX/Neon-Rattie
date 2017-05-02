using Flusk.Management;
using NeonRattie.Controls;
using NeonRattie.Rat.RatStates;
using NeonRattie.Shared;
using UnityEngine;
using UnityEngine.AI;

namespace NeonRattie.Rat
{
    [RequireComponent( typeof(RatAnimator))]
    public class RatController : NeonRattieBehaviour
    {
        [SerializeField] protected float walkSpeed = 10;
        [SerializeField] protected float runSpeed = 15;

        [SerializeField] protected Vector3 gravity;
        public Vector3 Gravity { get { return gravity; } }

        [SerializeField] protected float jumpForce = 10;
        public float JumpForce { get { return jumpForce; } }
        [SerializeField] protected AnimationCurve jumpArc;
        public AnimationCurve JumpAnimationCurve;

        [SerializeField] protected float mass = 1;
        public float Mass { get { return mass; } }

        [SerializeField] protected float rotateAmount = 300;

        [SerializeField] protected Transform ratPosition;

        public Transform RatPosition
        {
            get { return ratPosition; }
        }

        public Vector3 Velocity { get; protected set; }

        private Vector3 previousPosition;
        private Vector3 currentPosition;

        public RatAnimator RatAnimator { get; protected set; }
        public NavMeshAgent NavAgent { get; protected set; }
       

        //other rat effects...

        private Vector3 offsetRotation;

        //TODO: right editor script so these can be configurable!
        public Vector3 ForwardDirection
        {
            get { return (-Vector3.right).normalized; }
        }

        public Vector3 LocalForward
        {
            get { return (-transform.right).normalized; }
        }


#if UNITY_EDITOR
        [ReadOnly, SerializeField] protected Vector3 forwardDirection;
#endif

        private RatStateMachine ratStateMachine = new RatStateMachine();

        public RatStateMachine StateMachine
        {
            get { return ratStateMachine; }
        }

        //states and keys
        protected RatActionStates
            idle = RatActionStates.Idle,
            jump = RatActionStates.Jump,
            climb = RatActionStates.Climb,
            walk = RatActionStates.Walk,
            reverse = RatActionStates.Reverse;

        protected Idle idling;
        protected Jump jumping;
        protected Climb climbing;
        protected Walk walking;
        protected WalkBack reversing;

        public void TankControls()
        {
            if (PlayerControls.Instance.CheckKey(KeyCode.A))
            {
                RotateRat(-rotateAmount * Time.deltaTime * Mathf.Deg2Rad);
            }
            if (PlayerControls.Instance.CheckKey(KeyCode.D))
            {
                RotateRat(rotateAmount * Time.deltaTime * Mathf.Deg2Rad);
            }
        }


        public bool ClimbValid()
        {
            return false;
        }

        public bool IsGrounded()
        {
            return false;
        }

        public void WalkForward()
        {

            Walk(ForwardDirection);
        }

        public void WalkBackward()
        {
            Walk(-ForwardDirection);
        }

        protected virtual void OnManagementLoaded()
        {
            SceneManagement.Instance.Rat = this;
            NavAgent = GetComponentInChildren<NavMeshAgent>();
            Init();
            offsetRotation = new Vector3(-1, 0, 1);
        }

        private void Walk(Vector3 direction)
        {
            if (NavAgent == null)
            {
                transform.Translate(direction * walkSpeed * Time.deltaTime, Space.Self);
                return;
            }
            NavAgent.SetDestination(transform.position + direction * walkSpeed);
        }
        
        private void Init()
        {
            RatAnimator = GetComponent<RatAnimator>();

            ratStateMachine.Init(this);

            idling = new Idle();
            walking = new Walk();
            jumping = new Jump();
            climbing = new Climb();
            reversing = new WalkBack();

            idling.Init(this, ratStateMachine);
            walking.Init(this, ratStateMachine);
            jumping.Init(this, ratStateMachine);
            climbing.Init(this, ratStateMachine);
            reversing.Init(this, ratStateMachine);

            ratStateMachine.AddState(idle, idling);
            ratStateMachine.AddState(walk, walking);
            ratStateMachine.AddState(jump, jumping);
            ratStateMachine.AddState(climb, climbing);
            ratStateMachine.AddState(reverse, reversing);
            ratStateMachine.ChangeState(idle);
        }

        /// <summary>
        /// rotate around y-axis
        /// </summary>
        /// <param name="angle"></param>
        public virtual void RotateRat(float angle)
        {
            transform.RotateAround(transform.position, Vector3.up, angle);
        }

        public override void Destroy()
        {
            throw new System.NotImplementedException();
        }

        public override void Initialise()
        {
            throw new System.NotImplementedException();
        }

        protected virtual void Update()
        {
           ratStateMachine.Tick();
#if UNITY_EDITOR
            forwardDirection = ForwardDirection;
#endif
        }

        protected virtual void OnEnable()
        {
            MainPrefab.ManagementLoaded += OnManagementLoaded;
        }

        protected virtual void OnDisable()
        {
            MainPrefab.ManagementLoaded -= OnManagementLoaded;
        }

        protected virtual void LateUpdate()
        {
            UpdateVelocity(Time.deltaTime);
        }

        private void UpdateVelocity(float deltaTime)
        {
            currentPosition = transform.position;
            Vector3 difference = previousPosition - currentPosition;
            Velocity = difference / deltaTime;
            previousPosition = currentPosition;
        }
    }
}
