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

        public Vector3 Velocity { get; protected set; }

        private Vector3 previousPosition;
        private Vector3 currentPosition;

        public RatAnimator RatAnimator { get; protected set; }
        public NavMeshAgent NavAgent { get; protected set; }

        //other rat effects...

        private Vector3 offsetRotation;

        private Vector3 ForwardDirection
        {
            get { return (-Vector3.right).normalized; }
        }

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
            Debug.Log("Wal forward");
            Walk(ForwardDirection);
        }

        public void WalkBackward()
        {
            Debug.Log("Wal backward");
            Walk(-ForwardDirection);
        }

        protected virtual void OnManagementLoaded()
        {
            (SceneManagement.Instance as SceneManagement).Rat = this;
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
