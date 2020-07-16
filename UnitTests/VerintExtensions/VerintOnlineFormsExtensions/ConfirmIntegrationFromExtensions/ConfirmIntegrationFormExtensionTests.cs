using StockportGovUK.NetStandard.Extensions.VerintExtensions.VerintOnlineFormsExtensions.ConfirmIntegrationFromExtensions;
using StockportGovUK.NetStandard.Models.Verint;
using Xunit;

namespace UnitTests.VerintExtensions.VerintOnlineFormsExtensions.ConfirmIntegrationFromExtensions
{
    public class ConfirmIntegrationFormExtensionTests
    {
        [Fact]
        public void ToConfirmIntegrationFormCase_ShouldReturnPopulatedVerintOnlineFormRequest()
        {
            var caseLove = new Case
            {
                Customer = new Customer()
            };

            var configuration = new ConfirmIntegrationFormOptions
            {
                EventId = 1
            };

            var result = caseLove.ToConfirmIntegrationFormCase(configuration);

            Assert.NotNull(result);
            Assert.NotEmpty(result.FormData);
            Assert.Equal("confirm_integrationform", result.FormName);
        }
    }
}
