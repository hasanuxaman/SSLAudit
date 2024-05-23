using Newtonsoft.Json;
using Shampan.Models;
using System.Data;
using System.Security.Claims;

namespace SageERP.ExtensionMethods
{
    public static class IdentityExtensions
    {
        public static string? GetCurrentBranchId(this ClaimsPrincipal principal)
        {
           
            try
            {
                Claim? currentBranchClaim = principal.Claims.FirstOrDefault(x => x.Type == ClaimNames.CurrentBranch);

                return currentBranchClaim != null ? currentBranchClaim.Value : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

       

        public static string? GetUserId(this ClaimsPrincipal principal)
        {

            try
            {
                var userIdClaim = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.UserData);

                return userIdClaim != null ? userIdClaim.Value : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string? DataTableToJson(DataTable dataTable)
        {

            try
            {
                string json = JsonConvert.SerializeObject(dataTable, Formatting.Indented);
                return json;
            }
            catch (Exception)
            {
                return "";
            }
           
        }


        public static string[] Alphabet = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ", "CA", "CB", "CC", "CD", "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ", "DA", "DB", "DC", "DD", "DE", "DF", "DG", "DH", "DI", "DJ", "DK", "DL", "DM", "DN", "DO", "DP", "DQ", "DR", "DS", "DT", "DU", "DV", "DW", "DX", "DY", "DZ", "EA", "EB", "EC", "ED", "EE", "EF", "EG", "EH", "EI", "EJ", "EK", "EL", "EM", "EN", "EO", "EP", "EQ", "ER", "ES", "ET", "EU", "EV", "EW", "EX", "EY", "EZ", "FA", "FB", "FC", "FD", "FE", "FF", "FG", "FH", "FI", "FJ", "FK", "FL", "FM", "FN", "FO", "FP", "FQ", "FR", "FS", "FT", "FU", "FV", "FW", "FX", "FY", "FZ" };

    }
}
