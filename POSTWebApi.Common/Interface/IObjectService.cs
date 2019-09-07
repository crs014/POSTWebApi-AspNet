using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSTWebApi.Common.Interface
{
    public interface IObjectService
    {
        string SerializerObject(object data);
        object DeserializerObject(string json);
    }
}
