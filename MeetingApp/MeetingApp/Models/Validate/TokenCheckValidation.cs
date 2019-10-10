using MeetingApp.Models.Constants;
using MeetingApp.Models.Data;
using MeetingApp.Models.Param;
using MeetingApp.Utils;
using System.Threading.Tasks;

namespace MeetingApp.Models.Validate
{
    class TokenCheckValidation
    {
        ApplicationProperties _applicationProperties;
        RestService _restService;

        public async Task<TokenCheckParam> Validate()
        {
            TokenCheckParam tokenCheckParam = new TokenCheckParam();
            _applicationProperties = new ApplicationProperties();
            _restService = new RestService();


            //Localのtoken情報参照
            if (_applicationProperties.GetFromProperties<TokenData>("token") == null)
            {
                tokenCheckParam.HasError = true;
                tokenCheckParam.NoExistMyToken = true;

            }
            else
            {
                var tokenData = _applicationProperties.GetFromProperties<TokenData>("token");
                //DBのtokenと照合するAPIのコール
                tokenCheckParam = await _restService.CheckTokenDataAsync(TokenConstants.OpenTokenEndPoint, tokenData);
            }

            return tokenCheckParam;
        }
    }
}
