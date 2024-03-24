using BeirutWalksDomains.Dto;
using BeirutWalksDomains.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace BeirutWalksUI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory clientFactory;

        public RegionsController(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }
        public async Task<IActionResult> Index()
        {
            List<RegionsDto> regions = new List<RegionsDto>();
            try
            {
                var client = clientFactory.CreateClient();
                HttpResponseMessage message = await client.GetAsync("https://localhost:7081/api/regions");
                message.EnsureSuccessStatusCode();
                regions.AddRange(await message.Content.ReadFromJsonAsync<IEnumerable<RegionsDto>>());
                return View(regions);
            }
            catch (Exception)
            {

                return View();
            }
            
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddRegionDto addRegion)
        {
            try
            {
                var client = clientFactory.CreateClient();
                var httpmessage = new HttpRequestMessage()
                {
                    Content = new StringContent(JsonSerializer.Serialize(addRegion), Encoding.UTF8, "application/json"),
                    Headers = { { "Accept", "application/json" } },
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://localhost:7081/api/regions")
                };
                HttpResponseMessage message = await client.SendAsync(httpmessage);
                message.EnsureSuccessStatusCode();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                return View();
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var client = clientFactory.CreateClient();
                HttpResponseMessage message = await client.GetAsync($"https://localhost:7081/api/regions/{id}");
                message.EnsureSuccessStatusCode();
                RegionsDto region = await message.Content.ReadFromJsonAsync<RegionsDto>();
                return View(region);
            }
            catch (Exception)
            {

                return View();
            }
        }
        [HttpPut]
        public async Task<IActionResult> Edit(RegionsDto dto)
        {
            try
            {
                var client = clientFactory.CreateClient();
                var httpmessage = new HttpRequestMessage()
                {
                    Content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json"),
                    Headers = { { "Accept", "application/json" } },
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"https://localhost:7081/api/regions/{dto.Id}")
                };
                HttpResponseMessage message = await client.SendAsync(httpmessage);

                message.EnsureSuccessStatusCode();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                return View();
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var client = clientFactory.CreateClient();
                HttpResponseMessage message = await client.DeleteAsync($"https://localhost:7081/api/regions/{id}");
                message.EnsureSuccessStatusCode();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                return View();
            }
        }
    }
}
