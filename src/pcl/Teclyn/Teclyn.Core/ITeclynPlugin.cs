namespace Teclyn.Core
{
    public interface ITeclynPlugin
    {
        string Name { get; }
        void Initialize(TeclynApi teclyn);
    }
}