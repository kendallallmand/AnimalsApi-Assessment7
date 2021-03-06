﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Assessment7a.Models
{
    public class AniDal
    {

        public static List<KeyValuePair<string, string>> BuildApiArguments(params string[] pairs)
        {
            if (pairs.Length % 2 == 1)
            {
                return null;
            }

            var ret = new List<KeyValuePair<string, string>>();

            for (int i = 0; i < pairs.Length; i += 2)
            {
                ret.Add(new KeyValuePair<string, string>(pairs[i], pairs[i + 1]));
            }

            return ret;
        }

        public static async Task<List<T>> GetApiResponseList<T>(string controller, string action, string baseUrl, string SpeciesName,
    params KeyValuePair<string, string>[] options) where T : new()
        {
            string url = $"{baseUrl}/" +
                         $"{controller}/" +
                         $"{action}/" +
                         $"{SpeciesName}";

            bool first = true;
            foreach (KeyValuePair<string, string> argument in options)
            {
                url += first ? "?" : "&";
                url += $"{argument.Key}={Uri.EscapeDataString(argument.Value)}";
                first = false;
            }

            HttpWebRequest request = WebRequest.CreateHttp(url);
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException)
            {
                return null;
            }

            Stream s = response.GetResponseStream();
            if (s == null)
            {
                return null;
            }

            StreamReader rd = new StreamReader(s);

            string output = await rd.ReadToEndAsync();
            List<T> ret;
            try
            {
                ret = JsonConvert.DeserializeObject<List<T>>(output);
            }
            catch (JsonSerializationException) 
            {
                ret = new List<T> { JsonConvert.DeserializeObject<T>(output) };
            }

            return ret;
        }

        public static async Task<List<T>> GetApiResponseList<T>(string controller, string action, string baseUrl, string SpeciesName, params string[] options)
            where T : new()
        {
            var keyValuePairs = AniDal.BuildApiArguments(options);
            if (keyValuePairs == null && options.Length > 0)
            {
                return new List<T>();
            }

            return await GetApiResponseList<T>(controller, action, baseUrl, SpeciesName,
                (keyValuePairs ?? new List<KeyValuePair<string, string>>()).ToArray());
        }
        public static async Task<List<T>> GetApiResponseList<T>(string controller, string action, string baseUrl, string SpeciesName)
            where T : new()
        {
            return await GetApiResponseList<T>(controller, action, baseUrl, SpeciesName,
                (new List<KeyValuePair<string, string>>()).ToArray());
        }

        public static async Task<T> GetApiResponse<T>(string controller, string action, string baseUrl, string SpeciesName)
    where T : new()
        {
            return (await GetApiResponseList<T>(controller, action, baseUrl, SpeciesName,
                (new List<KeyValuePair<string, string>>()).ToArray())).FirstOrDefault();
        }

        public static async Task<T> GetApiResponse<T>(string controller, string action, string baseUrl, string SpeciesName, params string[] options)
            where T : new()
        {
            return (await GetApiResponseList<T>(controller, action, baseUrl, SpeciesName, options)).FirstOrDefault();
        }
    }
}
