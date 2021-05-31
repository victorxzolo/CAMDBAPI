using CampaignAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ESB.WebAppl.CAMDBAPI.Controllers
{
    public class SearchController : ApiController
    {
       
        [HttpPost]
        [Route("api/campaignsearch")]
        public HttpResponseMessage GetCampaignSearch([FromBody] Campaign Camp)
        {
            try {

                if(Camp.CampaignCode == null) {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Not Found Field campaigncode"); }
                if(Camp.CampaignStatus == null) {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Not Found Field campaignstatus"); }

                string sql = "SELECT  ROW_NUMBER() OVER(ORDER BY T74RDT  DESC ) AS ROW,  T74TYP AS CAMPAIGNTYPE, T74CMP AS CAMPAIGNCODE,T74DSC AS CAMPAIGNDESC ," +
                    " T74CTY AS CUSTOMERTYPE , T74BEF AS BENEFIT," +
                     //" VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74SDT),'YYYYMMDD'), 'DD/MM/YYYY') || ' '" +
                     //" || VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74EDT),'YYYYMMDD'), 'DD/MM/YYYY')  AS PERIODDATE ," +
                     "CONCAT(convert(nvarchar(MAX), CONVERT(varchar, CONVERT(datetime,   CONVERT(varchar,T74SDT) ), 103)),' ',convert(nvarchar(MAX), CONVERT(varchar, CONVERT(datetime, CASE WHEN T74EDT != 99999999 THEN  CONVERT(varchar,T74EDT) ELSE  CONVERT(varchar,25991231) END ), 103))) AS PERIODDATE," +
                    " CASE WHEN T74STS = 'A' THEN 'Approve' END AS STATUS" +
                    " FROM RLTB74 WHERE T74CMP LIKE '%" + Camp.CampaignCode.Trim() + "%' and T74STS='"+Camp.CampaignStatus.Trim()+"'";
                DB2Context db2 = new DB2Context();
                //  DataTable dt = db2.GetDataTableAS400(sql);
                DataTable dt = db2.GetDataTableSQL(sql);
                
                string json = JsonConvert.SerializeObject(dt,Formatting.Indented);
                if (dt.Rows.Count > 0)
                {
                    var response = new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(json
                                         , System.Text.Encoding.UTF8,
                                         "application/json"),
                    };
                    return response;
                    // return Request.CreateResponse(HttpStatusCode.OK, dt, Configuration.Formatters.JsonFormatter);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
                }

            } catch (Exception ex)
            {
               
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error" + ex);
                
            }
           
        }


        [HttpPost]
        [Route("api/autosearch")]
        public HttpResponseMessage GetCampaignAuto([FromBody] Campaign Camp)
        {
            try
            {

                if (Camp.CampaignCode == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Not Found Field campaigncode");
                }
                

                string sql = "SELECT  T74CMP AS CAMPAIGNCODE FROM RLTB74" +
                    " WHERE T74CMP LIKE '%" + Camp.CampaignCode.Trim() + "%' ";
                DB2Context db2 = new DB2Context();
                //  DataTable dt = db2.GetDataTableAS400(sql);
                DataTable dt = db2.GetDataTableSQL(sql);
                string json = JsonConvert.SerializeObject(dt, Formatting.Indented);
                if (dt.Rows.Count > 0)
                {
                    var response = new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(json
                                         , System.Text.Encoding.UTF8,
                                         "application/json"),
                    };
                    return response;
                    // return Request.CreateResponse(HttpStatusCode.OK, dt, Configuration.Formatters.JsonFormatter);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
                }

            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error" + ex);

            }

        }
        public class Campaign
        {
            public string CampaignCode { get; set; }
            public string CampaignStatus { get; set; }
        }
        public class Message
        {
            public string messagecode { get; set; }
            public bool messagestatus { get; set; }
            public string messagedetail { get; set; }
            public int numrow { get; set; }

            public campaignlist campaignlist{ get; set; }
          
        }
        public class campaignlist
        {
            public int row { get; set; }
            public string campaigntype { get; set; }
            public string campaigncode { get; set; }
            public string campaigndesc { get; set; }
            public string customertype { get; set; }
            public string benefit { get; set; }
            public string perioddate { get; set; }
            public string campaignstatus { get; set; }

        }

    }
}
