using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnflxFormSchemaV1
{
    public class FormCustomDataItem
    {
        public string key;
        public string value;
        public FormCustomDataItem(string pKey, string pValue)
        {
            key = pKey;
            value = pValue;
        }
    }
}

