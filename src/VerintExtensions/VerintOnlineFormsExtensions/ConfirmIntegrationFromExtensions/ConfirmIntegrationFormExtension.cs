using StockportGovUK.NetStandard.Models.Models.Verint.VerintOnlineForm;
using StockportGovUK.NetStandard.Models.Verint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace StockportGovUK.NetStandard.Extensions.VerintExtensions.VerintOnlineFormsExtensions.ConfirmIntegrationFromExtensions
{
    public static class ConfirmIntegrationFormExtension
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
        public static VerintOnlineFormRequest ToConfirmIntegrationFormCase(this Case crmCase, ConfirmIntegrationFormOptions configuration)
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
                    {"CONF_LOGGED_TIME", DateTime.Now.ToString()}
                };

            if (!string.IsNullOrEmpty(configuration.XCoordinate) && !string.IsNullOrEmpty(configuration.YCoordinate))
            {
                formData.Add("CONF_X_COORD", configuration.XCoordinate);
                formData.Add("CONF_Y_COORD", configuration.YCoordinate);
            }

            if (crmCase.IsSMBCEmployee)
            {
                formData.Add("CONF_POC_CODE", "SHOT");
                formData.Add("CONF_POC_NAME", "Customer Service Centre");

                switch (crmCase.SMBCChannel)
                {
                    case "EMAIL_IN":
                        formData.Add("CONF_METH_NAME", "Email");
                        formData.Add("CONF_METH_CODE", "EMAI");
                        break;
                    case "VOICE_IN":
                        formData.Add("CONF_METH_NAME", "Telephone");
                        formData.Add("CONF_METH_CODE", "TELE");
                        break;
                    case "FACE_TO_FACE":
                        formData.Add("CONF_METH_NAME", "Person");
                        formData.Add("CONF_METH_CODE", "PERS");
                        break;
                    case "WEB":
                        formData.Add("CONF_METH_NAME", "Web/Online Form");
                        formData.Add("CONF_METH_CODE", "WEB");
                        break;
                }
            }
            else
            {
                formData.Add("CONF_METH_NAME", "Web/Online Form");
                formData.Add("CONF_METH_CODE", "WEB");
            }

            if (crmCase.Customer.Address != null)
            {
                var address = crmCase.Customer.Address;
                if (string.IsNullOrEmpty(address.Description))
                    throw new Exception("ConfirmIntegrationFormExtension.ToConfirmIntegrationFormCase: Address.Description is required within Confirm.");

                var addressDetails = address.Description.Split(',');
                formData.Add("CONF_CUST_STREET", addressDetails.First().Trim());
                if (addressDetails.Length > 2)
                    formData.Add("CONF_CUST_TOWN", addressDetails[2].Trim());


                var postcode = addressDetails.FirstOrDefault(_ => Regex.IsMatch(_, @"(sK|Sk|SK|sk|M|m)[0-9][0-9A-Za-z]?\s?[0-9][A-Za-z]{2}")).Trim();
                formData.Add("CONF_CUST_POSTCODE", postcode);

            }

            if (crmCase.Property != null)
            {
                if (string.IsNullOrEmpty(crmCase.Property.Description))
                    throw new Exception("ConfirmIntegrationFormExtension.ToConfirmIntegrationFormCase: Property.Description is required within Confirm.");

                var siteDetails = crmCase.Property.Description.Split(',');

                formData.Add("CONF_SITE_CODE", crmCase.Property.USRN);
                formData.Add("CONF_SITE_NAME", siteDetails[0].Trim());
                if (siteDetails.Length > 1)
                    formData.Add("CONF_SITE_LOCALITY", siteDetails[1].Trim());
                if (siteDetails.Length > 2)
                    formData.Add("CONF_SITE_TOWN", siteDetails[2].Trim());
                formData.Add("CONF_LOCATION", string.IsNullOrEmpty(crmCase.FurtherLocationInformation)
                    ? crmCase.Property.Description
                    : $"{crmCase.Property.Description} - {crmCase.FurtherLocationInformation}");
            }
            else if (crmCase.Street != null)
            {
                if (string.IsNullOrEmpty(crmCase.Street.Description))
                    throw new Exception("ConfirmIntegrationFormExtension.ToConfirmIntegrationFormCase: Street.Description is required within Confirm.");

                var siteDetails = crmCase.Street.Description.Split(',');

                formData.Add("CONF_SITE_CODE", crmCase.Street.USRN);
                formData.Add("CONF_LOCATION", crmCase.FurtherLocationInformation);
                formData.Add("CONF_SITE_NAME", siteDetails[0].Trim());
                if (siteDetails.Length > 1)
                    formData.Add("CONF_SITE_LOCALITY", siteDetails[1].Trim());
                if (siteDetails.Length > 2)
                    formData.Add("CONF_SITE_TOWN", siteDetails[2].Trim());
            }

            foreach (var key in formData.Keys)
                if (string.IsNullOrEmpty(formData[key]))
                    formData.Remove(key);

            return new VerintOnlineFormRequest
            {
                VerintCase = crmCase,
                FormName = VOFName,
                FormData = formData
            };
        }
    }
}
