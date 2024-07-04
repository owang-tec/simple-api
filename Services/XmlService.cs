using op.Models;
using op.Services.Interfaces;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace op.Services
{
    public class XmlService : IXmlService
    {
        public void ValidateXml(string xmlContent)
        {
            var schema = new XmlSchemaSet();
            schema.Add("", "schema.xsd");

            var settings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema,
                Schemas = schema
            };

            settings.ValidationEventHandler += ValidationEventHandler;

            using (var reader = XmlReader.Create(new StringReader(xmlContent), settings))
            {
                while (reader.Read()) ;
            }
        }

        public EvaluationModel ParseXml(string xmlContent)
        {
            try
            {
                XDocument doc = XDocument.Parse(xmlContent);

                int id = (int)doc.Root.Element("id");
                string name = (string)doc.Root.Element("name");
                string description = (string)doc.Root.Element("description");

                return new EvaluationModel
                {
                    Id = id,
                    Name = name,
                    Description = description
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error parsing XML content", ex);
            }
        }

        private void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Error || e.Severity == XmlSeverityType.Warning)
            {
                throw new XmlSchemaValidationException(e.Message);
            }
        }
    }
}
