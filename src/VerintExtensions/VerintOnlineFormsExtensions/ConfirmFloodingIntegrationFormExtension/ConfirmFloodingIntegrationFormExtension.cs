using StockportGovUK.NetStandard.Gateways.Models.Verint;
using StockportGovUK.NetStandard.Gateways.Models.Verint.VerintOnlineForm;

namespace StockportGovUK.NetStandard.Extensions.VerintExtensions.VerintOnlineFormsExtensions.ConfirmIntegrationFromExtensions
{
    public static class ConfirmFloodingIntegrationFormExtension
    {
        private const string VOFName = "confirm_universalform";

        /// <summary>
        /// An extension method to create an instance of VerintOnlineFormRequest that will be used to
        /// generate an VerintOnlineForm of type ConfirmIntegrationForm when VerintOnlineFormController.Create
        /// is called in the verint-service.
        /// </summary>
        /// <param name="crmCase"></param>
        /// <param name="configuration"></param>
        /// <returns>VerintOnlineFormRequest</returns>
        public static VerintOnlineFormRequest ToConfirmFloodingIntegrationFormCase(this Case crmCase, ConfirmFloodingIntegrationFormOptions configuration)
        {
            var baseCase = crmCase.ToConfirmIntegrationFormCase(configuration);

            if (!string.IsNullOrEmpty(configuration.FloodingSourceReported))
                baseCase.FormData.Add("CONF_ATTRIBUTE_FSRC_CODE", configuration.FloodingSourceReported);

            if (!string.IsNullOrEmpty(configuration.LocationOfFlooding))
                baseCase.FormData.Add("CONF_ATTRIBUTE_FLOC_CODE", configuration.LocationOfFlooding);

            if (!string.IsNullOrEmpty(configuration.DomesticOrCommercial))
                baseCase.FormData.Add("CONF_ATTRIBUTE_FDOC_CODE", configuration.DomesticOrCommercial);

            return baseCase;
        }
    }
}
