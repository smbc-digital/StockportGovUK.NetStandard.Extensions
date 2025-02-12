﻿using System;
using System.Linq;
using StockportGovUK.NetStandard.Extensions.VerintExtensions.VerintOnlineFormsExtensions.ConfirmIntegrationFromExtensions;
using StockportGovUK.NetStandard.Gateways.Models.Verint;
using StockportGovUK.NetStandard.Gateways.Models.Verint.Enums;
using Xunit;

namespace UnitTests.VerintExtensions.VerintOnlineFormsExtensions.ConfirmIntegrationFromExtensions
{
    public class ConfirmIntegrationFormExtensionTests
    {
        [Fact]
        public void ToConfirmIntegrationFormCase_GivenIsSmbcEmployee_ShouldAddCaseFields()
        {
            var caseLove = new Case
            {
                Customer = new Customer(),
                IsSMBCEmployee = true
            };

            var configuration = new ConfirmIntegrationFormOptions();

            var result = caseLove.ToConfirmIntegrationFormCase(configuration);

            Assert.NotEmpty(result.FormData);
            Assert.Equal("SHOT", result.FormData.FirstOrDefault(_ => _.Key == "CONF_POC_CODE").Value);
            Assert.Equal("Customer Service Centre", result.FormData.FirstOrDefault(_ => _.Key == "CONF_POC_NAME").Value);
        }

        [Fact]
        public void ToConfirmIntegrationFormCase_GivenIsNotSmbcEmployee_ShouldAddCaseFields()
        {
            var caseLove = new Case
            {
                Customer = new Customer(),
                IsSMBCEmployee = false
            };

            var configuration = new ConfirmIntegrationFormOptions();

            var result = caseLove.ToConfirmIntegrationFormCase(configuration);

            Assert.NotEmpty(result.FormData);
            Assert.Equal("WEB", result.FormData.FirstOrDefault(_ => _.Key == "CONF_METH_CODE").Value);
            Assert.Equal("Web/Online Form", result.FormData.FirstOrDefault(_ => _.Key == "CONF_METH_NAME").Value);
        }

        [Theory]
        [InlineData(EChannel.Email, "Email", "EMAI")]
        [InlineData(EChannel.Voice, "Telephone", "TELE")]
        [InlineData(EChannel.FaceToFace, "Person", "PERS")]
        [InlineData(EChannel.Web, "Web/Online Form", "WEB")]
        public void ToConfirmIntegrationFormCase_GivenDifferentSmbcChannels_ShouldAddRelevantCaseFields(EChannel channel, string methodName, string methodCode)
        {
            var caseLove = new Case
            {
                Customer = new Customer(),
                IsSMBCEmployee = true,
                Channel = channel
            };

            var configuration = new ConfirmIntegrationFormOptions();

            var result = caseLove.ToConfirmIntegrationFormCase(configuration);

            Assert.NotEmpty(result.FormData);
            Assert.Equal(methodName, result.FormData.FirstOrDefault(_ => _.Key == "CONF_METH_NAME").Value);
            Assert.Equal(methodCode, result.FormData.FirstOrDefault(_ => _.Key == "CONF_METH_CODE").Value);
        }

        [Fact]
        public void ToConfirmIntegrationFormCase_GivenAddressIsNull_ShouldNotNameCaseFields()
        {
            var caseLove = new Case
            {
                Customer = new Customer()
            };

            var configuration = new ConfirmIntegrationFormOptions();

            var result = caseLove.ToConfirmIntegrationFormCase(configuration);

            Assert.NotEmpty(result.FormData);
            Assert.False(result.FormData.Keys.Contains("CONF_CUST_LOCALITY"));
            Assert.False(result.FormData.Keys.Contains("CONF_CUST_TOWN"));
            Assert.False(result.FormData.Keys.Contains("CONF_CUST_COUNTY"));
            Assert.False(result.FormData.Keys.Contains("CONF_CUST_POSTCODE"));
        }

        [Fact]
        public void ToConfirmIntegrationFormCase_GivenAddressIsNotNull_AndAutomatic_ShouldNameCaseFields()
        {
            var caseLove = new Case
            {
                Customer = new Customer
                {
                    Address = new Address
                    {
                        Description = "1,2,3,SK1 3XE",
                        UPRN = "uprn"
                    }
                }
            };

            var configuration = new ConfirmIntegrationFormOptions();

            var result = caseLove.ToConfirmIntegrationFormCase(configuration);

            Assert.NotEmpty(result.FormData);
            Assert.True(result.FormData.Keys.Contains("CONF_CUST_LOCALITY"));
            Assert.True(result.FormData.Keys.Contains("CONF_CUST_TOWN"));
            Assert.True(result.FormData.Keys.Contains("CONF_CUST_STREET"));
            Assert.True(result.FormData.Keys.Contains("CONF_CUST_POSTCODE"));
        }

        [Fact]
        public void ToConfirmIntegrationFormCase_GivenAddressIsNotNull_AndManual_ShouldNameCaseFields()
        {
            var caseLove = new Case
            {
                Customer = new Customer
                {
                    Address = new Address
                    {
                        AddressLine1 = "1",
                        City = "3",
                        Postcode = "SK1 3XE"
                    }
                }
            };

            var configuration = new ConfirmIntegrationFormOptions();

            var result = caseLove.ToConfirmIntegrationFormCase(configuration);

            Assert.NotEmpty(result.FormData);
            Assert.False(result.FormData.Keys.Contains("CONF_CUST_LOCALITY"));
            Assert.True(result.FormData.Keys.Contains("CONF_CUST_TOWN"));
            Assert.True(result.FormData.Keys.Contains("CONF_CUST_STREET"));
            Assert.True(result.FormData.Keys.Contains("CONF_CUST_POSTCODE"));
        }

        [Fact]
        public void ToConfirmIntegrationFormCase_GivenAddressDescriptionIsNull_AndIsAutomaticAddress_ShouldThrowException()
        {
            var caseLove = new Case
            {
                Customer = new Customer
                {
                    Address = new Address
                    {
                        UPRN = "uprn"
                    }
                }
            };

            var configuration = new ConfirmIntegrationFormOptions();

            Assert.Throws<Exception>(() => caseLove.ToConfirmIntegrationFormCase(configuration));
        }

        [Fact]
        public void ToConfirmIntegrationFormCase_GivenStreetSupplied_AndDescriptionIsNull_ShouldThrowException()
        {
            var caseLove = new Case
            {
                Customer = new Customer(),
                Street = new Street()
            };

            var configuration = new ConfirmIntegrationFormOptions();

            Assert.Throws<Exception>(() => caseLove.ToConfirmIntegrationFormCase(configuration));
        }

        [Fact]
        public void ToConfirmIntegrationFormCase_GivenStreetSupplied_ShouldAddDataToCaseFields()
        {
            var usrn = "USRN";
            var description = "Name";
            var furtherLocationInformation = "FurtherLocationInformation";

            var caseLove = new Case
            {
                Customer = new Customer(),
                Street = new Street
                {
                    USRN = usrn,
                    Description = description
                },
                FurtherLocationInformation = furtherLocationInformation
            };

            var configuration = new ConfirmIntegrationFormOptions();

            var result = caseLove.ToConfirmIntegrationFormCase(configuration);

            Assert.NotEmpty(result.FormData);
            Assert.Equal(usrn, result.FormData.FirstOrDefault(_ => _.Key == "CONF_SITE_CODE").Value);
            Assert.Equal(description, result.FormData.FirstOrDefault(_ => _.Key == "CONF_SITE_NAME").Value);
            Assert.Equal(furtherLocationInformation, result.FormData.FirstOrDefault(_ => _.Key == "CONF_LOCATION").Value);
        }

        [Fact]
        public void ToConfirmIntegrationFormCase_ShouldRemoveEmptyFields()
        {
            var caseLove = new Case
            {
                Customer = new Customer(),
                Property = new Address
                {
                    Description = "Description",
                    USRN = string.Empty
                }
            };

            var configuration = new ConfirmIntegrationFormOptions();

            var result = caseLove.ToConfirmIntegrationFormCase(configuration);

            Assert.NotEmpty(result.FormData);
            Assert.False(result.FormData.Keys.Contains("CONF_SITE_CODE"));
        }
    }
}
