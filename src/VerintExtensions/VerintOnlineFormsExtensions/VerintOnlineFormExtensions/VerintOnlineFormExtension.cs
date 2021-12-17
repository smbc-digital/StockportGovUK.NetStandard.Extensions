using StockportGovUK.NetStandard.Models.Models.Verint.VerintOnlineForm;
using StockportGovUK.NetStandard.Models.Verint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace StockportGovUK.NetStandard.Extensions.VerintExtensions.VerintOnlineFormsExtensions.VerintOnlineFormExtensions
{
    public static class VerintOnlineFormExtension
    {
        private const string VOFName = "verint_ig_foi_request";

        /// <summary>
        /// An extension method to create an instance of VerintOnlineFormRequest that will be used to
        /// generate an VerintOnlineForm of type ConfirmIntegrationForm when VerintOnlineFormController.Create
        /// is called in the verint-service.
        /// </summary>
        /// <param name="crmCase"></param>
        /// <param name="configuration"></param>
        /// <returns>VerintOnlineFormRequest</returns>
        public static VerintOnlineFormRequest ToVerintOnlineFormCase(this Case crmCase, VerintOnlineFormOptions configuration)
        {
            crmCase.EventCode = configuration.EventId;

            var formData = new Dictionary<string, string>
            {
                {"le_eventcode", configuration.EventId.ToString()},
                {"txt_requiredresponse", configuration.ServiceCode},
                {"txt_formatrequired", configuration.SubjectCode}
            };

            return new VerintOnlineFormRequest
            {
                VerintCase = crmCase,
                FormName = VOFName,
                FormData = formData
            };
        }
    }
}
