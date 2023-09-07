using AutoMapper;
using CommitExplorerOAuth2AspNET.Domain.Entities;
using Octokit;

namespace CommitExplorerOAuth2AspNET.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // CreateRequest -> User
            CreateMap<GitHubCommit, GitCommit>();
        }
    }
}
