using System.Text.RegularExpressions;

namespace TGSP.Shared.Mongo
{
    /// <summary>
    /// This method contains extensions and other helper for Mongo
    /// </summary>
    public static class MongoExtensions
    {

        /// <summary>
        /// This method will test if a given string is matching the mongo object id format
        /// </summary>
        /// <param name="id">The string to test</param>
        /// <returns>True when valid else false</returns>
        public static bool IsValidObjectId(string id)
        {
            var regexp = new Regex("^[0-9a-fA-F]{24}$");
            return regexp.IsMatch(id);
        }
    }
}