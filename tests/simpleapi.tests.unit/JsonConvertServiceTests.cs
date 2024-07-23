using FluentAssertions;
using Newtonsoft.Json;
using op.Models;
using op.Services;

namespace op.tests.unit
{
    [TestFixture]
    public class JsonConvertServiceTests
    {
        [Test]
        public void ConvertXmlToJson_CorrectDataPassedIn_ShouldConvertEvaluationModelToJsonString()
        {
            // Arrange
            var evaluation = new EvaluationModel
            {
                Id = 1,
                Name = "Test Evaluation",
                Description = "This is a test description."
            };

            var expectedJson = JsonConvert.SerializeObject(new
            {
                evaluation.Id,
                evaluation.Name,
                evaluation.Description
            });

            // Act
            var actualJson = new JsonConvertService().ConvertXmlToJson(evaluation);

            // Assert
            actualJson.Should().Be(expectedJson);
        }
    }
}