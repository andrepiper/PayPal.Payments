using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models.Backend
{
    public class ServiceResponse<T>
    {
        public Boolean Success;
        public String Message;
        public T Response;
        public Exception Error;
    }
}
