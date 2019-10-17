using System.Threading.Tasks;
using TeslaExplorer.Api;

namespace TeslaExplorer.Models
{
    public class AuthRequestFactory
    {
        public async Task<bool> GetAuthToken(TeslaApi api, string emailAddress, string password)
        {
            var authRequestDto = new AuthRequestDto
            {
                ClientId = api.TESLA_CLIENT_ID,
                ClientSecret = api.TESLA_CLIENT_SECRET,
                Email = emailAddress,
                GrantType = "password",
                Password = password
            };

            var authResponse = await api.AuthRequest(authRequestDto);

            if (authResponse.IsSuccess)
            {
                api.HttpClient.SetAuthorizationHeader(authResponse.Result.AccessToken);
                api.AccessToken = authResponse.Result.AccessToken;
                
                return true;
            }

            return false;
        }
    }
}