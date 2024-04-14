using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseStarter.DTO
{
    /// <summary>
    /// Object sended over API
    /// </summary>
    public class BaseDtoObject
    {
        /// <summary>
        /// Object Identifier
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// Property need to check if nobody change object in databaze
        /// </summary>
        [Timestamp]
        public byte[]? ConcurrencyCheck { get; set; }
    }
}
