using MeetingApp.Models.Constants;
using MeetingApp.Models.Data;
using MeetingApp.Models.Param;
using MeetingApp.Utils;
using System.Threading.Tasks;

namespace MeetingApp.Models.Validate
{
    public class TokenCheckValidation
    {
        RestService _restService;
        OperateDateTime _operateDateTime;

        public TokenCheckValidation(RestService restService)
        {
            _restService = restService;
        }

        public async Task<TokenCheckParam> Validate(TokenData token)
        {
            var tokenCheckParam = new TokenCheckParam();


            //Localのtoken情報参照
            if (token == null)
            {
                tokenCheckParam.HasError = true;
                tokenCheckParam.NoExistMyToken = true;

            }
            else
            {
                var tokenData = token;
                //DBのtokenと照合するAPIのコール
                tokenCheckParam = await _restService.CheckTokenDataAsync(TokenConstants.OpenTokenEndPoint, tokenData);
            }

            return tokenCheckParam;
        }
    }
}
