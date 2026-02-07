using CsvHelper;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Fizzler;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace CDMTests
{
    public class PersonsControllerIntegrationsTest: IClassFixture<CustomWebApplicationFactory>
    {

        private readonly HttpClient _client;

        public PersonsControllerIntegrationsTest(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        #region Index
        [Fact]
        public async Task Index_ToReturnView()
        {
            //Arrange

            //Aact
            HttpResponseMessage response = await _client.GetAsync("/Persons/Index");

            //Assert
            response.IsSuccessStatusCode.Should().BeTrue(); // 2xx response only but no response body

            string responsBody = await response.Content.ReadAsStringAsync();

            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(responsBody);
            var document = html.DocumentNode;

            document.QuerySelectorAll("table.persons").Should().NotBeNull();
        }

        #endregion
    }
}
