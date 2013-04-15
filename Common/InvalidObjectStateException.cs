using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class InvalidObjectStateException: Exception
    {
        public string FieldName { get; private set; }
    
        public InvalidObjectStateException(string fieldName, string message): base(message)
        {
            FieldName = fieldName;
        }
    }
}
