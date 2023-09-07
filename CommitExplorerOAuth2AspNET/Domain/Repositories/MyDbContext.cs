using CommitExplorerOAuth2AspNET.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CommitExplorerOAuth2AspNET.Domain.Repositories
{
    public class MyDbContext : DbContext
    {
        public DbSet<GitUser> UserEntity { get; set; }
        public DbSet<GitRepository> RepoEntity { get; set; }
        public DbSet<GitCommit> CommitEntity { get; set; }
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<GitRepository>()
                .HasOne<GitUser>(d => d.GitUser)
                .WithMany(p => p.GitRepositories)
                .HasForeignKey(d => d.GitUserId);

            modelBuilder.Entity<GitCommit>()
                .HasOne<GitRepository>(d => d.GitRepository)
                .WithMany(p => p.GitCommits)
                .HasForeignKey(d => d.GitRepositoryId);
           

            base.OnModelCreating(modelBuilder);


        }
    }
}
