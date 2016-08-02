namespace Teclyn.AspNetMvc.Models
{
    public interface IListModel
    {
        string PreviousLink { get; }
        string NextLink { get; }
        int PreviousMin { get; }
        int PreviousMax { get; }
        int NextMin { get; }
        int NextMax { get; }
        int Total { get; }
    }
}