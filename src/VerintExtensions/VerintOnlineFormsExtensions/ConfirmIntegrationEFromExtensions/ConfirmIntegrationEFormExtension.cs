using StockportGovUK.NetStandard.Models.Models.Verint.VerintOnlineForm;
using StockportGovUK.NetStandard.Models.Verint;
using System.Collections.Generic;

namespace StockportGovUK.NetStandard.Extensions.VerintExtensions.VerintOnlineFormsExtensions.ConfirmIntegrationEFromExtensions
{
    public static class ConfirmIntegrationEFormExtension
    {
        private const string VOFName = "confirm_integrationform";

        /// <summary>
        /// An extension method to create an instance of VerintOnlineFormRequest that will be used to
        /// generate an VerintOnlineForm of type ConfirmIntegrationForm when VerintOnlineFormController.Create
        /// is called in the verint-service.
        /// </summary>
        /// <param name="crmCase"></param>
        /// <param name="configuration"></param>
        /// <returns>VerintOnlineFormRequest</returns>
        public static VerintOnlineFormRequest ToConfirmIntegrationEFormCase(this Case crmCase, ConfirmIntegrationEFormOptions configuration)
        {
            crmCase.EventCode = configuration.EventId;
            
            var formData = new Dictionary<string, string>
                {
                    {"CONF_SERVICE_CODE", configuration.ServiceCode},
                    {"CONF_SUBJECT_CODE", configuration.SubjectCode},
                    {"FOLLOW_UP_BY", configuration.FollowUp},
                    {"CboClassCode", configuration.ClassCode},
                    {"CONF_CASE_ID", crmCase.CaseReference},
                    {"CONF_CUST_REF", crmCase.Customer.CustomerReference},
                    {"CONF_CUST_TITLE", crmCase.Customer.Title},
                    {"CONF_CUST_FORENAME", crmCase.Customer.Forename},
                    {"CONF_CUST_SURNAME", crmCase.Customer.Surname},
                    {"CONF_CUST_PHONE", crmCase.Customer.Telephone},
                    {"CONF_CUST_ALT_TEL", crmCase.Customer.AlternativeTelephone},
                    {"CONF_CUST_FAX", crmCase.Customer.FaxNumber},
                    {"CONF_CUST_EMAIL", crmCase.Customer.Email},
                    {"CONF_CLASSIFICATION", crmCase.Classification},
                    {" CONF_DESC", crmCase.Description}
                };

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

            if (string.IsNullOrEmpty(crmCase.Customer.FullName))
            {
                crmCase.Customer.Title = "Mr";
                crmCase.Customer.Surname = "ANON";
            }
            if (crmCase.Customer.FullName.Length <= 30)
            {
                formData.Add("CONF_CONTACT", crmCase.Customer.FullName);
            }
            else if ((crmCase.Customer.Title + " " + crmCase.Customer.Surname).Trim().Length <= 30)
            {
                formData.Add("CONF_CONTACT", $"{crmCase.Customer.Title} {crmCase.Customer.Surname.Trim()}");
            }
            else if (crmCase.Customer.Forename.Length <= 30)
            {
                formData.Add("CONF_CONTACT", crmCase.Customer.Forename);
            }
            else
            {
                formData.Add("CONF_CONTACT", crmCase.Customer.FullName.Substring(0, 30));
            }

            if (crmCase.Customer.Address != null)
            {
                if (int.TryParse(crmCase.Customer.Address.Number, out var streetNum))
                {
                    formData.Add("CONF_CUST_STREETNUM", crmCase.Customer.Address.Number);
                }
                else
                {
                    formData.Add("CONF_CUST_BUILDING", crmCase.Customer.Address.Number);
                }

                formData.Add("CONF_CUST_STREET", crmCase.Customer.Address.AddressLine1);
                formData.Add("CONF_CUST_LOCALITY", crmCase.Customer.Address.AddressLine3);
                formData.Add("CONF_CUST_TOWN", crmCase.Customer.Address.City);
                formData.Add("CONF_CUST_POSTCODE", crmCase.Customer.Address.Postcode);
            }

            if (crmCase.Property != null)
            {
                formData.Add("CONF_SITE_CODE", crmCase.Property.USRN);
                formData.Add("CONF_SITE_NAME", crmCase.Property.AddressLine1);
                formData.Add("CONF_SITE_LOCALITY", crmCase.Property.AddressLine3);
                formData.Add("CONF_SITE_TOWN", crmCase.Property.City);
                formData.Add("CONF_LOCATION", string.IsNullOrEmpty(crmCase.FurtherLocationInformation)
                    ? crmCase.Property.Description
                    : $"{crmCase.Property.Description} - {crmCase.FurtherLocationInformation}");
            }
            else if (crmCase.Street != null)
            {
                formData.Add("CONF_SITE_CODE", crmCase.Street.USRN);
                formData.Add("CONF_LOCATION", crmCase.FurtherLocationInformation);

                string[] siteDetails = crmCase.Street.Description.Split(',');
                formData.Add("CONF_SITE_NAME", siteDetails[0].Trim());
                if (siteDetails.Length > 1)
                    formData.Add("CONF_SITE_LOCALITY", siteDetails[1].Trim());
                if (siteDetails.Length > 2)
                    formData.Add("CONF_SITE_TOWN", siteDetails[2].Trim());
            }

            return new VerintOnlineFormRequest
            {
                VerintCase = crmCase,
                FormName = VOFName,
                FormData = formData
            };
        }
    }
}
