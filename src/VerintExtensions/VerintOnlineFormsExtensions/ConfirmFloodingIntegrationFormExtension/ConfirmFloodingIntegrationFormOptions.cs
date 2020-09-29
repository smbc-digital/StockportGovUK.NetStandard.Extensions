namespace StockportGovUK.NetStandard.Extensions.VerintExtensions.VerintOnlineFormsExtensions.ConfirmIntegrationFromExtensions
{
    /// <summary>
    /// Object that's only use will be for providing required information to ToConfirmFloodingIntegrationFormCase extension
    /// method. This object should be populated from the configuration file of a service that wants to use this
    /// extension method.
    /// </summary>
    public class ConfirmFloodingIntegrationFormOptions : ConfirmIntegrationFormOptions
    {
        public string FloodingSourceReported { get; set; }
        public string LocationOfFlooding { get; set; }
        public string DomesticOrCommercial { get; set; }
    }
}
