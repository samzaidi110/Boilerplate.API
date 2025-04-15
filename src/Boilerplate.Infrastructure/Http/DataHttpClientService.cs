using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Boilerplate.Domain.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ThirdParty.Json.LitJson;

namespace Boilerplate.Infrastructure.Http;
public class DataHttpClientService
{
    private readonly HttpServiceSetting httpServiceSettings;
    private readonly HttpClient _httpClient;

    public DataHttpClientService(IOptions<HttpServiceSetting> settings, HttpClient httpClient)
    {

        httpServiceSettings = settings.Value;
        _httpClient = httpClient;

        _httpClient.BaseAddress = new Uri($"{httpServiceSettings!.BaseUrl}" );

        // Setup required Headers.
        //for example 'Content-type': 'application/x-www-form-urlencoded',
        _httpClient.DefaultRequestHeaders.Add(
            HeaderNames.Authorization, $"Bearer {httpServiceSettings.ClientId}");
    }

    public async Task<EmployeeDateResponse?> GetEmployeeData(EmployeeDateRequest requestBody)
    {

        var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

        var response =   await _httpClient.PostAsync($"{httpServiceSettings.BaseUrl}/public/my_procedure", content)
                .ConfigureAwait(false);
        var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        var httpServiceTokenResponseDto = JsonConvert.DeserializeObject<EmployeeDateResponse>(
                            responseJson);

        return httpServiceTokenResponseDto;
    }
}