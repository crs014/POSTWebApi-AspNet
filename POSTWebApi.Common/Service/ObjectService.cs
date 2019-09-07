using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using POSTWebApi.Common.Interface;

namespace POSTWebApi.Common.Service
{
    public class ObjectService<T> : IObjectService
    {
        private JavaScriptSerializer _javaScriptSerializer;

        public ObjectService()
        {
            _javaScriptSerializer = new JavaScriptSerializer();
        }


        public object DeserializerObject(string json)
        {
            try
            {
                return _javaScriptSerializer.Deserialize<T>(json);
            }
            catch(Exception e)
            {
                throw new Exception("Object is not required");
            }
             
        }

        public string SerializerObject(object data)
        {
            try
            {     
                string json = _javaScriptSerializer.Serialize(data);
                return json;
            }
            catch(Exception e)
            {
                return "";
            }
        }
    }
}
