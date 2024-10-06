using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndsertDB.Model
{
    using Microsoft.EntityFrameworkCore;

    namespace IndsertDB.Model
    {
        public class ImdbDbContext : DbContext
        {
            public DbSet<People> Peoples { get; set; }
            public DbSet<Movie> Movies { get; set; }
            public DbSet<Profession> Professions { get; set; }
            public DbSet<PersonProfession> PersonProfessions { get; set; }
            public DbSet<KnownFor> KnownFors { get; set; }
            public DbSet<Genre> Genres { get; set; }
            public DbSet<MovieGenre> MovieGenres { get; set; }
            public DbSet<MovieDirector> MovieDirectors { get; set; }
            public DbSet<MovieWriter> MovieWriters { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                if (!optionsBuilder.IsConfigured)
                {
                    optionsBuilder.UseSqlServer("Data Source=DESKTOP-JLQ17VU;Integrated Security=True;Database=xx;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
                }
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                // People: 主键
                modelBuilder.Entity<People>()
                    .HasKey(p => p.Nconst);

                // PersonProfessions: 联合主键
                modelBuilder.Entity<PersonProfession>()
                    .HasKey(pp => new { pp.Nconst, pp.ProfessionId });

                modelBuilder.Entity<PersonProfession>()
                    .HasOne(pp => pp.People)
                    .WithMany(p => p.PersonProfessions)
                    .HasForeignKey(pp => pp.Nconst);

                modelBuilder.Entity<PersonProfession>()
                    .HasOne(pp => pp.Professions)
                    .WithMany(p => p.PersonProfessions)
                    .HasForeignKey(pp => pp.ProfessionId);

                // KnownFor: 联合主键
                modelBuilder.Entity<KnownFor>()
                    .HasKey(kf => new { kf.Nconst, kf.Tconst });

                modelBuilder.Entity<KnownFor>()
                    .HasOne(kf => kf.People)
                    .WithMany(p => p.KnownForTitles)
                    .HasForeignKey(kf => kf.Nconst);

                modelBuilder.Entity<KnownFor>()
                    .HasOne(kf => kf.Movies)
                    .WithMany(m => m.KnownForTitles)
                    .HasForeignKey(kf => kf.Tconst);

                // MovieGenre: 联合主键
                modelBuilder.Entity<MovieGenre>()
                    .HasKey(mg => new { mg.Tconst, mg.GenreId });

                modelBuilder.Entity<MovieGenre>()
                    .HasOne(mg => mg.Movies)
                    .WithMany(m => m.MovieGenres)
                    .HasForeignKey(mg => mg.Tconst);

                modelBuilder.Entity<MovieGenre>()
                    .HasOne(mg => mg.Genres)
                    .WithMany(g => g.MovieGenres)
                    .HasForeignKey(mg => mg.GenreId);

                // MovieDirector: 联合主键
                modelBuilder.Entity<MovieDirector>()
                    .HasKey(md => new { md.Tconst, md.Nconst });

                modelBuilder.Entity<MovieDirector>()
                    .HasOne(md => md.Movies)
                    .WithMany(m => m.MovieDirectors)
                    .HasForeignKey(md => md.Tconst);

                modelBuilder.Entity<MovieDirector>()
                    .HasOne(md => md.People)
                    .WithMany(p => p.MovieDirectors)
                    .HasForeignKey(md => md.Nconst);

                // MovieWriter: 联合主键
                modelBuilder.Entity<MovieWriter>()
                    .HasKey(mw => new { mw.Tconst, mw.Nconst });

                modelBuilder.Entity<MovieWriter>()
                    .HasOne(mw => mw.Movies)
                    .WithMany(m => m.MovieWriters)
                    .HasForeignKey(mw => mw.Tconst);

                modelBuilder.Entity<MovieWriter>()
                    .HasOne(mw => mw.People)
                    .WithMany(p => p.MovieWriters)
                    .HasForeignKey(mw => mw.Nconst);
            }
        }
    }


}
