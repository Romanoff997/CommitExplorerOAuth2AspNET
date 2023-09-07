using AutoMapper.QueryableExtensions;
using AutoMapper;
using CommitExplorerOAuth2AspNET.Domain.Entities;
using Octokit;
using CommitExplorerOAuth2AspNET.Interface;

namespace CommitExplorerOAuth2AspNET.Services
{
    public class MappingServiceNative : IMapingService
    {
        private readonly IMapper _mapper;
        public MappingServiceNative(IMapper mapper)
        {
            _mapper = mapper;

        }

        public IQueryable<GitCommit> GetCommitsEntity(IQueryable<GitHubCommit> commitsView)
        {
            return commitsView.ProjectTo<GitCommit>(_mapper.ConfigurationProvider);
        }
    }
}
