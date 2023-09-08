using Octokit;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace CommitExplorerOAuth2AspNET.Domain.Entities
{
    public class GitUser
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<GitRepository> GitRepositories { get; set; }
    }

    public class GitRepository
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? GitUserId { get; set; }
        public virtual GitUser GitUser { get; set; }
        public virtual ICollection<GitCommit> GitCommits { get; set; }
    }

    public class GitCommit
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid? GitRepositoryId { get; set; }
        public string author { get; set; }
        public string sha { get; set; }
        public virtual GitRepository GitRepository { get; set; }
        public string json { get; set; }
    }
}
