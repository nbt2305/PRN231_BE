using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<JobPosition> JobPositions { get; set; }
        public DbSet<DepartmentJobPosition> DepartmentJobPositions { get; set; }
        public DbSet<CommonCode> CommonCodes { get; set; }
        public DbSet<LogWork> LogWorks { get; set; }
        public DbSet<Phase> Phases { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectPhase> ProjectPhases { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }


        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {
        }

    }
}
