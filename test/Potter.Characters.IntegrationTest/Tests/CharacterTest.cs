using Newtonsoft.Json;
using Potter.Characters.Application.DTOs.Character;
using Potter.Characters.IntegrationTest.Configs;
using Potter.Characters.Utils.Messages;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        /* 
         * Foi necessário criar as classes abaixo pois o JsonConvert.DeserializeObject
         * não consegue deserializar o conteúdo do Data, provávelmente por set um tipo 
         * especificado na implementação dos objetos DefaultResult e DefaultResultMessage
         */

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
                School = "Hogwarts School of Witchcraft and Wizardry"
            };

            await DeleteCharacterTest(characterId);
            await PostCharacterTest(myCharacter);

            myCharacter.House = "5a05dc8cd45bd0a11bd5e071";

            await PutCharacterTest(myCharacter);
            await GetCharacters(myCharacter);
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

        private async Task GetCharacters(CharacterRequest characterRequest)
        {
            _outputHelper.WriteLine("Executando Get");
            var getResponse = await _httpClient.GetAsync($"/api/v1/Characters?name={characterRequest.Name}");

            var jsonString = await getResponse.Content.ReadAsStringAsync();
            var getResponseJson = JsonConvert.DeserializeObject<DefaultResult_CharacterResponseList>(jsonString);

            Assert.True(getResponseJson.Success);
            Assert.Single(getResponseJson.Data);
            Assert.Equal(characterRequest.Name, getResponseJson.Data.FirstOrDefault().Name);
            Assert.Equal(characterRequest.Role, getResponseJson.Data.FirstOrDefault().Role);
            Assert.Equal(characterRequest.School, getResponseJson.Data.FirstOrDefault().School);
            Assert.Equal(characterRequest.House, getResponseJson.Data.FirstOrDefault().House.Id);
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        }
    }
}
