using StockportGovUK.NetStandard.Extensions.VerintExtensions.VerintOnlineFormsExtensions.ConfirmGridsIntegrationFormExtention.cs;
using StockportGovUK.NetStandard.Gateways.Models.Verint;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTests.VerintExtensions.VerintOnlineFormsExtensions.ConfirmIntegrationFromExtensions {
    public class ConfirmGridsIntegrationFormExtensionTests {
        [Fact]
        public void ToConfirmGridsIntegrationFormCase_ShouldReturnPopulatedVerintOnlineFormRequest_With_GridData() {
            var caseLove = new Case {
                Customer = new Customer()
            };

            var configuration = new ConfirmGridsIntegrationFormOptions {
                EventId = 1,
                XCoordinate = "12345",
                YCoordinate = "54321",
                AssetId = "1234",
                BlockedDrainAffecting = "PRO",
            };

            var result = caseLove.ToConfirmGridsIntegrationFormCase(configuration);

            Assert.NotNull(result);
            Assert.NotEmpty(result.FormData);
            Assert.True(result.FormData.ContainsKey("CONF_X_COORD"));
            Assert.True(result.FormData.ContainsKey("CONF_Y_COORD"));
            Assert.True(result.FormData.ContainsKey("CONF_ATTRIBUTE_BGTY_CODE"));
            Assert.True(result.FormData.ContainsKey("CONF_ATTRIBUTE_MCAI_TEXT"));
        }
    }
}
