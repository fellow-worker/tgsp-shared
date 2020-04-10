using System;
using System.Collections.Generic;

namespace TGSP.Shared.Security
{
    /// <summary>
    /// The interface for the token provider
    /// </summary>
    public interface ITokenProvider
    {
        /// <summary>
        /// This method will validate a given token and returns the data in the token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        (TokenValidationResult, TokenData) Validate(string token);
    }
}