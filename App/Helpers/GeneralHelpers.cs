using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Helpers
{
    public static class GeneralHelpers
    {
        public static string GenerateZeroString(int length)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append("0");
            }
            return sb.ToString();
        }

        private static string Right(this string value, int length)
        {
            if (String.IsNullOrEmpty(value)) return string.Empty;

            return value.Length <= length ? value : value.Substring(value.Length - length);
        }

        public static string GenerateRandomDigitCode(int length)
        {
            var random = new Random();
            string str = string.Empty;
            for (int i = 0; i < length; i++)
                str = String.Concat(str, random.Next(10).ToString());
            return str;
        }

        public static string MailBody(string name, string url, string content1, string content2)
        {
            StringBuilder sbEmailBody = new StringBuilder();
            sbEmailBody.Append("<td style =\"font-family: sans-serif; font-size: 14px; vertical-align: top;>\"");
            sbEmailBody.Append("<p style =\"font -family: sans-serif; font-size: 14px; font-weight: normal; margin: 0; Margin-bottom: 15px;\">");
            sbEmailBody.Append($"Hi {name},");
            sbEmailBody.Append($"<br/><br/><br/></p><p style =\"font-family: sans-serif; font-size: 14px; font-weight: normal; margin: 0; Margin-bottom: 15px;\">{ content1 }</p>");
            sbEmailBody.Append("<table border =\"0\" cellpadding =\"0\" cellspacing =\"0\" class=\"btn btn-primary\" style=\"border-collapse: separate; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100 %; box-sizing: border-box;\"><tbody><tr>");
            sbEmailBody.Append("<td align =\"left\" style=\"font-family: sans-serif; font-size: 14px; vertical-align: top; padding-bottom: 15px;\">");
            sbEmailBody.Append("<table border = \"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: separate; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: auto;\">");
            sbEmailBody.Append("<tbody><tr>");
            sbEmailBody.Append($"<td style=\"font-family: sans-serif; font-size: 14px; vertical-align: top; background-color: #3498db; border-radius: 5px; text-align: center;\"><a href=\"{ url }\" target=\"_blank\" style=\"display: inline-block; color: #ffffff; background-color: #3498db; border: solid 1px #3498db; border-radius: 5px; box-sizing: border-box; cursor: pointer; text-decoration: none; font-size: 14px; font-weight: bold; margin: 0; padding: 12px 25px; text-transform: capitalize; border-color: #3498db;\">Click Here</a></td></tr>");

            sbEmailBody.Append("</tbody></table></td></tr></tbody>");
            sbEmailBody.Append("</table>");
            sbEmailBody.Append($"<p style=\"font-family: sans-serif; font-size: 14px; font-weight: normal; margin: 0; Margin-bottom: 15px;\"> { content2 }</p>");
            sbEmailBody.Append("<br/><br/><br/><p style=\"font-family: sans-serif; font-size: 14px; font-weight: normal; margin: 0; Margin-bottom: 15px;\"> Kind Regards!</p>");
            sbEmailBody.Append("</td>");

            return sbEmailBody.ToString();
        }
    }
}
