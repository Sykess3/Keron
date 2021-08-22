namespace CodeBase.Logic
{
    public interface IAnimatorStateReader
    {
        void EnteredState(int hash);
        void ExitedState(int hash);
    
        AnimatorState State { get; }
    }
}