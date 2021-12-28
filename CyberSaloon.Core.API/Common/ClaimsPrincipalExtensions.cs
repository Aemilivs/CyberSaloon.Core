using System;
using System.Linq;
using System.Security.Claims;

namespace CyberSaloon.Core.API.Common
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Extract the unique identifier of a user from the principal.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns>Guid representing unique id of a user.</returns>
        public static Guid GetSub(this ClaimsPrincipal principal)
        {
            var raw =
                principal
                    .Claims
                    .FirstOrDefault(it => it.Type == "sub")
                    .Value;
            
            var result = Guid.TryParse(raw, out var sub);

            return 
                result ? 
                    sub : 
                    default;
        }
    }
}