using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Coop.Sample.Api
{
    public class ErrorResponse
    {
        public int Status { get; set; }
        public IEnumerable<string> Messages { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Exception { get; set; }
    }
}
