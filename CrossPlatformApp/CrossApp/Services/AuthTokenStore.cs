using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossApp.Services.Api
{
    public class AuthTokenStore
    {
        private string? _token;

        public void SetToken(string token)
        {
            _token = token;
        }

        public string? GetToken()
        {
            return _token;
        }

        public void Clear()
        {
            _token = null;
        }
    }
}