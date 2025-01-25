

public interface IStateHanlder
{
	public void SetSystem(FSMSystem fsm);
	public void Enter(FSMSystem fsm);
	public void _Update(FSMSystem fsm);
	public void Exit(FSMSystem fsm);
}
