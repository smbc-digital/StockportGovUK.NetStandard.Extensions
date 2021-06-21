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
                {"CONF_SERVICE_CODE", configuration.ServiceCode},
                {"CONF_SUBJECT_CODE", configuration.SubjectCode},
                {"FOLLOW_UP_BY", configuration.FollowUp},
                {"CboClassCode", configuration.ClassCode},
                {"le_eventcode", configuration.EventId.ToString()},
                {"le_queue_complete", "AppsConfirmQueuePending"},
                {"CONF_CASE_ID", crmCase.CaseReference},
                {"CONF_CUST_REF", crmCase.Customer.CustomerReference},
                {"CONF_CUST_TITLE", crmCase.Customer.Title},
                {"CONF_CUST_FORENAME", crmCase.Customer.Forename},
                {"CONF_CUST_SURNAME", crmCase.Customer.Surname},
                {"CONF_CUST_PHONE", crmCase.Customer.Telephone},
                {"CONF_CUST_ALT_TEL", crmCase.Customer.AlternativeTelephone},
                {"CONF_CUST_FAX", crmCase.Customer.FaxNumber},
                {"CONF_CUST_EMAIL", crmCase.Customer.Email},
                {"CONF_DESC", crmCase.Description},
                {"CONF_LOGGED_BY", "Lagan"},
                {"CONF_LOGGED_TIME", DateTime.Now.ToString()},
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
