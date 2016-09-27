// -----------------------------------------------------------------------
// <copyright file="MPRHMACSHA512.cs" company="RTI, Inc.">
// Class for implemeting HMACSHA512 with a key.
// See: http://stackoverflow.com/questions/9080620/fips-hmac-sha512/9080656#9080656
// </copyright>
// -----------------------------------------------------------------------
namespace EvalEngine.Domain.Concrete
{
    using System.Security.Cryptography;

    /// <summary>
    /// MPRHMACSHA512 class.
    /// </summary>
    public class MPRHMACSHA512 : HMAC
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MPRHMACSHA512"/> class.
        /// </summary>
        /// <param name="key">key for HMACSHA512</param>
        public MPRHMACSHA512(byte[] key)
        {
            this.HashName = "System.Security.Cryptography.SHA512CryptoServiceProvider";
            this.HashSizeValue = 512;
            this.BlockSizeValue = 128;
            this.Key = key;
        }
    }
}
