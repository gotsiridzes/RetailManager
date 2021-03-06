﻿using RMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RMDesktopUI.Library.API
{
    public class ApiHelper : IApiHelper
    {
        private HttpClient apiClient;
        private ILoggedInUserModel loggedInUser;

        public HttpClient ApiClient 
        { 
            get 
            {
                return apiClient;
            } 
        }

        public ApiHelper(ILoggedInUserModel loggedInUser)
        {
            InitializeClient();
            this.loggedInUser = loggedInUser;
        }

        private void InitializeClient()
        {
            var api = ConfigurationManager.AppSettings["api"];
            apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri(api);
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<User> Authenticate(string username, string password)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });

            using (var response = await apiClient.PostAsync("/Token", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<User>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public void LogOffUser()
        {
            apiClient.DefaultRequestHeaders.Clear();
        }

        public async Task GetLoggedInUserInfo(string token)
        {
            apiClient.DefaultRequestHeaders.Clear();
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer { token }");

            using (var response = await apiClient.GetAsync("api/user"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<LoggedInUserModel>();

                    loggedInUser.Id = result.Id;
                    loggedInUser.Email = result.Email;
                    loggedInUser.FirstName = result.FirstName;
                    loggedInUser.LastName = result.LastName;
                    loggedInUser.CreatedDate = result.CreatedDate;
                    loggedInUser.Token = token;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
