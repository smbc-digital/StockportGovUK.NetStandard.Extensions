using StockportGovUK.NetStandard.Extensions.VerintExtensions.VerintOnlineFormsExtensions.ConfirmIntegrationFromExtensions;
using StockportGovUK.NetStandard.Gateways.Models.Verint;
using StockportGovUK.NetStandard.Gateways.Models.Verint.VerintOnlineForm;

namespace StockportGovUK.NetStandard.Extensions.VerintExtensions.VerintOnlineFormsExtensions.ConfirmGridsIntegrationFormExtention.cs
{
    public static class ConfirmGridsIntegrationFormExtension
    {
        /// <summary>
        /// An extension method to create an instance of VerintOnlineFormRequest that will be used to
        /// generate an VerintOnlineForm of type ConfirmIntegrationForm when VerintOnlineFormController.Create
        /// is called in the verint-service.
        /// </summary>
        /// <param name="crmCase"></param>
        /// <param name="configuration"></param>
        /// <returns>VerintOnlineFormRequest</returns>
        public static VerintOnlineFormRequest ToConfirmGridsIntegrationFormCase(this Case crmCase, ConfirmGridsIntegrationFormOptions configuration)
        {
            var baseCase = crmCase.ToConfirmIntegrationFormCase(configuration);

            if (!string.IsNullOrEmpty(configuration.AssetId))
                baseCase.FormData.Add("CONF_ATTRIBUTE_BGTY_CODE", configuration.BlockedDrainAffecting);

            if (!string.IsNullOrEmpty(configuration.AssetId))
                baseCase.FormData.Add("CONF_ATTRIBUTE_MCAI_TEXT", configuration.AssetId);

            return baseCase;
        }
    }
}
