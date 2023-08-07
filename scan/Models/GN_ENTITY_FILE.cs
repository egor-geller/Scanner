using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Scanner.Models;

//[PrimaryKey("GN_MODULE_ID", "ENTITY_CODE", "ENTITY_KEY", "FILE_NAME")]
[Table("GN_ENTITY_FILES", Schema = "dbo")]
public partial class GN_ENTITY_FILE
{
    [Key]
    [StringLength(2)]
    [Unicode(false)]
    public string GN_MODULE_ID { get; set; } = null!;

    [Key]
    [StringLength(3)]
    [Unicode(false)]
    public string ENTITY_CODE { get; set; } = null!;

    [Key]
    [StringLength(78)]
    [Unicode(false)]
    public string ENTITY_KEY { get; set; } = null!;

    [Key]
    [StringLength(78)]
    [Unicode(false)]
    public string FILE_NAME { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string FILE_PATH { get; set; } = null!;

    [StringLength(8)]
    [Unicode(false)]
    public string USER_ID_NAME { get; set; } = null!;

    [StringLength(32)]
    [Unicode(false)]
    public string UPLOAD_TIME { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string ARCHIVE { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string? ACTION { get; set; }

    [StringLength(8)]
    [Unicode(false)]
    public string? HELD_BY { get; set; }

    public int? MS_SAPAK { get; set; }

    [Column(TypeName = "decimal(4, 0)")]
    public decimal? SHANA_TAK_CHESH { get; set; }

    public int? MS_CHESHBONIT_SP { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string? OFI_CHESHBONIT { get; set; }

    [NotMapped]
    [StringLength(500)]
    [Unicode(false)]
    public string DEST_PATH { get; set; } = string.Empty;

    [NotMapped]
    public string ENVIRONMENT { get; set; } = string.Empty;

    [NotMapped]
    public string? TEUR1 { get; set; }

    [NotMapped]
    public string? FILE_TYPE_CD { get; set; }

    public override string? ToString()
    {
        return
            "GN_MODULE_ID: " + GN_MODULE_ID +
            ", ENTITY_CODE: " + ENTITY_CODE +
            ", ENTITY_KEY: " + ENTITY_KEY +
            ", FILE_NAME: " + FILE_NAME +
            ", FILE_PATH: " + FILE_PATH +
            ", USER_ID_NAME: " + USER_ID_NAME +
            ", UPLOAD_TIME: " + UPLOAD_TIME +
            ", ARCHIVE: " + ARCHIVE +
            ", ACTION: " + ACTION +
            ", HELD_BY: " + HELD_BY +
            ", MS_SAPAK: " + MS_SAPAK +
            ", SHANA_TAK_CHESH: " + SHANA_TAK_CHESH +
            ", MS_CHESHBONIT_SP: " + MS_CHESHBONIT_SP +
            ", OFI_CHESHBONIT: " + OFI_CHESHBONIT +
            ", DEST_PATH: " + DEST_PATH +
            ", ENVIRONMENT: " + ENVIRONMENT +
            ", TEUR1: " + TEUR1 +
            ", FILE_TYPE_CD: " + FILE_TYPE_CD;
    }
}
