using System;
using System.Collections.Generic;

namespace Toolbox.SM
{
	public class StateMachine
	{
		private StateNode _current;
		private readonly Dictionary<Type, StateNode> _nodesDictionary = new();
		private readonly HashSet<Transition> _anyTransitionsSet = new();

		public void SetState(IState state)
		{
			_current = _nodesDictionary[state.GetType()];
			_current.State?.OnEnter();
		}

		public void Update()
		{
			var transition = GetTransition();

			if (transition != null)
			{
				ChangeState(transition.To);
			}

			_current.State?.Update();
		}

		public void FixedUpdate()
		{
			_current.State?.FixedUpdate();
		}

		private ITransition GetTransition()
		{
			foreach (var transition in _anyTransitionsSet)
			{
				if (transition.Condition.Evaluate())
				{
					return transition;
				}
			}

			foreach (var transition in _current.TransitionsSet)
			{
				if (transition.Condition.Evaluate())
				{
					return transition;
				}
			}

			return null;
		}

		private void ChangeState(IState state)
		{
			if (state == _current.State) return;

			var previousState = _current.State;
			var nextState = _nodesDictionary[state.GetType()].State;

			previousState?.OnExit();
			nextState?.OnEnter();

			_current = _nodesDictionary[state.GetType()];
		}

		public void AddTransition(IState from, IState to, IPredicate condition)
		{
			GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
		}

		public void AddAnyTransition(IState to, IPredicate condition)
		{
			_anyTransitionsSet.Add(new Transition(GetOrAddNode(to).State, condition));
		}

		private StateNode GetOrAddNode(IState state)
		{
			var node = _nodesDictionary.GetValueOrDefault(state.GetType());

			if (node == null)
			{
				node = new StateNode(state);
				_nodesDictionary.Add(state.GetType(), node);
			}

			return node;
		}

		private class StateNode
		{
			public IState State { get; }
			public HashSet<ITransition> TransitionsSet { get; }

			public StateNode(IState state)
			{
				State = state;
				TransitionsSet = new HashSet<ITransition>();
			}

			public void AddTransition(IState to, IPredicate condition)
			{
				TransitionsSet.Add(new Transition(to, condition));
			}
		}
	}
}