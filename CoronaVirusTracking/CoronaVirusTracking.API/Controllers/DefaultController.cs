using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CoronaVirusTracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        private const string CONFIRMED_GLOBAL_URL = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv";
        private const string DEATHS_GLOBAL_URL = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_deaths_global.csv";
        private const string RECOVERED_GLOBAL_URL = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_recovered_global.csv";

        private async Task<ActionResult<IList<RecordEntry>>> _getRecordEntries(string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                var responseData = await response.Content.ReadAsStringAsync();
                string[] rows = responseData.Split('\n', StringSplitOptions.RemoveEmptyEntries);

                Dictionary<string, int> dateHeaderColumns = new Dictionary<string, int>();
                List<RecordEntry> recordEntries = new List<RecordEntry>();

                string[] headerColumns = rows[0].Split(',');
                for (int i = 4; i < headerColumns.Length; i++)
                {
                    dateHeaderColumns.Add(headerColumns[i], 0);
                }

                for (int i = 1; i < rows.Length; i++)
                {
                    string[] columns = rows[i].Split(new char[] { ',' });
                    RecordEntry recordEntry = new RecordEntry();
                    int k = 0;
                    for (int j = 4; j < columns.Length; j++)
                    {
                        try
                        {
                            recordEntry.DateEntry.Add(dateHeaderColumns.ElementAt(k).Key, Convert.ToInt32(columns[j]));
                        }

                        catch
                        {
                            //return null;
                        }

                        k++;
                    }

                    recordEntry.ProvinceState = columns[0];
                    recordEntry.CountryRegion = columns[1];
                    try
                    {
                        recordEntry.Lat = Convert.ToDouble(columns[2]);
                    }

                    catch
                    {

                    }

                    try
                    {
                        recordEntry.Long = Convert.ToDouble(columns[3]);
                    }

                    catch
                    {

                    }

                    recordEntries.Add(recordEntry);
                }

                return recordEntries;
            }
        }

        [HttpGet]
        public async Task<ActionResult<IList<RecordEntry>>> GetConfirmedCases()
        {
            return await _getRecordEntries(CONFIRMED_GLOBAL_URL);
        }

        [HttpGet]
        public async Task<ActionResult<IList<RecordEntry>>> GetConfirmedDeaths()
        {
            return await _getRecordEntries(DEATHS_GLOBAL_URL);
        }        
        
        [HttpGet("{id}")]
        public async Task<ActionResult<IList<RecordEntry>>> GetConfirmedRecovered()
        {
            return await _getRecordEntries(RECOVERED_GLOBAL_URL);
        }
    }
}