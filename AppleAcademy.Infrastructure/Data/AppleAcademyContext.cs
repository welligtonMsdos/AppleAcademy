using AppleAcademy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppleAcademy.Infrastructure.Data;

public class AppleAcademyContext:DbContext
{
    public static readonly Guid AdminPerfilGuid = Guid.Parse("9ddf8627-87fa-4f38-86e1-9c5f5530ed98");
    public static readonly Guid AlunoPerfilGuid = Guid.Parse("dee0f9e9-becc-47a4-8aa6-db09ac1c394d");
    public static readonly Guid ProfessorPerfilGuid = Guid.Parse("8ba1ccbe-85a5-4eea-b6f0-51006fd1091f");
    public DbSet<Status> Statuses => Set<Status>();
    public DbSet<Perfil> Perfis => Set<Perfil>();
    public DbSet<User> Users => Set<User>();

    public AppleAcademyContext(DbContextOptions<AppleAcademyContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var status = modelBuilder.Entity<Status>();
        status.ToTable(nameof(Status));
        status.HasKey(item => item.Guid);
        status.Property(item => item.Description).HasMaxLength(50).IsRequired();
        status.Property(item => item.Active).IsRequired();

        var perfil = modelBuilder.Entity<Perfil>();
        perfil.ToTable(nameof(Perfil));
        perfil.HasKey(item => item.Guid);
        perfil.Property(item => item.Name).HasMaxLength(50).IsRequired();
        perfil.HasData(new Perfil { Guid = AdminPerfilGuid, Name = "Admin", Active = true });

        var user = modelBuilder.Entity<User>();
        user.ToTable(nameof(User));
        user.HasKey(item => item.Guid);
        user.Property(item => item.Name).HasMaxLength(150).IsRequired();
        user.Property(item => item.Email).HasMaxLength(150).IsRequired();
        user.Property(item => item.Password).HasMaxLength(100).IsRequired();
        user.Property(item => item.Active).IsRequired();
        user.HasOne(u =>u.Perfil).WithMany(p => p.Users).HasForeignKey(u => u.PerfilGuid);
    }
}
