#region License
// Copyright (c) 2014 True Systems Informatica Ltda
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Newtonsoft.Json;

namespace OnflxFormSchemaV1
{
    public class ImageField
    {
        [JsonProperty(PropertyName = "$type")]
        public string doNetObjectType;
        public string jacksonObjectType;
        public string image;
        public string format;
        public string thumbnail;
        public ImageField(string pImage, string pFormaUrlOrBase64, string pThumbnail = "")
        {
            /*
             * This is required do deserialization using Newtonsoft.Json v6.0.3 or later
             * on .Net Framework 
             */
            doNetObjectType = this.GetType().Namespace + "." + this.GetType().Name + "," + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            /*
             * This is required do deserialization using Jackson JSon 
             * on Java 1.7 or greater  
             */
            jacksonObjectType = this.GetType().Name;

            /*
             * This is(are) object properties 
             */
            image = pImage;
            format = pFormaUrlOrBase64;
            thumbnail = pThumbnail;
        }
    }
}
