using UnityEngine;
using System;
using System.Collections.Generic;

namespace Common.FSM
{
	///<summary>
	/// This is the main engine of our FSM, without this, you won't be
	/// able to use FSM States and FSM Actions.
	///</summary>
	public class FSM
	{
		//tên của FSM này ? :))
		private readonly string name;
		public  FSMState currentState;
		//con này nhằm chống add 2 state giống nhau vào hệ thống FSM
		private readonly Dictionary< int, FSMState> stateMap;

		public string Name {
			get {
				return name;
			}
		}

		private delegate void StateActionProcessor (FSMAction action);

		/// <summary>
		/// This gets all the actions that is inside the state and loop them.
		/// </summary>
		/// <param name="state">State.</param>
		/// <param name="actionProcessor">Action processor.</param>\
		/// con hàm này được gọi liên tục trong update
		private void ProcessStateAction (FSMState state, StateActionProcessor actionProcessor)
		{
			FSMState currentStateOnInvoke = this.currentState;
			//mỗi 1 state thì có các action(lob, in, end) bên trong 
			List<FSMAction> actions = state.GetActions ();
			//con này chỉ có 1 action tại sao cần dùng list?
			//foreach (FSMAction action in actions)
			int countAction=0;
            for(int i = 0; i < actions.Count; i++)
            {
	            //Debug.Log("INVOKE ACTION		"+ actions[i]+"		"+ ++countAction); 
	            //nếu đang invoke trên state này mà tự dưng bị đổi sang state khác thì sao?
				if (this.currentState != currentStateOnInvoke)
				{
					Debug.Log("TAO ĐÃ BỊ BREAK Ở ĐÂY NHA CÁC CHÁU : "); 
					break;
				}
				//invoke các con action lobby ingame endgame ở đây mỗi đối tượng action bên trong đều có(OnEnter, OnUpdate, OnExit)
				actionProcessor(actions[i]); 	//Nó là cái gì còn phụ thuộc vào nơi gọi nó
				//hàm anomyous ->  OnEnter
				//lob in end -> Start
				//lob in end -> Update
				//lob in end -> LastUpdate
			}
		}

		public FSMState AddState (int name)
		{
			if (stateMap.ContainsKey (name)) {
				Debug.LogWarning ("The FSM already contains: " + name);
				return null;
			}

			FSMState newState = new FSMState (name, this);
			stateMap [name] = newState;
			return newState;
		}

		///<summary>
		/// This is the constructor that will initialize the FSM and give it
		/// a unique name or id.
		///</summary>
		public FSM (string name)
		{
			this.name = name;
			this.currentState = null;
			stateMap = new Dictionary<int, FSMState> ();
		}

		///<summary>
		/// This initializes the FSM. We can indicate the starting State of
		/// the Object that has an FSM.
		///</summary>
		public void Start (int stateId)
		{
			if (!stateMap.ContainsKey (stateId)) {
				Debug.LogWarning ("The FSM doesn't contain: " + stateId);
				return;
			}

			ChangeToState (stateMap [stateId]);
		}

		///<summary>
		/// This changes the state of the Object. This also calls the exit
		/// state before doing the next state.
		///</summary>
		public void ChangeToState (FSMState state)
		{
            //Debug.Log("Change From State--"+ currentState?.ToString()+ "--To--" + state?.ToString()); 
			//current cũ, nếu mà start game thì tất nhiên chưa có state nào rồi nó sẽ k chạy exist
			if (this.currentState != null) {
				
				ExitState (this.currentState);
                //Debug.Log(" CHẠY EXIT: ");
			}

			
			//current mới
			this.currentState = state;
			EnterState (this.currentState);
			//Debug.Log(" CHẠY ENTER : "); 
		}

		///<summary>
		/// This changes the state of the Object. It is not advisable to
		/// call this to change state.
		///</summary>
		public void EnterState (FSMState state)
		{
			ProcessStateAction (state, delegate(FSMAction action) {
				action.OnEnter ();	
			});
		}

		private void ExitState (FSMState state)
		{
			FSMState currentStateOnInvoke = this.currentState;

			ProcessStateAction (state, delegate(FSMAction action) {

				if (this.currentState != currentStateOnInvoke)
					Debug.LogError ("State cannont be changed on exit of the specified state");

				action.OnExit ();	
			});
		}

		///<summary>
		/// Call this under a MonoBehaviour's Update.
		///</summary>
		public void Update ()
		{
			if (this.currentState == null)
				return;
			//delegate(FSMAction action) {action.OnUpdate ()};
			//ngang với (FSMAction action) => {action.OnUpdate ()}  1 hàm anynomous
			//tức là gọi hàm này thì ta truyền vào tham số   là   1 hàm
			ProcessStateAction (this.currentState, delegate(FSMAction action) {
				//tóm lại là action này được invoke mà action này lại gọi OnUpdate
				action.OnUpdate ();	
			});
		}

		public void LateUpdate()
		{
			if (this.currentState == null)
				return;

			ProcessStateAction(this.currentState, delegate (FSMAction action) {
				action.OnLateUpdate();
			});
		}

		public void FixedUpdate()
        {
            if (this.currentState == null)
                return;

            ProcessStateAction(this.currentState, delegate (FSMAction action) {
                action.OnFixedUpdate();
            });
        }

		///<summary>
		/// This handles the events that is bound to a state and changes
		/// the state.
		///</summary>
		public void SendEvent (int eventId)
		{
			FSMState transitonState = ResolveTransition (eventId);

			if (transitonState == null)
				Debug.LogWarning ($"The current state {currentState.GetId()} of FSM {this.name} has no transition for event " + eventId);
			else
				ChangeToState (transitonState);
			
		}

	    public int GetCurrentState()
	    {
	        return currentState.GetId();
	    }
		/// <summary>
		/// This gets the next state from the current state.
		/// </summary>
		/// <returns>The transition.</returns>
		/// <param name="eventId">Event identifier.</param>
		private FSMState ResolveTransition (int eventId)
		{
			FSMState transitionState = this.currentState.GetTransition (eventId);

			if (transitionState == null)
				return null;
			else
				return transitionState;
		}
	}
}
