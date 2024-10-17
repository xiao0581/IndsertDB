using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
             public DbSet<GetByIdView> GetByIdView { get; set; }
        public ImdbDbContext()
        {
        }
        public ImdbDbContext(DbContextOptions<ImdbDbContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                if (!optionsBuilder.IsConfigured)
                {
                    optionsBuilder.UseSqlServer("Data Source=XIAO-PC\\XIAODATA;Integrated Security=True;Database=vv;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
                }
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                // People table mapping
                modelBuilder.Entity<People>(entity =>
                {
                    entity.ToTable("Peoples");
                    entity.HasKey(p => p.Nconst);
                    entity.Property(p => p.Nconst).HasColumnName("nconst");
                    entity.Property(p => p.PrimaryName).HasColumnName("primaryName");
                    entity.Property(p => p.BirthYear).HasColumnName("birthYear");
                    entity.Property(p => p.DeathYear).HasColumnName("deathYear");
                });

                // Movies table mapping
                modelBuilder.Entity<Movie>(entity =>
                {
                    entity.ToTable("Movies");
                    entity.HasKey(m => m.Tconst);
                    entity.Property(m => m.Tconst).HasColumnName("tconst");
                    entity.Property(m => m.TitleType).HasColumnName("titleType");
                    entity.Property(m => m.PrimaryTitle).HasColumnName("primaryTitle");
                    entity.Property(m => m.OriginalTitle).HasColumnName("originalTitle");
                    entity.Property(m => m.IsAdult).HasColumnName("isAdult");
                    entity.Property(m => m.StartYear).HasColumnName("startYear");
                    entity.Property(m => m.EndYear).HasColumnName("endYear");
                    entity.Property(m => m.RuntimeMinutes).HasColumnName("runtimeMinutes");
                });

                // Professions table mapping
                modelBuilder.Entity<Profession>(entity =>
                {
                    entity.ToTable("Professions");
                    entity.HasKey(p => p.ProfessionId);
                    entity.Property(p => p.ProfessionId).HasColumnName("professionId");
                    entity.Property(p => p.ProfessionName).HasColumnName("professionName");
                });

                // PersonProfessions table mapping
                modelBuilder.Entity<PersonProfession>(entity =>
                {
                    entity.ToTable("PersonProfessions");

                    entity.HasKey(pp => new { pp.Nconst, pp.ProfessionId });

                    entity.HasOne(pp => pp.People)
                          .WithMany(p => p.PersonProfessions)
                          .HasForeignKey(pp => pp.Nconst);

                    entity.HasOne(pp => pp.Profession)
                          .WithMany(p => p.PersonProfessions)
                          .HasForeignKey(pp => pp.ProfessionId);
                });

                // KnownFor table mapping
                modelBuilder.Entity<KnownFor>(entity =>
                {
                    entity.ToTable("KnownFors");
                    entity.HasKey(kf => new { kf.Nconst, kf.Tconst });
                    entity.HasOne(kf => kf.People)
                          .WithMany(p => p.KnownForTitles)
                          .HasForeignKey(kf => kf.Nconst);
                    entity.HasOne(kf => kf.Movie)
                          .WithMany(m => m.KnownForTitles)
                          .HasForeignKey(kf => kf.Tconst);
                });

                // Genres table mapping
                modelBuilder.Entity<Genre>(entity =>
                {
                    entity.ToTable("Genres");
                    entity.HasKey(g => g.GenreId);
                    entity.Property(g => g.GenreId).HasColumnName("genreId");
                    entity.Property(g => g.GenreName).HasColumnName("genreName");
                });

                // MovieGenres table mapping
                modelBuilder.Entity<MovieGenre>(entity =>
                {
                    entity.ToTable("MovieGenres");
                    entity.HasKey(mg => new { mg.Tconst, mg.GenreId });
                    entity.HasOne(mg => mg.Movie)
                          .WithMany(m => m.MovieGenres)
                          .HasForeignKey(mg => mg.Tconst);
                    entity.HasOne(mg => mg.Genre)
                          .WithMany(g => g.MovieGenres)
                          .HasForeignKey(mg => mg.GenreId);
                });

                // MovieDirectors table mapping
                modelBuilder.Entity<MovieDirector>(entity =>
                {
                    entity.ToTable("MovieDirectors");
                    entity.HasKey(md => new { md.Tconst, md.Nconst });
                    entity.HasOne(md => md.Movie)
                          .WithMany(m => m.MovieDirectors)
                          .HasForeignKey(md => md.Tconst);
                    entity.HasOne(md => md.People)
                          .WithMany(p => p.MovieDirectors)
                          .HasForeignKey(md => md.Nconst);
                });

                // MovieWriters table mapping
                modelBuilder.Entity<MovieWriter>(entity =>
                {
                    entity.ToTable("MovieWriters");
                    entity.HasKey(mw => new { mw.Tconst, mw.Nconst });
                    entity.HasOne(mw => mw.Movie)
                          .WithMany(m => m.MovieWriters)
                          .HasForeignKey(mw => mw.Tconst);
                    entity.HasOne(mw => mw.People)
                          .WithMany(p => p.MovieWriters)
                          .HasForeignKey(mw => mw.Nconst);
                });

            // GetByIdView table mapping
            modelBuilder
            .Entity<GetByIdView>()
            .ToView("GetById") // 
            .HasNoKey();
        }


        }
   }
