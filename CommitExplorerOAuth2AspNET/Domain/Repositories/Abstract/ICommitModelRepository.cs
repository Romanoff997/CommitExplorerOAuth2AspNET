using CommitExplorerOAuth2AspNET.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitExplorerOAuth2AspNET.Domain.Repositories.Abstract
{
    public interface ICommitModelRepository
    {
        public Task<IEnumerable<CommitModel>> GetListAsync();
        public Task<CommitModel> CreateAsync(CommitModel client);
        public Task<CommitModel> GetAsync(Guid id);
        public Task UpdateAsync(CommitModel client);
        public Task DeteleAsync(Guid id);
    }
}
