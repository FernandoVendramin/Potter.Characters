using Newtonsoft.Json;
using Potter.Characters.Application.DTOs;
using Potter.Characters.Application.DTOs.Character;
using Potter.Characters.IntegrationTest.Configs;
using Potter.Characters.Utils.Messages;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Potter.Characters.IntegrationTest.Tests
{
    public class CharacterTest : BaseIntegrationTest
    {
        private readonly ITestOutputHelper _outputHelper;
        public CharacterTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #region AuxClasses
        public class DefaultResult_CharacterResponseList
        {
            public List<CharacterResponse> Data { get; set; }
            public bool Success { get; set; }
            public List<string> Messages { get; set; }
        }

        public class DefaultResult_CharacterResponse
        {
            public CharacterResponse Data { get; set; }
            public bool Success { get; set; }
            public List<string> Messages { get; set; }
        }

        public class DefaultResultMessageTest
        {
            public bool Success { get; set; }
            public List<string> Messages { get; set; }
        }
        #endregion

        [Fact]
        public async Task RunCrudTest()
        {
            var characterId = "5a0fa6bbae5bc100213c2334";
            var myCharacter = new CharacterRequest()
            {
                Name = "Phineas Nigellus Black",
                Role = "(Formerly) Headmaster of Hogwarts",
                School = "Hogwarts School of Witchcraft and Wizardry",
                //House = "5a05dc8cd45bd0a11bd5e071"
            };

            await DeleteCharacterTest(characterId);
            await PostCharacterTest(myCharacter);
            myCharacter.House = "5a05dc8cd45bd0a11bd5e071";
            await PutCharacterTest(myCharacter);


            //var getResponse = await GetCharacters();

            //jsonString = await getResponse.Content.ReadAsStringAsync();
            //var getResponseJson = JsonConvert.DeserializeObject<DefaultResult_CharacterResponseList>(jsonString);

            //Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            //getResponse.EnsureSuccessStatusCode();

        }

        private async Task DeleteCharacterTest(string id)
        {
            _outputHelper.WriteLine("Executando Delete");
            var deleteResponse = await _httpClient.DeleteAsync($"/api/v1/Characters?id={id}");

            var jsonString = await deleteResponse.Content.ReadAsStringAsync();
            var deleteResponseJson = JsonConvert.DeserializeObject<DefaultResultMessageTest>(jsonString);

            Assert.Single(deleteResponseJson.Messages);

            if (deleteResponseJson.Success)
                Assert.Contains(deleteResponseJson.Messages, x => x == CommonMessages.ReccordsDeleted);
            else
                Assert.Contains(deleteResponseJson.Messages, x => x == string.Format(CharacterMessages.NotExistsIdInDatabase, id));

            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
        }

        private async Task PostCharacterTest(CharacterRequest characterRequest)
        {
            _outputHelper.WriteLine("Executando Post");
            var postResponse = await _httpClient.PostAsJsonAsync<CharacterRequest>($"/api/v1/Characters", characterRequest);

            var jsonString = await postResponse.Content.ReadAsStringAsync();
            var postResponseJson = JsonConvert.DeserializeObject<DefaultResult_CharacterResponse>(jsonString);

            Assert.True(postResponseJson.Success);
            Assert.Equal(characterRequest.Name, postResponseJson.Data.Name);
            Assert.Equal(characterRequest.Role, postResponseJson.Data.Role);
            Assert.Equal(characterRequest.School, postResponseJson.Data.School);
            Assert.Null(postResponseJson.Data.House);
            Assert.Equal(HttpStatusCode.OK, postResponse.StatusCode);
        }

        private async Task PutCharacterTest(CharacterRequest characterRequest)
        {
            _outputHelper.WriteLine("Executando Put");
            var putResponse = await _httpClient.PutAsJsonAsync<CharacterRequest>($"/api/v1/Characters", characterRequest);

            var jsonString = await putResponse.Content.ReadAsStringAsync();
            var putResponseJson = JsonConvert.DeserializeObject<DefaultResult_CharacterResponse>(jsonString);

            Assert.True(putResponseJson.Success);
            Assert.Equal(characterRequest.Name, putResponseJson.Data.Name);
            Assert.Equal(characterRequest.Role, putResponseJson.Data.Role);
            Assert.Equal(characterRequest.School, putResponseJson.Data.School);
            Assert.Equal(characterRequest.House, putResponseJson.Data.House.Id);
            Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);
        }

        private async Task<HttpResponseMessage> GetCharacters()
        {
            _outputHelper.WriteLine("Executando Get");
            return await _httpClient.GetAsync($"/api/v1/Characters");
        }

        //[Fact, TestPriority(1)]
        //public async Task GetCharacters()
        //{
        //    var data = DateTime.Now;
        //    _outputHelper.WriteLine("Executando Get 1 {0:hh:mm:ss.fff tt}", data);

        //    var response = await _httpClient.GetAsync($"/api/v1/Characters");
        //    response.EnsureSuccessStatusCode();

        //    var resultString = await response.Content.ReadAsStringAsync();
        //    var characterResponse = JsonConvert.DeserializeObject<DefaultResult_CharacterResponse>(resultString);

        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //}

        //[Fact, TestPriority(2)]
        //public async Task GetCharacters2()
        //{
        //    var data = DateTime.Now;
        //    _outputHelper.WriteLine("Executando Get 2 {0:hh:mm:ss.fff tt}", data);

        //    var response = await _httpClient.GetAsync($"/api/v1/Characters");
        //    response.EnsureSuccessStatusCode();

        //    var resultString = await response.Content.ReadAsStringAsync();
        //    var characterResponse = JsonConvert.DeserializeObject<DefaultResult_CharacterResponse>(resultString);

        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //}
    }
}
