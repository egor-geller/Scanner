using EntityFramework.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using NLog;
using Scanner.Models;
using System.Data.Entity.Core;

namespace Scanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GnEntityFilesController : Controller
    {
        //private readonly ApplicationDbContext _dbcontext;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public GnEntityFilesController()//ApplicationDbContext dbcontext)
        {
            //_dbcontext = dbcontext;
        }

        //get{id}
        [HttpGet("{EntityKey}")]
        public async Task<ActionResult<GN_ENTITY_FILE>> GetGnEntityFile(string GnModule, string EntityCode, string EntityKey, string FileName)
        {
            /* if (_dbcontext.GN_ENTITY_FILEs == null)
             {
                 return NotFound();
             }
             var gnEntityFiles = await _dbcontext.GN_ENTITY_FILEs.FindAsync(GnModule, EntityCode, EntityKey, FileName);
             if (gnEntityFiles == null)
             {
                 return NotFound();
             }
             return gnEntityFiles;*/
            return StatusCode(405, "Not supported");
        }

        [HttpPost]
        public async Task<ActionResult<GN_ENTITY_FILE>> PostGnEntityFiles(GN_ENTITY_FILE gnEntityFiles)
        {
            try
            {
                var directory = new DirectoryInfo(gnEntityFiles.DEST_PATH);
                var sourcePath = gnEntityFiles.FILE_PATH + gnEntityFiles.FILE_NAME;
                var targetPath = gnEntityFiles.DEST_PATH + gnEntityFiles.FILE_NAME;

                logger.Info("Scanner:PostGnEntityFiles::sourcePath:" + sourcePath); // C:\\temp\\Srika - 9998 - 2023 - 00006 - 6.PNG
                logger.Info("Scanner:PostGnEntityFiles::targetPath:" + targetPath); // \\NAS01\CHOSHEN\PPR\TAKTZIV_DIGIT\Srika-9998-2023-00006-6.PNG

                if (!System.IO.File.Exists(sourcePath))
                {
                    logger.Error($"Scanner:PostGnEntityFiles: File does not exists in {sourcePath}");
                    return NotFound("לא נמצא קובץ בניתוב המבוקש");
                }

                if (!String.IsNullOrEmpty(gnEntityFiles.DEST_PATH) && !Directory.Exists(Path.GetDirectoryName(gnEntityFiles.DEST_PATH)))
                {
                    Directory.CreateDirectory(gnEntityFiles.DEST_PATH);
                    logger.Info($"Scanner:PostGnEntityFiles: Created new directory to save new file {gnEntityFiles.DEST_PATH}");
                }

                System.IO.File.Copy(sourcePath, targetPath, true);
                logger.Info($"Scanner:PostGnEntityFiles: File {gnEntityFiles.FILE_NAME} has been copied to {gnEntityFiles.DEST_PATH} successfully");

                // Delete file from temp folder
                foreach (var file in directory.GetFiles())
                {
                    if (file.FullName.Equals(sourcePath))
                    {
                        System.IO.File.Delete(file.FullName);
                    }
                }
                logger.Info($"Scanner:PostGnEntityFiles: File {gnEntityFiles.FILE_NAME} has been deleted from {gnEntityFiles.FILE_PATH} successfully");

                gnEntityFiles.FILE_PATH = gnEntityFiles.DEST_PATH;
                logger.Info($"DATA: {gnEntityFiles}");
                //var gnEntityFileExt = SetGnEntFileExt(gnEntityFiles);
                var _dbcontext = new ApplicationDbContext(gnEntityFiles.ENVIRONMENT);
                _dbcontext.GN_ENTITY_FILEs.Add(gnEntityFiles);
                await _dbcontext.SaveChangesAsync();
                SetGnEntFileExt(gnEntityFiles, _dbcontext.GetConnString);
                string newTeur = GetTeurFromFileExt(gnEntityFiles, _dbcontext.GetConnString);
                gnEntityFiles.TEUR1 = newTeur;
                SetGnEntFileExt(gnEntityFiles, _dbcontext.GetConnString);
                return CreatedAtAction(nameof(GetGnEntityFile), new { GnModule = gnEntityFiles.GN_MODULE_ID, EntityCode = gnEntityFiles.ENTITY_CODE, EntityKey = gnEntityFiles.ENTITY_KEY, FileName = gnEntityFiles.FILE_NAME }, gnEntityFiles);
            }
            catch (Exception ex)
            {
                logger.Error($"Scanner:PostGnEntityFiles: ERROR: {ex.Message}, INNER: {ex.InnerException}");
                return BadRequest("קיימת שגיאה באחד מהתהליכים");
            }
        }

        private static string GetTeurFromFileExt(GN_ENTITY_FILE gnEntityFiles, string? conn)
        {
            string selectTeurSql =
                "SELECT [TEUR1] " +
                "FROM [dbo].[GN_ENT_FILE_EXT] " +
                "WHERE GN_MODULE_ID = @DbId AND ENTITY_CODE = @ObjectType AND ENTITY_KEY = @EntityKey";

            string? DbId = gnEntityFiles.GN_MODULE_ID;
            string? ObjectType = gnEntityFiles.ENTITY_CODE;
            string? EntityKey = gnEntityFiles.ENTITY_KEY;
            string? teur = gnEntityFiles.TEUR1;

            logger.Info($"SetGnEntFileExt::Connctiong string: {conn}");

            if (String.IsNullOrEmpty(DbId) ||
                String.IsNullOrEmpty(ObjectType) ||
                String.IsNullOrEmpty(EntityKey) ||
                String.IsNullOrEmpty(teur) ||
                String.IsNullOrEmpty(conn))
            {
                logger.Warn($"SetGnEntFileExt::Connection params are empty");
                return string.Empty;
            }

            using (var connection = new SqlConnection(conn))
            {
                using (var command = new SqlCommand(selectTeurSql, connection))
                {
                    command.Parameters.AddWithValue("@DbId", DbId);
                    command.Parameters.AddWithValue("@ObjectType", ObjectType);
                    command.Parameters.AddWithValue("@EntityKey", EntityKey);
                    command.Parameters.AddWithValue("@teur", teur);
                    logger.Info($"SetGnEntFileExt::Connection params: {command.Parameters}");

                    // Open the connection in a try/catch block.
                    // Create and execute the DataReader, writing the result
                    // set to the console window.
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                return reader.GetString(0);
                            }
                        }
                        else
                        {
                            logger.Warn($"TAUpload:DeleteAllFiles: No rows were deleted");
                        }
                        reader.Close();
                        logger.Info($"SetGnEntFileExt::SaveFileToDB: Connection closed, Teur {teur}");
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"SetGnEntFileExt::Connection params:ERROR {ex.Message}, {ex.InnerException}");
                    }
                }
            }
            return string.Empty;
        }

        private static void SetGnEntFileExt(GN_ENTITY_FILE gnEntityFiles, string? conn)
        {
            logger.Info($"GN_ENT_FILE_EXT: {gnEntityFiles}");

            string updateSugAndTeur =
            "UPDATE [dbo].[GN_ENT_FILE_EXT]" +
            "SET [TEUR1] = @teur, [FILE_TYPE_CD] = @fileTypeCd " +
            "WHERE GN_MODULE_ID = @DbId AND ENTITY_CODE = @ObjectType AND ENTITY_KEY = @EntityKey";

            string? DbId = gnEntityFiles.GN_MODULE_ID;
            string? ObjectType = gnEntityFiles.ENTITY_CODE;
            string? EntityKey = gnEntityFiles.ENTITY_KEY;
            string? teur = gnEntityFiles.TEUR1;
            string? fileTypeCd = gnEntityFiles.FILE_TYPE_CD;

            logger.Info($"SetGnEntFileExt::Connctiong string: {conn}");

            if (String.IsNullOrEmpty(DbId) ||
                String.IsNullOrEmpty(ObjectType) ||
                String.IsNullOrEmpty(EntityKey) ||
                String.IsNullOrEmpty(teur) ||
                String.IsNullOrEmpty(fileTypeCd) ||
                String.IsNullOrEmpty(conn))
            {
                logger.Warn($"SetGnEntFileExt::Connection params are empty");
                return;
            }

            using (var connection = new SqlConnection(conn))
            {
                using (var command = new SqlCommand(updateSugAndTeur, connection))
                {
                    command.Parameters.AddWithValue("@DbId", DbId);
                    command.Parameters.AddWithValue("@ObjectType", ObjectType);
                    command.Parameters.AddWithValue("@EntityKey", EntityKey);
                    command.Parameters.AddWithValue("@teur", teur);
                    command.Parameters.AddWithValue("@fileTypeCd", fileTypeCd);
                    logger.Info($"SetGnEntFileExt::Connection params: {command.Parameters}");

                    // Open the connection in a try/catch block.
                    // Create and execute the DataReader, writing the result
                    // set to the console window.
                    try
                    {
                        connection.Open();
                        var reader = command.ExecuteNonQuery();
                        if (reader == 0)
                        {
                            logger.Warn($"TAUpload:UpdateTeurAndFileType: No rows were updated");
                        }
                        logger.Info($"SetGnEntFileExt::SaveFileToDB: Connection closed, Teur {teur} and fileTypeCd {fileTypeCd} wrote to DB");
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"SetGnEntFileExt::Connection params:ERROR {ex.Message}, {ex.InnerException}");
                    }
                }
            }
        }
    }
}
