using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace RequestZp1 {
    class Crc32 {

        #region Constants
        /// <summary>
        /// Generator polynomial (modulo 2) for the reversed CRC32 algorithm. 
        /// </summary>
        private const UInt32 s_generator = 0xEDB88320;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of the Crc32 class.
        /// </summary>
        public Crc32() {
            // Constructs the checksum lookup table. Used to optimize the checksum.
            m_checksumTable = Enumerable.Range(0, 256).Select(i => {
                var tableEntry = (uint)i;
                for (var j = 0; j < 8; ++j) {
                    tableEntry = ((tableEntry & 1) != 0)
                        ? (s_generator ^ (tableEntry >> 1))
                        : (tableEntry >> 1);
                }
                return tableEntry;
            }).ToArray();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Calculates the checksum of the byte stream.
        /// </summary>
        /// <param name="byteStream">The byte stream to calculate the checksum for.</param>
        /// <returns>A 32-bit reversed checksum.</returns>
        public UInt32 Get<T>(IEnumerable<T> byteStream) {

            // Initialize checksumRegister to 0xFFFFFFFF and calculate the checksum.
            return ~byteStream.Aggregate(0xFFFFFFFF, (checksumRegister, currentByte) =>
                        (m_checksumTable[(checksumRegister & 0xFF) ^ Convert.ToByte(currentByte)] ^ (checksumRegister >> 8)));

        }
        #endregion

        #region Fields
        /// <summary>
        /// Contains a cache of calculated checksum chunks.
        /// </summary>
        private readonly UInt32[] m_checksumTable;

        #endregion

        /*private static string[] CRC32Table = {
                "0x00000000","0x77073096","0xee0e612c","0x990951ba","0x076dc419","0x706af48f","0xe963a535",
                "0x9e6495a3","0x0edb8832","0x79dcb8a4","0xe0d5e91e","0x97d2d988","0x09b64c2b","0x7eb17cbd",
                "0xe7b82d07","0x90bf1d91","0x1db71064","0x6ab020f2","0xf3b97148","0x84be41de","0x1adad47d"
        };

        public string GetRandomCode(int i) {
            return CRC32Table[i];
        }*/
    }
}
