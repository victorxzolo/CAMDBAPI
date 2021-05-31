using CampaignAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ESB.WebAppl.CAMDBAPI.Controllers
{
    public class CampaignDetailController : ApiController
    {
        [HttpPost]
        [Route("api/CampDetail")]
        public HttpResponseMessage GetCampaignSearch([FromBody] Campaign Camp)
        {
            try
            {
                DB2Context db2 = new DB2Context();
                if (Camp.CampaignCode == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Not Found Field campaigncode");
                }
                string Campaigntype = "";
                string Benefit = "";
                string ActionType = "";
                string sql = "";
                //GetDataTableAS400
                DataTable dtType = db2.GetDataTableSQL("SELECT T74TYP , T74BEF , T74ACT FROM RLTB74 WHERE T74CMP = '"+Camp.CampaignCode.Trim()+"'");
                foreach (DataRow item in dtType.Rows)
                {
                    Campaigntype = item["T74TYP"].ToString();
                    Benefit = item["T74BEF"].ToString();
                    ActionType = item["T74ACT"].ToString();
                }
               
                if ((Campaigntype == "FWD" || Campaigntype == "ACT" || Campaigntype == "MEX") && Benefit == "PM")
                    {

                    //sql = @"SELECT  T74TYP AS CAMPAIGNTYPE, T76NME AS CAMPAIGNTYPEDESC, T74CMP AS CAMPAIGNCODE,T74DSC AS CAMPAIGNDESC , 
                    //        T74BEF AS BENEFIT, T74CTY AS CUSTOMERTYPE , T74CDT AS CARDTYPE, T74ACT AS ACTIONTYPE, T74WAM AS WITHDRAWAMOUNT,
                    //        T74WDT AS WITHINDAY, T74OSB AS REMAINOB ,T74BDY AS PERIODOB,  T76LAB AS PROCESSLABEL,
                    //        T74RUS AS REGISTERBY, T74UPU AS APPROVEBY,
                    //        VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74LBE), 'YYYYMMDD'), 'DD/MM/YYYY') AS LASTDATE,
                    //        VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74RDT), 'YYYYMMDD'), 'DD/MM/YYYY') AS REGISTERDATE,
                    //        VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74UPD), 'YYYYMMDD'), 'DD/MM/YYYY') AS UPDATEDATE,
                    //        VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74SDT), 'YYYYMMDD'), 'DD/MM/YYYY') || '   ' || VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74EDT), 'YYYYMMDD'), 'DD/MM/YYYY')AS PERIODDATE
                    //        FROM RLTB74 INNER JOIN RLTB76 ON RLTB76.T76TYP = RLTB74.T74TYP AND T76ACT = T74ACT AND T76BEF = T74BEF
                    //        WHERE T74CMP = '" + Camp.CampaignCode.Trim() + "'";

                    sql = @"SELECT  T74TYP AS CAMPAIGNTYPE, T76NME AS CAMPAIGNTYPEDESC, T74CMP AS CAMPAIGNCODE,T74DSC AS CAMPAIGNDESC , 
                            T74BEF AS BENEFIT, T74CTY AS CUSTOMERTYPE , T74CDT AS CARDTYPE, T74ACT AS ACTIONTYPE, T74WAM AS WITHDRAWAMOUNT,
                            T74WDT AS WITHINDAY, T74OSB AS REMAINOB ,T74BDY AS PERIODOB,  T76LAB AS PROCESSLABEL,T74RUS AS REGISTERBY, T74UPU AS APPROVEBY,
                            CONVERT(varchar, CONVERT(datetime, CASE WHEN T74LBE != 0 THEN  CONVERT(varchar,T74LBE) END ), 103) AS LASTDATE,
							CONVERT(varchar, CONVERT(datetime, CASE WHEN T74RDT != 0 THEN  CONVERT(varchar,T74RDT) END ), 103) AS REGISTERDATE ,
							CONVERT(varchar, CONVERT(datetime, CASE WHEN T74UPD != 0 THEN  CONVERT(varchar,T74UPD) END ), 103) AS UPDATEDATE ,
							CONCAT(convert(nvarchar(MAX), CONVERT(varchar, CONVERT(datetime,   CONVERT(varchar,T74SDT) ), 103)),' ',convert(nvarchar(MAX), CONVERT(varchar, CONVERT(datetime, CASE WHEN T74EDT != 99999999 THEN  CONVERT(varchar,T74EDT) ELSE  CONVERT(varchar,25991231) END ), 103))) AS PERIODDATE 
						    FROM RLTB74 INNER JOIN RLTB76 ON RLTB76.T76TYP = RLTB74.T74TYP AND T76ACT = T74ACT AND T76BEF = T74BEF
                            WHERE T74CMP = '" + Camp.CampaignCode.Trim() + "'";

                }
                else if (Campaigntype == "FWD" && Benefit == "RT")
                {
                    //sql ="SELECT T74TYP AS CAMPAIGNTYPE, T76NME AS CAMPAIGNTYPEDESC, T74CMP AS CAMPAIGNCODE," +
                    //    "T74DSC AS CAMPAIGNDESC , T74BEF AS BENEFIT, T74CTY AS CUSTOMERTYPE , T74CDT AS CARDTYPE, T74ACT AS ACTIONTYPE," +
                    //    " T74RTE AS BINT ,T74CRU AS CRU, 0 AS BILL, T74DAY AS BDAY, T74RUS AS REGISTERBY, T74UPU AS APPROVEBY," +
                    //    " VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74RDT), 'YYYYMMDD'), 'DD/MM/YYYY') AS REGISTERDATE, " +
                    //    "VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74UPD), 'YYYYMMDD'), 'DD/MM/YYYY') AS UPDATEDATE," +
                    //    "VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74SDT), 'YYYYMMDD'), 'DD/MM/YYYY') || '   ' || VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74EDT), 'YYYYMMDD'), 'DD/MM/YYYY')AS PERIODDATE" +
                    //    " FROM RLTB74 INNER JOIN RLTB76 ON RLTB76.T76TYP = RLTB74.T74TYP AND T76ACT = T74ACT AND T76BEF = T74BEF" +
                    //    " WHERE T74CMP = '" + Camp.CampaignCode.Trim() + "'";


                    sql = @" SELECT T74TYP AS CAMPAIGNTYPE, T76NME AS CAMPAIGNTYPEDESC, T74CMP AS CAMPAIGNCODE,
                            T74DSC AS CAMPAIGNDESC , T74BEF AS BENEFIT, T74CTY AS CUSTOMERTYPE , T74CDT AS CARDTYPE, T74ACT AS ACTIONTYPE,
                            T74RTE AS BINT ,T74CRU AS CRU, 0 AS BILL, T74DAY AS BDAY, T74RUS AS REGISTERBY, T74UPU AS APPROVEBY,
				            CONVERT(varchar, CONVERT(datetime, CASE WHEN T74RDT != 0 THEN  CONVERT(varchar,T74RDT) END ), 103) AS REGISTERDATE,
				            CONVERT(varchar, CONVERT(datetime, CASE WHEN T74UPD != 0 THEN  CONVERT(varchar,T74UPD) END ), 103) AS UPDATEDATE,
                            CONCAT(convert(nvarchar(MAX), CONVERT(varchar, CONVERT(datetime,   CONVERT(varchar,T74SDT) ), 103)),' ',convert(nvarchar(MAX), CONVERT(varchar, CONVERT(datetime, CASE WHEN T74EDT != 99999999 THEN  CONVERT(varchar,T74EDT) ELSE  CONVERT(varchar,20991231) END ), 103))) AS PERIODDATE
                            FROM RLTB74 INNER JOIN RLTB76 ON RLTB76.T76TYP = RLTB74.T74TYP AND T76ACT = T74ACT AND T76BEF = T74BEF 
                            WHERE T74CMP = '" + Camp.CampaignCode.Trim() + "'";

                }
                else if(Campaigntype == "MGM" || Campaigntype =="PGP")
                {
                    //sql = @"SELECT  T74TYP AS CAMPAIGNTYPE, T76NME AS CAMPAIGNTYPEDESC, T74CMP AS CAMPAIGNCODE,T74DSC AS CAMPAIGNDESC , 
                    //    T74BEF AS BENEFIT, T74CTY AS CUSTOMERTYPE , T74CDT AS CARDTYPE, T74ACT AS ACTIONTYPE, 
                    //    VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74EXD), 'YYYYMMDD'), 'DD/MM/YYYY') AS EXPIRERECEIVE , T74SPC AS CAMPTYPE,
                    //    T74RUS AS REGISTERBY, T74UPU AS APPROVEBY, 
                    //    VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74RDT), 'YYYYMMDD'), 'DD/MM/YYYY') AS REGISTERDATE,
                    //    VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74UPD), 'YYYYMMDD'), 'DD/MM/YYYY') AS UPDATEDATE,
                    //    VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74SDT), 'YYYYMMDD'), 'DD/MM/YYYY') || '   ' || VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74EDT), 'YYYYMMDD'), 'DD/MM/YYYY')AS PERIODDATE 
                    //    FROM RLTB74 INNER JOIN RLTB76 ON RLTB76.T76TYP = RLTB74.T74TYP AND T76ACT = T74ACT AND T76BEF = T74BEF AND T74PCD = T76PCD
                    //    WHERE T74CMP = '" + Camp.CampaignCode.Trim() + "'";
                    sql = @"
			               SELECT  T74TYP AS CAMPAIGNTYPE, T76NME AS CAMPAIGNTYPEDESC, T74CMP AS CAMPAIGNCODE,T74DSC AS CAMPAIGNDESC , 
                                T74BEF AS BENEFIT, T74CTY AS CUSTOMERTYPE , T74CDT AS CARDTYPE, T74ACT AS ACTIONTYPE, 
					            T74RUS AS REGISTERBY, T74UPU AS APPROVEBY, T74SPC AS CAMPTYPE,
                                CONVERT(varchar, CONVERT(datetime, CASE WHEN T74EXD != 0 THEN  CONVERT(varchar,T74EXD) END ), 103) AS EXPIRERECEIVE,
                    	        CONVERT(varchar, CONVERT(datetime, CASE WHEN T74RDT != 0 THEN  CONVERT(varchar,T74RDT) END ), 103) AS REGISTERDATE,
				                CONVERT(varchar, CONVERT(datetime, CASE WHEN T74UPD != 0 THEN  CONVERT(varchar,T74UPD) END ), 103) AS UPDATEDATE,
                                CONCAT(convert(nvarchar(MAX), CONVERT(varchar, CONVERT(datetime,   CONVERT(varchar,T74SDT) ), 103)),' ',convert(nvarchar(MAX), CONVERT(varchar, CONVERT(datetime, CASE WHEN T74EDT != 99999999 THEN  CONVERT(varchar,T74EDT) ELSE  CONVERT(varchar,20991231) END ), 103))) AS PERIODDATE
                                        FROM RLTB74 INNER JOIN RLTB76 ON RLTB76.T76TYP = RLTB74.T74TYP AND T76ACT = T74ACT AND T76BEF = T74BEF AND T74PCD = T76PCD
                                        WHERE T74CMP = '" + Camp.CampaignCode.Trim() + "'";

                }
                else if (Campaigntype == "NON" || Campaigntype == "NVR")
                {
                    //sql = @"SELECT  T74TYP AS CAMPAIGNTYPE, T76NME AS CAMPAIGNTYPEDESC, T74CMP AS CAMPAIGNCODE,T74DSC AS CAMPAIGNDESC , 
                    //    T74BEF AS BENEFIT, T74CTY AS CUSTOMERTYPE ,  T74ACT AS ACTIONTYPE,
                    //    '' AS TARGETQUALIFICATION , '' AS EXCEPTIONCAMP,
                    //    VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74GDT), 'YYYYMMDD'), 'DD/MM/YYYY') AS GENTARGETDATE, 
                    //    VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74RWS), 'YYYYMMDD'), 'DD/MM/YYYY') AS WITHDRAWDATESTART, 
                    //    VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74RWE), 'YYYYMMDD'), 'DD/MM/YYYY') AS WITHDRAWDATEEND, 
                    //    VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74RDT), 'YYYYMMDD'), 'DD/MM/YYYY') AS REGISTERDATE, 
                    //    T74RTE AS BINT , T74CRU AS CRU, 0 AS BILL, T74DAY AS BDAY, 
                    //    '' AS ENDDATE, 
                    //    T74RUS AS REGISTERBY, T74UPU AS APPROVEBY,
                    //    VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74UPD), 'YYYYMMDD'), 'DD/MM/YYYY') AS UPDATEDATE,
                    //    VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74SDT), 'YYYYMMDD'), 'DD/MM/YYYY') || '   ' || VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74EDT), 'YYYYMMDD'), 'DD/MM/YYYY')AS PERIODDATE 
                    //    FROM RLTB74 INNER JOIN RLTB76 ON RLTB76.T76TYP = RLTB74.T74TYP AND T76ACT = T74ACT AND T76BEF = T74BEF
                    //    WHERE T74CMP = '" + Camp.CampaignCode.Trim() + "'";
                    sql = @" 
                        SELECT  T74TYP AS CAMPAIGNTYPE, T76NME AS CAMPAIGNTYPEDESC, T74CMP AS CAMPAIGNCODE,T74DSC AS CAMPAIGNDESC , 
                                T74BEF AS BENEFIT, T74CTY AS CUSTOMERTYPE ,  T74ACT AS ACTIONTYPE, '' AS TARGETQUALIFICATION , '' AS EXCEPTIONCAMP,                 
					   CONVERT(varchar, CONVERT(datetime, CASE WHEN T74GDT != 0 THEN  CONVERT(varchar,T74GDT) END ), 103) AS GENTARGETDATE,       
					   CONVERT(varchar, CONVERT(datetime, CASE WHEN T74RWS != 0 THEN  CONVERT(varchar,T74RWS) END ), 103) AS WITHDRAWDATESTART,                     
					   CONVERT(varchar, CONVERT(datetime, CASE WHEN T74RWE != 0 THEN  CONVERT(varchar,T74RWE) END ), 103) AS WITHDRAWDATEEND,                   
					   CONVERT(varchar, CONVERT(datetime, CASE WHEN T74RDT != 0 THEN  CONVERT(varchar,T74RDT) END ), 103) AS REGISTERDATE,
                       T74RTE AS BINT , T74CRU AS CRU, 0 AS BILL, T74DAY AS BDAY, 
                       '' AS ENDDATE, 
                       T74RUS AS REGISTERBY, T74UPU AS APPROVEBY,
					    CONVERT(varchar, CONVERT(datetime, CASE WHEN T74UPD != 0 THEN  CONVERT(varchar,T74UPD) END ), 103) AS REGISTERDATE,
                       CONCAT(convert(nvarchar(MAX), CONVERT(varchar, CONVERT(datetime,   CONVERT(varchar,T74SDT) ), 103)),' ',convert(nvarchar(MAX), CONVERT(varchar, CONVERT(datetime, CASE WHEN T74EDT != 99999999 THEN  CONVERT(varchar,T74EDT) ELSE  CONVERT(varchar,20991231) END ), 103))) AS PERIODDATE
                       FROM RLTB74 INNER JOIN RLTB76 ON RLTB76.T76TYP = RLTB74.T74TYP AND T76ACT = T74ACT AND T76BEF = T74BEF
                            WHERE T74CMP = '" + Camp.CampaignCode.Trim() + "'";
                }
                else if(Campaigntype == "SPC")
                {
                    if(Benefit == "GV") {

                        if (ActionType == "WK")
                        {
                            //    sql = @"SELECT  T74TYP AS CAMPAIGNTYPE, T76NME AS CAMPAIGNTYPEDESC, T74CMP AS CAMPAIGNCODE,T74DSC AS CAMPAIGNDESC , 
                            //T74BEF AS BENEFIT, T74CTY AS CUSTOMERTYPE ,  T74ACT AS ACTIONTYPE,
                            //VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74GDT), 'YYYYMMDD'), 'DD/MM/YYYY') AS GENTARGETDATE, 
                            //VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74EXD), 'YYYYMMDD'), 'DD/MM/YYYY') AS EXPIREGVDATE, T74GVA AS BENEFITGVAMOUNT,
                            //T74WDM AS WITHDRAWAMOUNT, T74KOB AS KEEPOB, 
                            //VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74KDT), 'YYYYMMDD'), 'DD/MM/YYYY') AS UNITDATE, 
                            //VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74WDS), 'YYYYMMDD'), 'DD/MM/YYYY') || ' ' || VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74WDE), 'YYYYMMDD'), 'DD/MM/YYYY') AS WITHDRAWDATE, 
                            //VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74RDT), 'YYYYMMDD'), 'DD/MM/YYYY') AS REGISTERDATE, 
                            //T74RUS AS REGISTERBY, T74UPU AS APPROVEBY,
                            //VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74UPD), 'YYYYMMDD'), 'DD/MM/YYYY') AS UPDATEDATE,
                            //VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74SDT), 'YYYYMMDD'), 'DD/MM/YYYY') || '   ' || VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74EDT), 'YYYYMMDD'), 'DD/MM/YYYY')AS PERIODDATE 
                            //FROM RLTB74 INNER JOIN RLTB76 ON RLTB76.T76TYP = RLTB74.T74TYP AND T76ACT = T74ACT AND T76BEF = T74BEF 
                            //WHERE T74CMP = '" + Camp.CampaignCode.Trim() + "'";
                            sql = @"
                            SELECT  T74TYP AS CAMPAIGNTYPE, T76NME AS CAMPAIGNTYPEDESC, T74CMP AS CAMPAIGNCODE,T74DSC AS CAMPAIGNDESC , 
                            T74BEF AS BENEFIT, T74CTY AS CUSTOMERTYPE ,  T74ACT AS ACTIONTYPE,
							T74WDM AS WITHDRAWAMOUNT, T74KOB AS KEEPOB, T74GVA AS BENEFITGVAMOUNT, T74RUS AS REGISTERBY, T74UPU AS APPROVEBY,
							CONVERT(varchar, CONVERT(datetime, CASE WHEN T74GDT != 0 THEN  CONVERT(varchar,T74GDT) END ), 103) AS GENTARGETDATE,  
                            CONVERT(varchar, CONVERT(datetime, CASE WHEN T74EXD != 0 THEN  CONVERT(varchar,T74EXD) END ), 103) AS EXPIREGVDATE, 
							CONVERT(varchar, CONVERT(datetime, CASE WHEN T74KDT != 0 THEN  CONVERT(varchar,T74KDT) END ), 103) AS UNITDATE, 
							CONCAT(convert(nvarchar(MAX), CONVERT(varchar, CONVERT(datetime,  CASE WHEN T74WDS != 0 THEN  CONVERT(varchar,T74WDS) END ), 103)),' ',convert(nvarchar(MAX), CONVERT(varchar, CONVERT(datetime, CASE WHEN T74WDE != 0 THEN  CONVERT(varchar,T74WDE)  END ), 103))) AS WITHDRAWDATE,
							CONVERT(varchar, CONVERT(datetime, CASE WHEN T74RDT != 0 THEN  CONVERT(varchar,T74RDT) END ), 103) AS REGISTERDATE,
							CONVERT(varchar, CONVERT(datetime, CASE WHEN T74UPD != 0 THEN  CONVERT(varchar,T74UPD) END ), 103) AS UPDATEDATE,
                            CONCAT(convert(nvarchar(MAX), CONVERT(varchar, CONVERT(datetime,   CONVERT(varchar,T74SDT) ), 103)),' ',convert(nvarchar(MAX), CONVERT(varchar, CONVERT(datetime, CASE WHEN T74EDT != 99999999 THEN  CONVERT(varchar,T74EDT) ELSE  CONVERT(varchar,20991231) END ), 103))) AS PERIODDATE
                            FROM RLTB74 INNER JOIN RLTB76 ON RLTB76.T76TYP = RLTB74.T74TYP AND T76ACT = T74ACT AND T76BEF = T74BEF 
                               WHERE T74CMP = '" + Camp.CampaignCode.Trim() + "'";

                        }
                        else if(ActionType == "WT")
                        {
                            //    sql = @"SELECT  T74TYP AS CAMPAIGNTYPE, T76NME AS CAMPAIGNTYPEDESC, T74CMP AS CAMPAIGNCODE,T74DSC AS CAMPAIGNDESC , 
                            //T74BEF AS BENEFIT, T74CTY AS CUSTOMERTYPE ,  T74ACT AS ACTIONTYPE,
                            //VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74GDT), 'YYYYMMDD'), 'DD/MM/YYYY') AS GENTARGETDATE, 
                            //VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74EXD), 'YYYYMMDD'), 'DD/MM/YYYY') AS EXPIREGVDATE, T74GVA AS BENEFITGVAMOUNT, T74WDM AS WITHDRAWAMOUNT,
                            //VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74WDS), 'YYYYMMDD'), 'DD/MM/YYYY') || ' ' || VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74WDE), 'YYYYMMDD'), 'DD/MM/YYYY') AS WITHDRAWDATE, 
                            //VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74RDT), 'YYYYMMDD'), 'DD/MM/YYYY') AS REGISTERDATE, 
                            //T74RUS AS REGISTERBY, T74UPU AS APPROVEBY,
                            //VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74UPD), 'YYYYMMDD'), 'DD/MM/YYYY') AS UPDATEDATE,
                            //VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74SDT), 'YYYYMMDD'), 'DD/MM/YYYY') || '   ' || VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74EDT), 'YYYYMMDD'), 'DD/MM/YYYY')AS PERIODDATE 
                            //FROM RLTB74 INNER JOIN RLTB76 ON RLTB76.T76TYP = RLTB74.T74TYP AND T76ACT = T74ACT AND T76BEF = T74BEF 
                            //WHERE T74CMP = '" + Camp.CampaignCode.Trim() + "'";
                            sql = @"
                                SELECT  T74TYP AS CAMPAIGNTYPE, T76NME AS CAMPAIGNTYPEDESC, T74CMP AS CAMPAIGNCODE,T74DSC AS CAMPAIGNDESC , 
                            T74BEF AS BENEFIT, T74CTY AS CUSTOMERTYPE ,  T74ACT AS ACTIONTYPE,T74RUS AS REGISTERBY, T74UPU AS APPROVEBY, T74GVA AS BENEFITGVAMOUNT, T74WDM AS WITHDRAWAMOUNT,
                         	CONVERT(varchar, CONVERT(datetime, CASE WHEN T74GDT != 0 THEN  CONVERT(varchar,T74GDT) END ), 103) AS GENTARGETDATE,  
                            CONVERT(varchar, CONVERT(datetime, CASE WHEN T74EXD != 0 THEN  CONVERT(varchar,T74EXD) END ), 103) AS EXPIREGVDATE, 
							CONCAT(convert(nvarchar(MAX), CONVERT(varchar, CONVERT(datetime,  CASE WHEN T74WDS != 0 THEN  CONVERT(varchar,T74WDS) END ), 103)),' ',convert(nvarchar(MAX), CONVERT(varchar, CONVERT(datetime, CASE WHEN T74WDE != 0 THEN  CONVERT(varchar,T74WDE)  END ), 103))) AS WITHDRAWDATE,
						 	CONVERT(varchar, CONVERT(datetime, CASE WHEN T74RDT != 0 THEN  CONVERT(varchar,T74RDT) END ), 103) AS REGISTERDATE,
							CONVERT(varchar, CONVERT(datetime, CASE WHEN T74UPD != 0 THEN  CONVERT(varchar,T74UPD) END ), 103) AS UPDATEDATE,
                            CONCAT(convert(nvarchar(MAX), CONVERT(varchar, CONVERT(datetime,   CONVERT(varchar,T74SDT) ), 103)),' ',convert(nvarchar(MAX), CONVERT(varchar, CONVERT(datetime, CASE WHEN T74EDT != 99999999 THEN  CONVERT(varchar,T74EDT) ELSE  CONVERT(varchar,20991231) END ), 103))) AS PERIODDATE
                          
							FROM RLTB74 INNER JOIN RLTB76 ON RLTB76.T76TYP = RLTB74.T74TYP AND T76ACT = T74ACT AND T76BEF = T74BEF 
                                WHERE T74CMP = '" + Camp.CampaignCode.Trim() + "'";

                        }
                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
                        }

                    }else if(Benefit == "RR")
                    {
                        if (ActionType == "WK")
                        {
                        //    sql = @"SELECT  T74TYP AS CAMPAIGNTYPE, T76NME AS CAMPAIGNTYPEDESC, T74CMP AS CAMPAIGNCODE,T74DSC AS CAMPAIGNDESC , 
                        //T74BEF AS BENEFIT, T74CTY AS CUSTOMERTYPE , T74CDT AS CARDTYPE, T74ACT AS ACTIONTYPE,
                        //VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74GDT), 'YYYYMMDD'), 'DD/MM/YYYY') AS GENTARGETDATE, 
                        //T74WDM AS WITHDRAWAMOUNT,  
                        //T74KOB AS KEEPOB, 
                        //VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74KDT), 'YYYYMMDD'), 'DD/MM/YYYY') AS UNITDATE, 
                        //VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74WDS), 'YYYYMMDD'), 'DD/MM/YYYY') || ' ' || VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74WDE), 'YYYYMMDD'), 'DD/MM/YYYY') AS WITHDRAWDATE, 
                        //VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74RDT), 'YYYYMMDD'), 'DD/MM/YYYY') AS REGISTERDATE, 
                        //T74RUS AS REGISTERBY, T74UPU AS APPROVEBY,
                        //VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74UPD), 'YYYYMMDD'), 'DD/MM/YYYY') AS UPDATEDATE,
                        //VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74SDT), 'YYYYMMDD'), 'DD/MM/YYYY') || '   ' || VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74EDT), 'YYYYMMDD'), 'DD/MM/YYYY')AS PERIODDATE 
                        //FROM RLTB74 INNER JOIN RLTB76 ON RLTB76.T76TYP = RLTB74.T74TYP AND T76ACT = T74ACT AND T76BEF = T74BEF
                        //WHERE T74CMP = '" + Camp.CampaignCode.Trim() + "'";
                            sql = @"
                                SELECT  T74TYP AS CAMPAIGNTYPE, T76NME AS CAMPAIGNTYPEDESC, T74CMP AS CAMPAIGNCODE,T74DSC AS CAMPAIGNDESC , 
							T74BEF AS BENEFIT, T74CTY AS CUSTOMERTYPE , T74CDT AS CARDTYPE, T74ACT AS ACTIONTYPE,T74WDM AS WITHDRAWAMOUNT,T74KOB AS KEEPOB,T74RUS AS REGISTERBY, T74UPU AS APPROVEBY, 
							CONVERT(varchar, CONVERT(datetime, CASE WHEN T74GDT != 0 THEN  CONVERT(varchar,T74GDT) END ), 103) AS GENTARGETDATE,
							CONVERT(varchar, CONVERT(datetime, CASE WHEN T74KDT != 0 THEN  CONVERT(varchar,T74KDT) END ), 103) AS UNITDATE,
                      		CONCAT(convert(nvarchar(MAX), CONVERT(varchar, CONVERT(datetime,  CASE WHEN T74WDS != 0 THEN  CONVERT(varchar,T74WDS) END ), 103)),' ',convert(nvarchar(MAX), CONVERT(varchar, CONVERT(datetime, CASE WHEN T74WDE != 0 THEN  CONVERT(varchar,T74WDE)  END ), 103))) AS WITHDRAWDATE,
						 	CONVERT(varchar, CONVERT(datetime, CASE WHEN T74RDT != 0 THEN  CONVERT(varchar,T74RDT) END ), 103) AS REGISTERDATE,
							CONVERT(varchar, CONVERT(datetime, CASE WHEN T74UPD != 0 THEN  CONVERT(varchar,T74UPD) END ), 103) AS UPDATEDATE,
                            CONCAT(convert(nvarchar(MAX), CONVERT(varchar, CONVERT(datetime,   CONVERT(varchar,T74SDT) ), 103)),' ',convert(nvarchar(MAX), CONVERT(varchar, CONVERT(datetime, CASE WHEN T74EDT != 99999999 THEN  CONVERT(varchar,T74EDT) ELSE  CONVERT(varchar,20991231) END ), 103))) AS PERIODDATE
                          FROM RLTB74 INNER JOIN RLTB76 ON RLTB76.T76TYP = RLTB74.T74TYP AND T76ACT = T74ACT AND T76BEF = T74BEF
                            WHERE T74CMP = '" + Camp.CampaignCode.Trim() + "'";


                        }
                        else if (ActionType == "WT") {

                            //    sql = @"SELECT  T74TYP AS CAMPAIGNTYPE, T76NME AS CAMPAIGNTYPEDESC, T74CMP AS CAMPAIGNCODE,T74DSC AS CAMPAIGNDESC , 
                            //T74BEF AS BENEFIT, T74CTY AS CUSTOMERTYPE , T74CDT AS CARDTYPE, T74ACT AS ACTIONTYPE,
                            //VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74GDT), 'YYYYMMDD'), 'DD/MM/YYYY') AS GENTARGETDATE, 
                            //T74WDM AS WITHDRAWAMOUNT, 
                            //VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74WDS), 'YYYYMMDD'), 'DD/MM/YYYY') || ' ' || VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74WDE), 'YYYYMMDD'), 'DD/MM/YYYY') AS WITHDRAWDATE, 
                            //VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74RDT), 'YYYYMMDD'), 'DD/MM/YYYY') AS REGISTERDATE, 
                            //T74RUS AS REGISTERBY, T74UPU AS APPROVEBY,
                            //VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74UPD), 'YYYYMMDD'), 'DD/MM/YYYY') AS UPDATEDATE,
                            //VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74SDT), 'YYYYMMDD'), 'DD/MM/YYYY') || '   ' || VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74EDT), 'YYYYMMDD'), 'DD/MM/YYYY')AS PERIODDATE 
                            //FROM RLTB74 INNER JOIN RLTB76 ON RLTB76.T76TYP = RLTB74.T74TYP AND T76ACT = T74ACT AND T76BEF = T74BEF
                            //WHERE T74CMP = '" + Camp.CampaignCode.Trim() + "'";

                            sql = @"
	                        SELECT  T74TYP AS CAMPAIGNTYPE, T76NME AS CAMPAIGNTYPEDESC, T74CMP AS CAMPAIGNCODE,T74DSC AS CAMPAIGNDESC , 
                            T74BEF AS BENEFIT, T74CTY AS CUSTOMERTYPE , T74CDT AS CARDTYPE, T74ACT AS ACTIONTYPE,T74WDM AS WITHDRAWAMOUNT,T74RUS AS REGISTERBY, T74UPU AS APPROVEBY,
                           	CONVERT(varchar, CONVERT(datetime, CASE WHEN T74GDT != 0 THEN  CONVERT(varchar,T74GDT) END ), 103) AS GENTARGETDATE,
		
                      		CONCAT(convert(nvarchar(MAX), CONVERT(varchar, CONVERT(datetime,  CASE WHEN T74WDS != 0 THEN  CONVERT(varchar,T74WDS) END ), 103)),' ',convert(nvarchar(MAX), CONVERT(varchar, CONVERT(datetime, CASE WHEN T74WDE != 0 THEN  CONVERT(varchar,T74WDE)  END ), 103))) AS WITHDRAWDATE,
						 	CONVERT(varchar, CONVERT(datetime, CASE WHEN T74RDT != 0 THEN  CONVERT(varchar,T74RDT) END ), 103) AS REGISTERDATE,
							CONVERT(varchar, CONVERT(datetime, CASE WHEN T74UPD != 0 THEN  CONVERT(varchar,T74UPD) END ), 103) AS UPDATEDATE,
                            CONCAT(convert(nvarchar(MAX), CONVERT(varchar, CONVERT(datetime,   CONVERT(varchar,T74SDT) ), 103)),' ',convert(nvarchar(MAX), CONVERT(varchar, CONVERT(datetime, CASE WHEN T74EDT != 99999999 THEN  CONVERT(varchar,T74EDT) ELSE  CONVERT(varchar,20991231) END ), 103))) AS PERIODDATE
                         
							FROM RLTB74 INNER JOIN RLTB76 ON RLTB76.T76TYP = RLTB74.T74TYP AND T76ACT = T74ACT AND T76BEF = T74BEF
                                    
                                   WHERE T74CMP = '" + Camp.CampaignCode.Trim() + "'";

                        }
                        else {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
                        }

                    }else if(Benefit == "SW" || Benefit == "UP")
                    {
                        //sql = @"SELECT  T74TYP AS CAMPAIGNTYPE, T76NME AS CAMPAIGNTYPEDESC, T74CMP AS CAMPAIGNCODE,T74DSC AS CAMPAIGNDESC , 
                        //T74BEF AS BENEFIT, T74CTY AS CUSTOMERTYPE , T74CDT AS CARDTYPE, T74ACT AS ACTIONTYPE,
                        //VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74GDT), 'YYYYMMDD'), 'DD/MM/YYYY') AS GENTARGETDATE, 
                        //VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74RDT), 'YYYYMMDD'), 'DD/MM/YYYY') AS REGISTERDATE, 
                        //T74RUS AS REGISTERBY, T74UPU AS APPROVEBY,
                        //VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74UPD), 'YYYYMMDD'), 'DD/MM/YYYY') AS UPDATEDATE,
                        //VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74SDT), 'YYYYMMDD'), 'DD/MM/YYYY') || '   ' || VARCHAR_FORMAT(TIMESTAMP_FORMAT(CHAR(T74EDT), 'YYYYMMDD'), 'DD/MM/YYYY')AS PERIODDATE 
                        //FROM RLTB74 INNER JOIN RLTB76 ON RLTB76.T76TYP = RLTB74.T74TYP AND T76ACT = T74ACT AND T76BEF = T74BEF
                        //WHERE T74CMP = '" + Camp.CampaignCode.Trim() + "'";
                        sql = @"
                    			SELECT  T74TYP AS CAMPAIGNTYPE, T76NME AS CAMPAIGNTYPEDESC, T74CMP AS CAMPAIGNCODE,T74DSC AS CAMPAIGNDESC , 
                            T74BEF AS BENEFIT, T74CTY AS CUSTOMERTYPE , T74CDT AS CARDTYPE, T74ACT AS ACTIONTYPE,T74RUS AS REGISTERBY, T74UPU AS APPROVEBY,
                        	CONVERT(varchar, CONVERT(datetime, CASE WHEN T74GDT != 0 THEN  CONVERT(varchar,T74GDT) END ), 103) AS GENTARGETDATE,
		
                      		CONCAT(convert(nvarchar(MAX), CONVERT(varchar, CONVERT(datetime,  CASE WHEN T74WDS != 0 THEN  CONVERT(varchar,T74WDS) END ), 103)),' ',convert(nvarchar(MAX), CONVERT(varchar, CONVERT(datetime, CASE WHEN T74WDE != 0 THEN  CONVERT(varchar,T74WDE)  END ), 103))) AS WITHDRAWDATE,
						 	CONVERT(varchar, CONVERT(datetime, CASE WHEN T74RDT != 0 THEN  CONVERT(varchar,T74RDT) END ), 103) AS REGISTERDATE,
							CONVERT(varchar, CONVERT(datetime, CASE WHEN T74UPD != 0 THEN  CONVERT(varchar,T74UPD) END ), 103) AS UPDATEDATE,
                            CONCAT(convert(nvarchar(MAX), CONVERT(varchar, CONVERT(datetime,   CONVERT(varchar,T74SDT) ), 103)),' ',convert(nvarchar(MAX), CONVERT(varchar, CONVERT(datetime, CASE WHEN T74EDT != 99999999 THEN  CONVERT(varchar,T74EDT) ELSE  CONVERT(varchar,20991231) END ), 103))) AS PERIODDATE
                         
                            FROM RLTB74 INNER JOIN RLTB76 ON RLTB76.T76TYP = RLTB74.T74TYP AND T76ACT = T74ACT AND T76BEF = T74BEF

                                WHERE T74CMP = '" + Camp.CampaignCode.Trim() + "'";
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
                    }
                    
                }
                else
                {

                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
                }

                // DataTable dt = db2.GetDataTableAS400(sql);
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
    }
}
