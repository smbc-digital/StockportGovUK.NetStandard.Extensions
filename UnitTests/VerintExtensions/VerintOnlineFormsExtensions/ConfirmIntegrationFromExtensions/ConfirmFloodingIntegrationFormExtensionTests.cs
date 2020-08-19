using StockportGovUK.NetStandard.Extensions.VerintExtensions.VerintOnlineFormsExtensions.ConfirmIntegrationFromExtensions;
using StockportGovUK.NetStandard.Models.Verint;
using Xunit;

namespace UnitTests.VerintExtensions.VerintOnlineFormsExtensions.ConfirmIntegrationFromExtensions
{
    public class ConfirmFloodingIntegrationFormExtensionTests
    {
        [Fact]
        public void ToConfirmFloodingIntegrationFormCase_ShouldReturnPopulatedVerintOnlineFormRequest_With_FloodingData()
        {
            var caseLove = new Case
            {
                Customer = new Customer()
            };

            var configuration = new ConfirmFloodingIntegrationFormOptions
            {
                EventId = 1,
                XCoordinate = "12345",
                YCoordinate = "54321",
                FloodingSourceReported = "NK",
                LocationOfFlooding = "STREET",
                DomesticOrCommercial = "DOM"
            };

            var result = caseLove.ToConfirmFloodingIntegrationFormCase(configuration);

            Assert.NotNull(result);
            Assert.NotEmpty(result.FormData);
            Assert.True(result.FormData.ContainsKey("CONF_X_COORD"));
            Assert.True(result.FormData.ContainsKey("CONF_Y_COORD"));
            Assert.True(result.FormData.ContainsKey("CONF_ATTRIBUTE_FSRC_CODE"));
            Assert.True(result.FormData.ContainsKey("CONF_ATTRIBUTE_FLOC_CODE"));
            Assert.True(result.FormData.ContainsKey("CONF_ATTRIBUTE_FDOC_CODE"));
        }
    }
}
