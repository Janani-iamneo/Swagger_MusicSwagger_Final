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
    public class MusicRecordsControllerTests
    {
        private const string MusicRecordServiceName = "MusicRecordService";
        private const string MusicRecordRepositoryName = "MusicRecordRepository";
        private HttpClient _httpClient;
        private Assembly _assembly;

        private MusicRecord _testMusicRecord;

        [SetUp]
        public async Task Setup()
        {
            _assembly = Assembly.GetAssembly(typeof(dotnetapp.Services.IMusicRecordService));
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:8080"); // Base URL of your API

            // Create a new test music record before each test case
            _testMusicRecord = await CreateTestMusicRecord();
        }

        private async Task<MusicRecord> CreateTestMusicRecord()
        {
            var newMusicRecord = new MusicRecord
            {
                Artist = "Test Artist",
                Album = "Test Album",
                Genre = "Test Genre",
                Price = 19.99m,
                StockQuantity = 10
            };

            var json = JsonConvert.SerializeObject(newMusicRecord);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/MusicRecord", content);
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<MusicRecord>(await response.Content.ReadAsStringAsync());
        }

        [Test]
        public async Task Test_GetAllMusicRecords_ReturnsListOfMusicRecords()
        {
            var response = await _httpClient.GetAsync("api/MusicRecord");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var musicRecords = JsonConvert.DeserializeObject<MusicRecord[]>(content);

            Assert.IsNotNull(musicRecords);
            Assert.IsTrue(musicRecords.Length > 0);
        }

        [Test]
        public async Task Test_GetMusicRecordById_ValidId_ReturnsMusicRecord()
        {
            var response = await _httpClient.GetAsync($"api/MusicRecord/{_testMusicRecord.MusicRecordId}");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var musicRecord = JsonConvert.DeserializeObject<MusicRecord>(content);

            Assert.IsNotNull(musicRecord);
            Assert.AreEqual(_testMusicRecord.MusicRecordId, musicRecord.MusicRecordId);
        }

        [Test]
        public async Task Test_GetMusicRecordById_InvalidId_ReturnsNotFound()
        {
            var response = await _httpClient.GetAsync($"api/MusicRecord/999");

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task Test_AddMusicRecord_ReturnsCreatedResponse()
        {
            var newMusicRecord = new MusicRecord
            {
                Artist = "Test Artist",
                Album = "Test Album",
                Genre = "Test Genre",
                Price = 19.99m,
                StockQuantity = 10
            };

            var json = JsonConvert.SerializeObject(newMusicRecord);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/MusicRecord", content);
            response.EnsureSuccessStatusCode();

            var createdMusicRecord = JsonConvert.DeserializeObject<MusicRecord>(await response.Content.ReadAsStringAsync());

            Assert.IsNotNull(createdMusicRecord);
            Assert.AreEqual(newMusicRecord.Artist, createdMusicRecord.Artist);
        }

        [Test]
        public async Task Test_UpdateMusicRecord_ValidId_ReturnsNoContent()
        {
            _testMusicRecord.Artist = "Updated Artist";

            var json = JsonConvert.SerializeObject(_testMusicRecord);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/MusicRecord/{_testMusicRecord.MusicRecordId}", content);

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        public async Task Test_DeleteMusicRecord_ValidId_ReturnsNoContent()
        {
            var response = await _httpClient.DeleteAsync($"api/MusicRecord/{_testMusicRecord.MusicRecordId}");

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        public void Test_MusicRecordService_Exist()
        {
            AssertServiceInstanceNotNull(MusicRecordServiceName);
        }

        [Test]
        public void Test_MusicRecordRepository_Exist()
        {
            AssertRepositoryInstanceNotNull(MusicRecordRepositoryName);
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
            // Delete the test music record after each test case
            if (_testMusicRecord != null)
            {
                var response = await _httpClient.DeleteAsync($"api/MusicRecord/{_testMusicRecord.MusicRecordId}");
                response.EnsureSuccessStatusCode();
            }
            _httpClient.Dispose();
        }
    }
}
