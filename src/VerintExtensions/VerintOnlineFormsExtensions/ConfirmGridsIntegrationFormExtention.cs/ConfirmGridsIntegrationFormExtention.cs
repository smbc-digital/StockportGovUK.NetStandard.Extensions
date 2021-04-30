using StockportGovUK.NetStandard.Extensions.VerintExtensions.VerintOnlineFormsExtensions.ConfirmIntegrationFromExtensions;
using StockportGovUK.NetStandard.Models.Models.Verint.VerintOnlineForm;
using StockportGovUK.NetStandard.Models.Verint;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockportGovUK.NetStandard.Extensions.VerintExtensions.VerintOnlineFormsExtensions.ConfirmGridsIntegrationFormExtention.cs {
    public static class ConfirmGridsIntegrationFormExtention {
        private const string VOFName = "confirm_universalform";

        /// <summary>
        /// An extension method to create an instance of VerintOnlineFormRequest that will be used to
        /// generate an VerintOnlineForm of type ConfirmIntegrationForm when VerintOnlineFormController.Create
        /// is called in the verint-service.
        /// </summary>
        /// <param name="crmCase"></param>
        /// <param name="configuration"></param>
        /// <returns>VerintOnlineFormRequest</returns>
        public static VerintOnlineFormRequest ToConfirmGridsIntegrationFormCase(this Case crmCase, ConfirmGridsIntegrationFormOptions configuration) {
            var baseCase = crmCase.ToConfirmIntegrationFormCase(configuration);

            if (!string.IsNullOrEmpty(configuration.AssetId))
                baseCase.FormData.Add("CONF_ATTRIBUTE_BGTY_CODE", configuration.BlockedDrainAffecting);

            if (!string.IsNullOrEmpty(configuration.AssetId))
                baseCase.FormData.Add("CONF_ATTRIBUTE_MCAI_TEXT", configuration.AssetId);

            return baseCase;
        }
    }
}
