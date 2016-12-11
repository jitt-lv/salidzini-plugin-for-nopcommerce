using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;

namespace Nop.Plugin.Feed.Salidzini.Models
{
    public class XmlResult<T> : ActionResult
    {
        private T _objectToSerialize;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlResult"/> class.
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize to XML.</param>
        public XmlResult(T objectToSerialize)
        {
            _objectToSerialize = objectToSerialize;
        }

        /// <summary>
        /// Gets the object to be serialized to XML.
        /// </summary>
        public T ObjectToSerialize
        {
            get { return _objectToSerialize; }
        }

        /// <summary>
        /// Serialises the object that was passed into the constructor to XML and writes the corresponding XML to the result stream.
        /// </summary>
        /// <param name="context">The controller context for the current request.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (_objectToSerialize != null)
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add(string.Empty, string.Empty);

                context.HttpContext.Response.Clear();
                context.HttpContext.Response.ContentType = "text/xml";

                var settings = new XmlWriterSettings
                {
                    Encoding = Encoding.UTF8,
                    ConformanceLevel = ConformanceLevel.Document,
                    OmitXmlDeclaration = false,
                    CloseOutput = true,
                    Indent = true,
                    NewLineHandling = NewLineHandling.Replace
                };

                var writer = XmlWriter.Create(context.HttpContext.Response.Output, settings);
                var serializer = new XmlSerializer(typeof(T));

                serializer.Serialize(writer, _objectToSerialize, ns);
            }
        }
    }
}
