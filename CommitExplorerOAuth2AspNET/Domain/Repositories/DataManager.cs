using CommitExplorerOAuth2AspNET.Domain.Repositories.Abstract;

namespace CommitExplorerOAuth2AspNET.Domain.Repositories
{
    public class DataManager
    {
        public ICommitModelRepository CommitRepository { get; set; }

        public DataManager(ICommitModelRepository commitRepository)
        {
            CommitRepository = commitRepository;
        }
    }

}
