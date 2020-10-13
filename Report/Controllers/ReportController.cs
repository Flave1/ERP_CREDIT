using AlanJuden.MvcReportViewer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Report.Controllers
{
	public class ReportController : AlanJuden.MvcReportViewer.ReportController
	{
		protected override ICredentials NetworkCredentials
		{
			get
			{
				//Custom Domain authentication (be sure to pull the info from a config file)
				//return new System.Net.NetworkCredential("username", "password", "domain");

				//Default domain credentials (windows authentication)
				return CredentialCache.DefaultNetworkCredentials;
			}
		}

		//protected override ICredentials NetworkCredentials => throw new NotImplementedException();

		protected override string ReportServerUrl
		{
			get
			{
				//You don't want to put the full API path here, just the path to the report server's ReportServer directory that it creates (you should be able to access this path from your browser: https://YourReportServerUrl.com/ReportServer/ReportExecution2005.asmx )
				return "https://YourReportServerUrl.com/ReportServer";
			}
		}

		//Only override this property if your controller is not called ReportController.
		//You'll want to enter whatever your controller's name is in place of "YourController" below.
		protected override string ReportImagePath
		{
			get
			{
				//This is the default as an example
				//return "/Report/ReportImage/?originalPath={0}";

				return "/YourController/ReportImage/?originalPath={0}";
			}
		}

		public ActionResult MyReport(string namedParameter1, string namedParameter2)
		{
			var model = this.GetReportViewerModel(Request);
			model.ReportPath = "/Folder/Report File Name";
			model.AddParameter("Parameter1", namedParameter1);
			model.AddParameter("Parameter2", namedParameter2);

			return View("ReportViewer", model);
		}

		protected ReportViewerModel GetReportViewerModel(HttpRequest request)
		{
			var model = new ReportViewerModel();
			model.AjaxLoadInitialReport = this.AjaxLoadInitialReport;
			model.Credentials = this.NetworkCredentials;

			var enablePagingResult = _getRequestValue(request, "ReportViewerEnablePaging");
			if (enablePagingResult.HasValue())
			{
				model.EnablePaging = enablePagingResult.ToBoolean();
			}
			else
			{
				model.EnablePaging = true;
			}

			model.Encoding = this.Encoding;
			model.ServerUrl = this.ReportServerUrl;
			model.ReportImagePath = this.ReportImagePath;
			model.Timeout = this.Timeout;
			model.UseCustomReportImagePath = this.UseCustomReportImagePath;
			model.BuildParameters(Request);

			return model;
		}

		private string _getRequestValue(HttpRequest request, string key)
		{
			if (request.Query.ContainsKey(key)) return key;

			try
			{
				if (request.Form != null && request.Form.Keys != null && request.Form.ContainsKey(key))
				{
					return request.Form[key].ToSafeString();
				}
			}
			catch
			{
				//No need to throw errors, just no Form was passed in and it's unhappy about that
			}

			return String.Empty;
		}
	}
}
