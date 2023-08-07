using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace Scanner.Models;

public partial class ApplicationDbContext : DbContext
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    private readonly string env = "";
    public string connectionString = "";
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(string _env)
    {
        env = _env;
        string[] server = CalcEnviroment(env);
        connectionString = "Data Source=" + server[0] + ";Initial Catalog=" + server[1] + ";Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;";
        logger.Info($"DbService::connectionString: {connectionString}");

    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<GN_ENTITY_FILE> GN_ENTITY_FILEs { get; set; }

    public virtual DbSet<GN_ENT_FILE_EXT> GN_ENT_FILE_EXTs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseSqlServer("Name=ConnectionStrings:conString");
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("DOM001\\c2147062");

        modelBuilder.Entity<GN_ENTITY_FILE>(entity =>
        {
            //entity.HasKey(e => new { e.GN_MODULE_ID, e.ENTITY_CODE, e.ENTITY_KEY, e.FILE_NAME }).HasName("PK__GN_ENTIT__553E8B692A3CCDD1");

            entity.Property(e => e.GN_MODULE_ID).IsFixedLength();
            entity.Property(e => e.ENTITY_CODE).IsFixedLength();
            entity.Property(e => e.ENTITY_KEY).IsFixedLength();
            entity.Property(e => e.FILE_NAME).IsFixedLength();
            entity.Property(e => e.ACTION).IsFixedLength();
            entity.Property(e => e.ARCHIVE).IsFixedLength();
            entity.Property(e => e.HELD_BY).IsFixedLength();
            entity.Property(e => e.OFI_CHESHBONIT).IsFixedLength();
            entity.Property(e => e.USER_ID_NAME).IsFixedLength();
        });

        modelBuilder.Entity<GN_ENT_FILE_EXT>(entity =>
        {
            entity.Property(e => e.ATT1).IsFixedLength();
            entity.Property(e => e.ATT2).IsFixedLength();
            entity.Property(e => e.ATT3).IsFixedLength();
            entity.Property(e => e.ENTITY_CODE).IsFixedLength();
            entity.Property(e => e.ENTITY_KEY).IsFixedLength();
            entity.Property(e => e.FILE_NAME).IsFixedLength();
            entity.Property(e => e.TEUR1).IsFixedLength();
            entity.Property(e => e.FILE_TYPE_CD).IsFixedLength();
            entity.Property(e => e.GN_MODULE_ID).IsFixedLength();
            entity.Property(e => e.TBH_KOD_TAHALICH).IsFixedLength();
            entity.Property(e => e.TBH_MAARECHET).IsFixedLength();
            entity.Property(e => e.TBH_MODOULE).IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public string? GetConnString { get; }

    private static string[] CalcEnviroment(string serverEnv)
    {
        string[] dbServer = new string[2];
        switch (serverEnv)
        {
            case "TADEV":
                dbServer[0] = "sql08\\devop";
                dbServer[1] = "db917";
                break;
            case "TAHAD":
                dbServer[0] = "sql08\\testop";
                dbServer[1] = "db917";
                break;
            case "TAPPR":
                dbServer[0] = "sql08\\preprodop";
                dbServer[1] = "db917";
                break;
            case "TAEY":
                dbServer[0] = "sql08\\preprodop";
                dbServer[1] = "db917_tst";
                break;
            case "TAPROD":
                dbServer[0] = "SQL09";
                dbServer[1] = "db917";
                break;
            default:
                dbServer[0] = "tamlogfin";
                dbServer[1] = "lgdata";
                break;
        }
        return dbServer;
    }
}
