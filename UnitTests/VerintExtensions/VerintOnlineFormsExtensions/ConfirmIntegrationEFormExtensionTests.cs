using StockportGovUK.NetStandard.Extensions.VerintExtensions.VerintOnlineFormsExtensions;
using StockportGovUK.NetStandard.Models.Verint;
using Xunit;

namespace UnitTests.VerintExtensions.VerintOnlineFormsExtensions
{
    public class ConfirmIntegrationEFormExtensionTests
    {
        [Fact]
        public void ToConfirmIntegrationEFormCase_ShouldReturnPopulatedVerintOnlineFormRequest()
        {
            var caseLove = new Case
            {
                Customer = new Customer()
            };

            var configuration = new ConfirmIntegrationEFormExtension.ConfirmIntegrationEFormConfiguration
            {
                EventId = 1
            };

            var result = caseLove.ToConfirmIntegrationEFormCase(configuration);

            Assert.NotNull(result);
            Assert.NotEmpty(result.FormData);
            Assert.Equal("confirm_integrationform", result.FormName);
        }
    }
}
