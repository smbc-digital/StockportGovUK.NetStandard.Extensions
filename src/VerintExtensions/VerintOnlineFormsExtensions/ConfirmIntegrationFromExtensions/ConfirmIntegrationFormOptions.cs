namespace StockportGovUK.NetStandard.Extensions.VerintExtensions.VerintOnlineFormsExtensions.ConfirmIntegrationFromExtensions
{
    /// <summary>
    /// Object that's only use will be for providing required information to ToConfirmIntegrationEFormCase extension
    /// method. This object should be populated from the configuration file of a service that wants to use this
    /// extension method.
    /// </summary>
    public class ConfirmIntegrationFormOptions
    {
        /// <summary>
        /// Used as the objects configurations name in config and startup of projects
        /// </summary>
        public const string ConfirmIntegrationEForm = "ConfirmIntegrationEFormOptions";
        public string ServiceCode { get; set; }
        public string SubjectCode { get; set; }
        public string FollowUp { get; set; }
        public string ClassCode { get; set; }
        public string ConfirmClassification { get; set; }
        public int EventId { get; set; }
    }
}
