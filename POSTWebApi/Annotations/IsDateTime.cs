using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POSTWebApi.Annotations
{
    public class IsDateTime : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            try
            {
                if (value != null)
                {
                    DateTime dateTime;
                    bool parsed = DateTime.TryParse((string)value, out dateTime);
                    if (!parsed)
                    {
                        return false;
                    }
                    return true;
                }
                return true;
            }
            catch(Exception)
            {
                return false;
            }
         
        }
    }
}