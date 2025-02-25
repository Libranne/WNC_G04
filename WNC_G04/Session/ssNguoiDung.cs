﻿using Newtonsoft.Json;
using WNC_G04.Models;

namespace WNC_G04.Session
{
    public class ssNguoiDung
    {
        public void SaveUserToSession(HttpContext context, NguoiDung user)
        {
            var json = JsonConvert.SerializeObject(user); // Sử dụng Newtonsoft.Json
            context.Session.SetString("User", json);
        }
        public NguoiDung GetUserFromSession(HttpContext context)
        {
            var json = context.Session.GetString("User");
            return json != null ? JsonConvert.DeserializeObject<NguoiDung>(json) : null;
        }
    }
}
