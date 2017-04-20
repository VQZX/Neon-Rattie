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


        private RatStateMachine ratStateMachine = new RatStateMachine();

        //states and keys
        protected RatActionStates idle, jump, climb, walk;

        protected Idle idling;
        protected Jump jumping;
        protected Climb climbing;
        protected Walk walking;


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
            if (NavAgent == null)
            {
                transform.Translate(Vector3.forward * walkSpeed, Space.Self);
                return;
            }
            NavAgent.SetDestination(transform.position + transform.forward * walkSpeed);
        }

        protected virtual void OnManagementLoaded()
        {
            (SceneManagement.Instance as SceneManagement).Rat = this;
            NavAgent = GetComponentInChildren<NavMeshAgent>();
        }
        
        private void Init()
        {
            ratStateMachine.AddState(idle, idling);
            ratStateMachine.AddState(walk, walking);
            ratStateMachine.AddState(jump, jumping);
            ratStateMachine.AddState(climb, climbing);
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
