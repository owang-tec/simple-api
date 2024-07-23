using FluentAssertions;
using op.Models;
using op.Services;
using System.Xml.Schema;

namespace op.tests.unit
{
    [TestFixture]
    public class XmlServiceTests
    {
        private XmlService _xmlService;

        [SetUp]
        public void Setup()
        {
            _xmlService = new XmlService();
        }

        [Test]
        public void ValidateXml_ShouldThrowXmlSchemaValidationException_WhenXmlIsInvalid()
        {
            // Arrange
            string invalidXml = "<root><invalidElement>Test</invalidElement></root>";
            string schemaContent = @"
            <xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>
              <xs:element name='root'>
                <xs:complexType>
                  <xs:sequence>
                    <xs:element name='id' type='xs:int'/>
                    <xs:element name='name' type='xs:string'/>
                    <xs:element name='description' type='xs:string'/>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
            </xs:schema>";

            File.WriteAllText("schema.xsd", schemaContent);

            // Act
            Action act = () => _xmlService.ValidateXml(invalidXml);

            // Assert
            act.Should().Throw<XmlSchemaValidationException>()
               .WithMessage("*invalidElement*");
        }

        [Test]
        public void ValidateXml_ShouldNotThrow_WhenXmlIsValid()
        {
            // Arrange
            string validXml = "<root><id>1</id><name>Test</name><description>Test Description</description></root>";
            string schemaContent = @"
            <xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>
              <xs:element name='root'>
                <xs:complexType>
                  <xs:sequence>
                    <xs:element name='id' type='xs:int'/>
                    <xs:element name='name' type='xs:string'/>
                    <xs:element name='description' type='xs:string'/>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
            </xs:schema>";

            File.WriteAllText("schema.xsd", schemaContent);

            // Act
            Action act = () => _xmlService.ValidateXml(validXml);

            // Assert
            act.Should().NotThrow();
        }

        [Test]
        public void ParseXml_ShouldReturnEvaluationModel_WhenXmlIsValid()
        {
            // Arrange
            string xmlContent = "<root><id>1</id><name>John Doe</name><description>Sample Description</description></root>";

            // Act
            var result = _xmlService.ParseXml(xmlContent);

            // Assert
            result.Should().BeEquivalentTo(new EvaluationModel
            {
                Id = 1,
                Name = "John Doe",
                Description = "Sample Description"
            });
        }

        [Test]
        public void ParseXml_ShouldThrowApplicationException_WhenXmlIsInvalid()
        {
            // Arrange
            string invalidXmlContent = "<root><test>1</test><test2>John Doe</test2></root>";

            // Act
            Action act = () => _xmlService.ParseXml(invalidXmlContent);

            // Assert
            act.Should().Throw<ApplicationException>().WithMessage("Error parsing XML content*");
        }
    }
}