using redd096.v2.ComponentsSystem;

/// <summary>
/// This is used only to have DeskStateMachine on every state
/// </summary>
public abstract class DeskBaseState : State
{
    protected DeskStateMachine deskStateMachine;

    protected override void OnInit()
    {
        base.OnInit();

        deskStateMachine = StateMachine as DeskStateMachine;
    }
}
