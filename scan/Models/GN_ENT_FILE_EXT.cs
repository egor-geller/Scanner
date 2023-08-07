using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using static System.Collections.Specialized.BitVector32;

namespace Scanner.Models;

//[Keyless]
[PrimaryKey("GN_MODULE_ID", "ENTITY_CODE", "ENTITY_KEY")]
[Table("GN_ENT_FILE_EXT", Schema = "dbo")]
public partial class GN_ENT_FILE_EXT
{
    [Key]
   // [DatabaseGenerated(DatabaseGeneratedOption.None)]
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

    [StringLength(60)]
    [Unicode(false)]
    public string? TEUR1 { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string? FILE_TYPE_CD { get; set; }

    public int? KEY1 { get; set; }

    public int? KEY2 { get; set; }

    public int? KEY3 { get; set; }

    public int? KEY4 { get; set; }

    public int? KEY5 { get; set; }

    [StringLength(36)]
    [Unicode(false)]
    public string? TBH_DOC_UID { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string? TBH_MAARECHET { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string? TBH_MODOULE { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string? TBH_KOD_TAHALICH { get; set; }

    public int? TBH_SHALAV { get; set; }

    [StringLength(60)]
    [Unicode(false)]
    public string? TEUR2 { get; set; }

    [StringLength(60)]
    [Unicode(false)]
    public string? TEUR3 { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string? ATT1 { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string? ATT2 { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string? ATT3 { get; set; }

    [StringLength(78)]
    [Unicode(false)]
    public string? FILE_NAME { get; set; }

    public override string? ToString()
    {
        return
            "GN_MODULE_ID: " + GN_MODULE_ID +
            ", ENTITY_CODE: " + ENTITY_CODE +
            ", ENTITY_KEY: " + ENTITY_KEY +
            ", FILE_NAME: " + FILE_NAME +
            ", TEUR1: " + TEUR1 +
            ", TEUR2: " + TEUR2 +
            ", TEUR3: " + TEUR3 +
            ", FILE_TYPE_CD: " + FILE_TYPE_CD +
            ", KEY1: " + KEY1 +
            ", KEY2: " + KEY2 +
            ", KEY3: " + KEY3 +
            ", KEY4: " + KEY4 +
            ", KEY5: " + KEY5 +
            ", TBH_DOC_UID: " + TBH_DOC_UID +
            ", TBH_MAARECHET: " + TBH_MAARECHET +
            ", TBH_MODOULE: " + TBH_MODOULE +
            ", TBH_KOD_TAHALICH: " + TBH_KOD_TAHALICH +
            ", TBH_SHALAV: " + TBH_SHALAV +
            ", ATT1: " + ATT1 +
            ", ATT2: " + ATT2 +
            ", ATT3: " + ATT3 +
            ", FILE_NAME: " + FILE_NAME;
    }
}
