using System;
using System.Linq;
using StockportGovUK.NetStandard.Extensions.VerintExtensions.VerintOnlineFormsExtensions.ConfirmIntegrationFromExtensions;
using StockportGovUK.NetStandard.Models.Verint;
using Xunit;

namespace UnitTests.VerintExtensions.VerintOnlineFormsExtensions.ConfirmIntegrationFromExtensions
{
    public class ConfirmIntegrationFormExtensionTests
    {
        [Fact]
        public void ToConfirmIntegrationFormCase_ShouldReturnPopulatedVerintOnlineFormRequest()
        {
            var caseLove = new Case
            {
                Customer = new Customer()
            };

            var configuration = new ConfirmIntegrationFormOptions
            {
                EventId = 1
            };

            var result = caseLove.ToConfirmIntegrationFormCase(configuration);

            Assert.NotNull(result);
            Assert.NotEmpty(result.FormData);
            Assert.Equal("confirm_universalform", result.FormName);
        }

        [Fact]
        public void ToConfirmIntegrationFormCase_ShouldReturnPopulatedVerintOnlineFormRequest_WithX_AndY_Coordinate()
        {
            var caseLove = new Case
            {
                Customer = new Customer()
            };

            var configuration = new ConfirmIntegrationFormOptions
            {
                EventId = 1,
                XCoordinate = "12345",
                YCoordinate = "54321"
            };

            var result = caseLove.ToConfirmIntegrationFormCase(configuration);

            Assert.NotNull(result);
            Assert.NotEmpty(result.FormData);
            Assert.True(result.FormData.ContainsKey("CONF_X_COORD"));
            Assert.True(result.FormData.ContainsKey("CONF_Y_COORD"));
        }

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
        [InlineData("EMAIL_IN", "Email", "EMAI")]
        [InlineData("VOICE_IN", "Telephone", "TELE")]
        [InlineData("FACE_TO_FACE", "Person", "PERS")]
        [InlineData("WEB", "Web/Online Form", "WEB")]
        public void ToConfirmIntegrationFormCase_GivenDifferentSmbcChannels_ShouldAddRelevantCaseFields(string channel, string methodName, string methodCode)
        {
            var caseLove = new Case
            {
                Customer = new Customer(),
                IsSMBCEmployee = true,
                SMBCChannel = channel
            };

            var configuration = new ConfirmIntegrationFormOptions();

            var result = caseLove.ToConfirmIntegrationFormCase(configuration);

            Assert.NotEmpty(result.FormData);
            Assert.Equal(methodName, result.FormData.FirstOrDefault(_ => _.Key == "CONF_METH_NAME").Value);
            Assert.Equal(methodCode, result.FormData.FirstOrDefault(_ => _.Key == "CONF_METH_CODE").Value);
        }

        [Fact]
        public void ToConfirmIntegrationFormCase_GivenFullNameIsNullOrEmpty_ShouldAddDefaultDetails()
        {
            var caseLove = new Case
            {
                Customer = new Customer()
            };

            var configuration = new ConfirmIntegrationFormOptions();

            var result = caseLove.ToConfirmIntegrationFormCase(configuration);

            Assert.NotEmpty(result.FormData);
            Assert.Equal("Mr", caseLove.Customer.Title);
            Assert.Equal("ANON", caseLove.Customer.Surname);
        }

        [Theory]
        [InlineData("FullName", "LessThan30Char")]
        [InlineData("FullNameThat", "IsEqualTo30Chars!")]
        public void ToConfirmIntegrationFormCase_GivenFullNameIsLessThanOrEqualTo30Chars_ShouldAddFullName(string firstName, string surname)
        {
            var caseLove = new Case
            {
                Customer = new Customer
                {
                    Forename = firstName,
                    Surname = surname
                }
            };

            var configuration = new ConfirmIntegrationFormOptions();

            var result = caseLove.ToConfirmIntegrationFormCase(configuration);

            Assert.NotEmpty(result.FormData);
            Assert.Equal($"{firstName} {surname}", result.FormData.FirstOrDefault(_ => _.Key == "CONF_CONTACT").Value);
        }

        [Fact]
        public void ToConfirmIntegrationFormCase_GivenFullNameIsGreaterThan30Chars_ShouldAddTitleAndSurname()
        {
            var title = "Mr";
            var surname = "Surname";

            var caseLove = new Case
            {
                Customer = new Customer
                {
                    Title = title,
                    Forename = "FirstNameThatIsGreaterThan30Chars",
                    Surname = surname
                }
            };

            var configuration = new ConfirmIntegrationFormOptions();

            var result = caseLove.ToConfirmIntegrationFormCase(configuration);

            Assert.NotEmpty(result.FormData);
            Assert.Equal($"{title} {surname}", result.FormData.FirstOrDefault(_ => _.Key == "CONF_CONTACT").Value);
        }

        [Fact]
        public void ToConfirmIntegrationFormCase_GivenFullNameIsGreaterThan30Chars_AndTitleSurnameIsGreaterThan30Chars_ShouldAddForename()
        {
            var forename = "Forename";

            var caseLove = new Case
            {
                Customer = new Customer
                {
                    Title = "Mr",
                    Forename = forename,
                    Surname = "SurnameThatIsGreaterThan30Chars"
                }
            };

            var configuration = new ConfirmIntegrationFormOptions();

            var result = caseLove.ToConfirmIntegrationFormCase(configuration);

            Assert.NotEmpty(result.FormData);
            Assert.Equal($"{forename}", result.FormData.FirstOrDefault(_ => _.Key == "CONF_CONTACT").Value);
        }

        [Fact]
        public void ToConfirmIntegrationFormCase_GivenFullNameIsGreaterThan30Chars_AndTitleSurnameIsGreaterThan30Chars_AndForenameIsGreaterThan30Chars_ShouldAddFullnameSubstring()
        {
            var forename = "ForenameThatIsGreaterThan30Chars";
            var surname = "SurnameThatIsGreaterThan30Chars";

            var caseLove = new Case
            {
                Customer = new Customer
                {
                    Forename = forename,
                    Surname = surname
                }
            };

            var configuration = new ConfirmIntegrationFormOptions();

            var result = caseLove.ToConfirmIntegrationFormCase(configuration);

            Assert.NotEmpty(result.FormData);
            Assert.Equal($"{forename} {surname}".Substring(0, 30), result.FormData.FirstOrDefault(_ => _.Key == "CONF_CONTACT").Value);
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
        public void ToConfirmIntegrationFormCase_GivenAddressDescriptionIsNull_ShouldThrowException()
        {
            var caseLove = new Case
            {
                Customer = new Customer
                {
                    Address = new Address()
                }
            };

            var configuration = new ConfirmIntegrationFormOptions();

            Assert.Throws<Exception>(() => caseLove.ToConfirmIntegrationFormCase(configuration));
        }

        [Fact]
        public void ToConfirmIntegrationFormCase_GivenAddressLocalitySupplied_ShouldSplitCorrectlyAndAddCaseFields()
        {
            var locality = "Address Locality";

            var caseLove = new Case
            {
                Customer = new Customer
                {
                    Address = new Address
                    {
                        Description = locality
                    }
                }
            };

            var configuration = new ConfirmIntegrationFormOptions();

            var result = caseLove.ToConfirmIntegrationFormCase(configuration);

            Assert.NotEmpty(result.FormData);
            Assert.Equal(locality, result.FormData.FirstOrDefault(_ => _.Key == "CONF_CUST_LOCALITY").Value);
        }

        [Fact]
        public void ToConfirmIntegrationFormCase_GivenAddressLocalityAndTownSupplied_ShouldSplitCorrectlyAndAddCaseFields()
        {
            var locality = "Address Locality";
            var town = "Town";
            var description = string.Join(',', new string[] {locality, town});

            var caseLove = new Case
            {
                Customer = new Customer
                {
                    Address = new Address
                    {
                        Description = description
                    }
                }
            };

            var configuration = new ConfirmIntegrationFormOptions();

            var result = caseLove.ToConfirmIntegrationFormCase(configuration);

            Assert.NotEmpty(result.FormData);
            Assert.Equal(locality, result.FormData.FirstOrDefault(_ => _.Key == "CONF_CUST_LOCALITY").Value);
            Assert.Equal(town, result.FormData.FirstOrDefault(_ => _.Key == "CONF_CUST_TOWN").Value);
        }

        [Fact]
        public void ToConfirmIntegrationFormCase_GivenAddressLocalityAndTownAndCountySupplied_ShouldSplitCorrectlyAndAddCaseFields()
        {
            var locality = "Address Locality";
            var town = "Town";
            var county = "County";
            var description = string.Join(',', new string[] {locality, town, county});

            var caseLove = new Case
            {
                Customer = new Customer
                {
                    Address = new Address
                    {
                        Description = description
                    }
                }
            };

            var configuration = new ConfirmIntegrationFormOptions();

            var result = caseLove.ToConfirmIntegrationFormCase(configuration);

            Assert.NotEmpty(result.FormData);
            Assert.Equal(locality, result.FormData.FirstOrDefault(_ => _.Key == "CONF_CUST_LOCALITY").Value);
            Assert.Equal(town, result.FormData.FirstOrDefault(_ => _.Key == "CONF_CUST_TOWN").Value);
            Assert.Equal(county, result.FormData.FirstOrDefault(_ => _.Key == "CONF_CUST_COUNTY").Value);
        }

        [Fact]
        public void ToConfirmIntegrationFormCase_GivenAddressLocalityAndTownAndCountyAndPostcodeSupplied_ShouldSplitCorrectlyAndAddCaseFields()
        {
            var locality = "Address Locality";
            var town = "Town";
            var county = "County";
            var postcode = "PostCode";
            var description = string.Join(',', new string[] {locality, town, county, postcode});

            var caseLove = new Case
            {
                Customer = new Customer
                {
                    Address = new Address
                    {
                        Description = description
                    }
                }
            };

            var configuration = new ConfirmIntegrationFormOptions();

            var result = caseLove.ToConfirmIntegrationFormCase(configuration);

            Assert.NotEmpty(result.FormData);
            Assert.Equal(locality, result.FormData.FirstOrDefault(_ => _.Key == "CONF_CUST_LOCALITY").Value);
            Assert.Equal(town, result.FormData.FirstOrDefault(_ => _.Key == "CONF_CUST_TOWN").Value);
            Assert.Equal(county, result.FormData.FirstOrDefault(_ => _.Key == "CONF_CUST_COUNTY").Value);
            Assert.Equal(postcode, result.FormData.FirstOrDefault(_ => _.Key == "CONF_CUST_POSTCODE").Value);
        }

        [Fact]
        public void ToConfirmIntegrationFormCase_GivenPropertyAndStreetAreNull_ShouldNotAddCaseFields()
        {
            var caseLove = new Case
            {
                Customer = new Customer()
            };

            var configuration = new ConfirmIntegrationFormOptions();

            var result = caseLove.ToConfirmIntegrationFormCase(configuration);

            Assert.NotEmpty(result.FormData);
            Assert.False(result.FormData.Keys.Contains("CONF_SITE_CODE"));
            Assert.False(result.FormData.Keys.Contains("CONF_SITE_NAME"));
            Assert.False(result.FormData.Keys.Contains("CONF_SITE_LOCALITY"));
            Assert.False(result.FormData.Keys.Contains("CONF_SITE_TOWN"));
            Assert.False(result.FormData.Keys.Contains("CONF_LOCATION"));
        }

        [Fact]
        public void ToConfirmIntegrationFormCase_GivenPropertySupplied_ShouldAddDataToCaseFields()
        {
            var usrn = "USRN";
            var addressLine1 = "AddressLine1";
            var addressLine3 = "AddressLine3";
            var city = "City";
            var description = "Description";

            var caseLove = new Case
            {
                Customer = new Customer(),
                Property = new Address
                {
                    USRN = usrn,
                    AddressLine1 = addressLine1,
                    AddressLine3 = addressLine3,
                    City = city,
                    Description = description
                }
            };

            var configuration = new ConfirmIntegrationFormOptions();

            var result = caseLove.ToConfirmIntegrationFormCase(configuration);

            Assert.NotEmpty(result.FormData);
            Assert.Equal(usrn, result.FormData.FirstOrDefault(_ => _.Key == "CONF_SITE_CODE").Value);
            Assert.Equal(addressLine1, result.FormData.FirstOrDefault(_ => _.Key == "CONF_SITE_NAME").Value);
            Assert.Equal(addressLine3, result.FormData.FirstOrDefault(_ => _.Key == "CONF_SITE_LOCALITY").Value);
            Assert.Equal(city, result.FormData.FirstOrDefault(_ => _.Key == "CONF_SITE_TOWN").Value);
            Assert.Equal(description, result.FormData.FirstOrDefault(_ => _.Key == "CONF_LOCATION").Value);
        }

        [Fact]
        public void ToConfirmIntegrationFormCase_GivenPropertySupplied_WithFurtherInformation_ShouldAddDataToCaseFields()
        {
            var usrn = "USRN";
            var addressLine1 = "AddressLine1";
            var addressLine3 = "AddressLine3";
            var city = "City";
            var description = "Description";
            var furtherLocationInformation = "FurtherLocationInformation";

            var caseLove = new Case
            {
                Customer = new Customer(),
                Property = new Address
                {
                    USRN = usrn,
                    AddressLine1 = addressLine1,
                    AddressLine3 = addressLine3,
                    City = city,
                    Description = description
                },
                FurtherLocationInformation = furtherLocationInformation
            };

            var configuration = new ConfirmIntegrationFormOptions();

            var result = caseLove.ToConfirmIntegrationFormCase(configuration);

            Assert.NotEmpty(result.FormData);
            Assert.Equal(usrn, result.FormData.FirstOrDefault(_ => _.Key == "CONF_SITE_CODE").Value);
            Assert.Equal(addressLine1, result.FormData.FirstOrDefault(_ => _.Key == "CONF_SITE_NAME").Value);
            Assert.Equal(addressLine3, result.FormData.FirstOrDefault(_ => _.Key == "CONF_SITE_LOCALITY").Value);
            Assert.Equal(city, result.FormData.FirstOrDefault(_ => _.Key == "CONF_SITE_TOWN").Value);
            Assert.Equal($"{description} - {furtherLocationInformation}", result.FormData.FirstOrDefault(_ => _.Key == "CONF_LOCATION").Value);
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
        public void ToConfirmIntegrationFormCase_GivenStreetSupplied_AndLocality_ShouldAddDataToCaseFields()
        {
            var usrn = "USRN";
            var name = "Name";
            var locality = "Locality";
            var description = string.Join(',', new string[] { name, locality });
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
            Assert.Equal(name, result.FormData.FirstOrDefault(_ => _.Key == "CONF_SITE_NAME").Value);
            Assert.Equal(locality, result.FormData.FirstOrDefault(_ => _.Key == "CONF_SITE_LOCALITY").Value);
            Assert.Equal(furtherLocationInformation, result.FormData.FirstOrDefault(_ => _.Key == "CONF_LOCATION").Value);
        }

        [Fact]
        public void ToConfirmIntegrationFormCase_GivenStreetSupplied_AndLocalityAndTown_ShouldAddDataToCaseFields()
        {
            var usrn = "USRN";
            var name = "Name";
            var locality = "Locality";
            var town = "Town";
            var description = string.Join(',', new string[] { name, locality, town });
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
            Assert.Equal(name, result.FormData.FirstOrDefault(_ => _.Key == "CONF_SITE_NAME").Value);
            Assert.Equal(locality, result.FormData.FirstOrDefault(_ => _.Key == "CONF_SITE_LOCALITY").Value);
            Assert.Equal(town, result.FormData.FirstOrDefault(_ => _.Key == "CONF_SITE_TOWN").Value);
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
