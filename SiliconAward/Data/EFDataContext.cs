using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SiliconAward.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.Data
{
    public class EFDataContext:DbContext
    {
        public EFDataContext(DbContextOptions<EFDataContext> options)
            : base(options)
        {
        }

        public EFDataContext()
        {
        }

        
        public DbSet<Status> Statues { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketDetails> TicketDetails { get; set; }
        public DbSet<CompetitionBranch> CompetitionBranchs { get; set; }
        public DbSet<CompetitionField> CompetitionFields { get; set; }
        public DbSet<CompetitionSubject> CompetitionSubjects { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Participant> Participants { get; set; }             
        public DbSet<User> Users { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"data source=(local); initial catalog=myprogra_10award_db;persist security info=True;user id=ahm604dbuser;pwd=!s78c4xO");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Status>()
            //    .HasMany<Participant>(g => g.Participants)
            //    .WithOne(s => s.Status)
            //    .HasForeignKey(s => s.StatusId)
            //    .OnDelete(DeleteBehavior.SetNull);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
