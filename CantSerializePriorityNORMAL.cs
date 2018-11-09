using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Xunit;

namespace xmlserial_bug
{
    /*
    dotnet --info
    .NET Core SDK (reflecting any global.json):
     Version:   2.1.402
     Commit:    3599f217f4

    Runtime Environment:
     OS Name:     Windows
     OS Version:  6.1.7601
     OS Platform: Windows
     RID:         win7-x64
     Base Path:   C:\Program Files\dotnet\sdk\2.1.402\

    Host (useful for support):
      Version: 2.1.4
      Commit:  85255dde3e

    .NET Core SDKs installed:
      1.1.10 [C:\Program Files\dotnet\sdk]
      2.1.201 [C:\Program Files\dotnet\sdk]
      2.1.202 [C:\Program Files\dotnet\sdk]
      2.1.400 [C:\Program Files\dotnet\sdk]
      2.1.402 [C:\Program Files\dotnet\sdk]

    .NET Core runtimes installed:
      Microsoft.AspNetCore.All 2.1.2 [C:\Program Files\dotnet\shared\Microsoft.AspNetCore.All]
      Microsoft.AspNetCore.All 2.1.4 [C:\Program Files\dotnet\shared\Microsoft.AspNetCore.All]
      Microsoft.AspNetCore.App 2.1.2 [C:\Program Files\dotnet\shared\Microsoft.AspNetCore.App]
      Microsoft.AspNetCore.App 2.1.4 [C:\Program Files\dotnet\shared\Microsoft.AspNetCore.App]
      Microsoft.NETCore.App 1.0.12 [C:\Program Files\dotnet\shared\Microsoft.NETCore.App]
      Microsoft.NETCore.App 1.1.9 [C:\Program Files\dotnet\shared\Microsoft.NETCore.App]
      Microsoft.NETCore.App 2.0.0 [C:\Program Files\dotnet\shared\Microsoft.NETCore.App]
      Microsoft.NETCore.App 2.0.7 [C:\Program Files\dotnet\shared\Microsoft.NETCore.App]
      Microsoft.NETCore.App 2.0.9 [C:\Program Files\dotnet\shared\Microsoft.NETCore.App]
      Microsoft.NETCore.App 2.1.2 [C:\Program Files\dotnet\shared\Microsoft.NETCore.App]
      Microsoft.NETCore.App 2.1.4 [C:\Program Files\dotnet\shared\Microsoft.NETCore.App]

    To install additional .NET Core runtimes or SDKs:
    https://aka.ms/dotnet-download

     */

    public class CantSerializePriorityNORMAL
    {
        [Fact]
        public void WhenSerializing_WithPriorityNORMAL()
        {
            //arrange  
            // for priorty NORMAL (this string exactly!) xml serializer omits  <priority>NORMAL</priorty> when serilizing
            var priority = "NORMAL";
            var dataContract = new Example() { priority = priority };
            //act
            string result;
            var serializer = new XmlSerializer(typeof(Example));
            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter))
                {
                    serializer.Serialize(xmlWriter, dataContract);
                    result = stringWriter.ToString();
                }
            }
            //assert
            Assert.Contains($"<priority>{priority}</priority>", result);
        }

        [Fact]
        public void WhenSerializing_WithPriorityNORMALPlusSomethingElse_ItWorks()
        {
            //arrange  
            // for priorty NORMAL (this string exactly!) xml serializer omits  <priority>NORMAL</priorty> when serilizing
            var priority = "NORMAL-";
            var dataContract = new Example() { priority = priority };
            //act
            string result = Serialize(dataContract);
            //assert
            Assert.Contains($"<priority>{priority}</priority>", result);
        }


        private string Serialize(Example dataContract)
        {
            var serializer = new XmlSerializer(typeof(Example));
            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter))
                {
                    serializer.Serialize(xmlWriter, dataContract);
                    return stringWriter.ToString();
                }
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://example.com/")]
    public partial class Example
    {
        private string priorityField;

        public Example()
        {
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        [System.ComponentModel.DefaultValueAttribute("NORMAL")]
        public string priority
        {
            get
            {
                return this.priorityField;
            }
            set
            {
                this.priorityField = value;
            }
        }
    }
}
