using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using dotnetapp.Models;
using System.Reflection;


namespace dotnetapp.Tests
{
    [TestFixture]
    public class MobilePhonesControllerTests
    {
        private const string MobilePhoneServiceName = "MobilePhoneService";
        private const string MobilePhoneRepositoryName = "MobilePhoneRepository";
        private HttpClient _httpClient;
        private Assembly _assembly;

        private MobilePhone _testMobilePhone;


        [SetUp]
        public async Task Setup()
        {
            _assembly = Assembly.GetAssembly(typeof(dotnetapp.Services.IMobilePhoneService));
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:8080"); // Base URL of your API

            // Create a new test mobilePhone before each test case
            _testMobilePhone = await CreateTestMobilePhone();
        }

        private async Task<MobilePhone> CreateTestMobilePhone()
        {
            var newMobilePhone = new MobilePhone
            {
                Brand = "Test Brand",
                Model = "Test Model",
                Price = 199.99m,
                StockQuantity = 10
            };

            var json = JsonConvert.SerializeObject(newMobilePhone);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/MobilePhone", content);
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<MobilePhone>(await response.Content.ReadAsStringAsync());
        }

        [Test]
        public async Task Test_GetAllMobilePhones_ReturnsListOfMobilePhones()
        {
            var response = await _httpClient.GetAsync("api/MobilePhone");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var mobilePhones = JsonConvert.DeserializeObject<MobilePhone[]>(content);

            Assert.IsNotNull(mobilePhones);
            Assert.IsTrue(mobilePhones.Length > 0);
        }

        [Test]
        public async Task Test_GetMobilePhoneById_ValidId_ReturnsMobilePhone()
        {
            var response = await _httpClient.GetAsync($"api/MobilePhone/{_testMobilePhone.MobilePhoneId}");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var mobilePhone = JsonConvert.DeserializeObject<MobilePhone>(content);

            Assert.IsNotNull(mobilePhone);
            Assert.AreEqual(_testMobilePhone.MobilePhoneId, mobilePhone.MobilePhoneId);
        }

        [Test]
        public async Task Test_GetMobilePhoneById_InvalidId_ReturnsNotFound()
        {
            var response = await _httpClient.GetAsync($"api/MobilePhone/999");

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task Test_AddMobilePhone_ReturnsCreatedResponse()
        {
            var newMobilePhone = new MobilePhone
            {
                Brand = "Test Brand",
                Model = "Test Model",
                Price = 199.99m,
                StockQuantity = 10
            };

            var json = JsonConvert.SerializeObject(newMobilePhone);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/MobilePhone", content);
            response.EnsureSuccessStatusCode();

            var createdMobilePhone = JsonConvert.DeserializeObject<MobilePhone>(await response.Content.ReadAsStringAsync());

            Assert.IsNotNull(createdMobilePhone);
            Assert.AreEqual(newMobilePhone.Brand, createdMobilePhone.Brand);
        }

        [Test]
        public async Task Test_UpdateMobilePhone_ValidId_ReturnsNoContent()
        {
            _testMobilePhone.Brand = "Updated Brand";

            var json = JsonConvert.SerializeObject(_testMobilePhone);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/MobilePhone/{_testMobilePhone.MobilePhoneId}", content);

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        public async Task Test_DeleteMobilePhone_ValidId_ReturnsNoContent()
        {
            var response = await _httpClient.DeleteAsync($"api/MobilePhone/{_testMobilePhone.MobilePhoneId}");

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }
          

        [Test]
        public void Test_MobilePhoneService_Exist()
        {
            AssertServiceInstanceNotNull(MobilePhoneServiceName);
        }

       

        [Test]
        public void Test_MobilePhoneRepository_Exist()
        {
            AssertRepositoryInstanceNotNull(MobilePhoneRepositoryName);
        }


        private void AssertServiceInstanceNotNull(string serviceName)
        {
            Type serviceType = _assembly.GetType($"dotnetapp.Services.{serviceName}");

            if (serviceType == null)
            {
                Assert.Fail($"Service {serviceName} does not exist.");
            }

            object serviceInstance = Activator.CreateInstance(serviceType);
            Assert.IsNotNull(serviceInstance);
        }

        private void AssertRepositoryInstanceNotNull(string repositoryName)
        {
            Type repositoryType = _assembly.GetType($"dotnetapp.Repository.{repositoryName}");

            if (repositoryType == null)
            {
                Assert.Fail($"Repository {repositoryName} does not exist.");
            }

            object repositoryInstance = Activator.CreateInstance(repositoryType);
            Assert.IsNotNull(repositoryInstance);
        }


        [TearDown]
        public async Task Cleanup()
        {
            // Delete the test mobilePhone after each test case
            if (_testMobilePhone != null)
            {
                var response = await _httpClient.DeleteAsync($"api/MobilePhone/{_testMobilePhone.MobilePhoneId}");
                response.EnsureSuccessStatusCode();
            }
            _httpClient.Dispose();
        }
    }
}

