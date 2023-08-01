using API_PIX.Application.Interfaces;
using API_PIX.Data.Interfaces;
using API_PIX.Domain.ClientModel;
using API_PIX.Domain.Error;
using API_PIX.Domain.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_PIX.Application.Services
{
    public class ClientService : IClientService
    {
        IClientRepository ClientRepository { get; set; }
        ILogsService LogsService { get; set; }
        //private IEmailService EmailService { get; set; }

        public ClientService(IClientRepository ClientRepository,  
            ILogsService logsService )
        {
            ClientRepository = ClientRepository;
            LogsService = logsService;
        }

        private string ValidationMessage { get; set; }

        public string GetValidationMessage()
        {
            return ValidationMessage;
        }

   
        public string ValidateClient(Client u)
        {
            try
            {
                var issue = "";
                if (ClientRepository.GetCPF(u.CPF) != null || 
                    (ClientRepository.GetEmail(u.Email) != null))
                    issue += "Email Address or CPF Already Registered ";
                if (string.IsNullOrEmpty(u.Email))
                    issue += "Email Address is empty ";

                if (string.IsNullOrEmpty(u.PassHash))
                    issue += "Password is Empty ";

                if (string.IsNullOrEmpty(u.NameCompleto))
                    issue += "Clientname is Empty ";
                if (!string.IsNullOrEmpty(issue))
                    issue = "Error: " + issue;

                ValidationMessage = issue;
                return issue;
            }
            catch (HandledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw LogsService.HandleException(ex, "Client error", "There was an error validating the Client", this.GetType().ToString());
            }
        }

        public void UpdateLastLogin(string CPF, DateTime updatedDate)
        {
            var Client = ClientRepository.GetCPF(CPF);
            Client.DtLastLogin = updatedDate;
            ClientRepository.Update(Client);
        }
        public Client RegisterClient(Client u)
        {
            try
            {

                var Clientvalidation = ValidateClient(u);

                if (string.IsNullOrEmpty(Clientvalidation))
                {
                    
                    u.DtLastLogin = DateTime.Now;
                    u.DtRegistered = DateTime.Now;
                    u.PassHash = HashingService.GetHash(u.PassHash);

                    var result = ClientRepository.Add(u);

                    /* ENVIAR EMAIL
                    var emailText = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional //EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\">\r\n\r\n<head>\r\n\t<!--[if gte mso 9]><xml><o:OfficeDocumentSettings><o:AllowPNG/><o:PixelsPerInch>96</o:PixelsPerInch></o:OfficeDocumentSettings></xml><![endif]-->\r\n\t<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\r\n\t<meta name=\"viewport\" content=\"width=device-width\">\r\n\t<!--[if !mso]><!-->\r\n\t<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n\t<!--<![endif]-->\r\n\t<title></title>\r\n\t<!--[if !mso]><!-->\r\n\t<!--<![endif]-->\r\n\t<style type=\"text/css\">\r\n\t\tbody {\r\n\t\t\tmargin: 0;\r\n\t\t\tpadding: 0;\r\n\t\t}\r\n\r\n\t\ttable,\r\n\t\ttd,\r\n\t\ttr {\r\n\t\t\tvertical-align: top;\r\n\t\t\tborder-collapse: collapse;\r\n\t\t}\r\n\r\n\t\t* {\r\n\t\t\tline-height: inherit;\r\n\t\t}\r\n\r\n\t\ta[x-apple-data-detectors=true] {\r\n\t\t\tcolor: inherit !important;\r\n\t\t\ttext-decoration: none !important;\r\n\t\t}\r\n\t</style>\r\n\t<style type=\"text/css\" id=\"media-query\">\r\n\t\t@media (max-width: 540px) {\r\n\r\n\t\t\t.block-grid,\r\n\t\t\t.col {\r\n\t\t\t\tmin-width: 320px !important;\r\n\t\t\t\tmax-width: 100% !important;\r\n\t\t\t\tdisplay: block !important;\r\n\t\t\t}\r\n\r\n\t\t\t.block-grid {\r\n\t\t\t\twidth: 100% !important;\r\n\t\t\t}\r\n\r\n\t\t\t.col {\r\n\t\t\t\twidth: 100% !important;\r\n\t\t\t}\r\n\r\n\t\t\t.col_cont {\r\n\t\t\t\tmargin: 0 auto;\r\n\t\t\t}\r\n\r\n\t\t\timg.fullwidth,\r\n\t\t\timg.fullwidthOnMobile {\r\n\t\t\t\twidth: 100% !important;\r\n\t\t\t}\r\n\r\n\t\t\t.no-stack .col {\r\n\t\t\t\tmin-width: 0 !important;\r\n\t\t\t\tdisplay: table-cell !important;\r\n\t\t\t}\r\n\r\n\t\t\t.no-stack.two-up .col {\r\n\t\t\t\twidth: 50% !important;\r\n\t\t\t}\r\n\r\n\t\t\t.no-stack .col.num2 {\r\n\t\t\t\twidth: 16.6% !important;\r\n\t\t\t}\r\n\r\n\t\t\t.no-stack .col.num3 {\r\n\t\t\t\twidth: 25% !important;\r\n\t\t\t}\r\n\r\n\t\t\t.no-stack .col.num4 {\r\n\t\t\t\twidth: 33% !important;\r\n\t\t\t}\r\n\r\n\t\t\t.no-stack .col.num5 {\r\n\t\t\t\twidth: 41.6% !important;\r\n\t\t\t}\r\n\r\n\t\t\t.no-stack .col.num6 {\r\n\t\t\t\twidth: 50% !important;\r\n\t\t\t}\r\n\r\n\t\t\t.no-stack .col.num7 {\r\n\t\t\t\twidth: 58.3% !important;\r\n\t\t\t}\r\n\r\n\t\t\t.no-stack .col.num8 {\r\n\t\t\t\twidth: 66.6% !important;\r\n\t\t\t}\r\n\r\n\t\t\t.no-stack .col.num9 {\r\n\t\t\t\twidth: 75% !important;\r\n\t\t\t}\r\n\r\n\t\t\t.no-stack .col.num10 {\r\n\t\t\t\twidth: 83.3% !important;\r\n\t\t\t}\r\n\r\n\t\t\t.video-block {\r\n\t\t\t\tmax-width: none !important;\r\n\t\t\t}\r\n\r\n\t\t\t.mobile_hide {\r\n\t\t\t\tmin-height: 0px;\r\n\t\t\t\tmax-height: 0px;\r\n\t\t\t\tmax-width: 0px;\r\n\t\t\t\tdisplay: none;\r\n\t\t\t\toverflow: hidden;\r\n\t\t\t\tfont-size: 0px;\r\n\t\t\t}\r\n\r\n\t\t\t.desktop_hide {\r\n\t\t\t\tdisplay: block !important;\r\n\t\t\t\tmax-height: none !important;\r\n\t\t\t}\r\n\t\t}\r\n\t</style>\r\n</head>\r\n\r\n<body class=\"clean-body\" style=\"margin: 0; padding: 0; -webkit-text-size-adjust: 100%; background-color: #35a594;\">\r\n\t<!--[if IE]><div class=\"ie-browser\"><![endif]-->\r\n\t<table class=\"nl-container\" style=\"table-layout: fixed; vertical-align: top; min-width: 320px; border-spacing: 0; border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #35a594; width: 100%;\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" width=\"100%\" bgcolor=\"#35a594\" valign=\"top\">\r\n\t\t<tbody>\r\n\t\t\t<tr style=\"vertical-align: top;\" valign=\"top\">\r\n\t\t\t\t<td style=\"word-break: break-word; vertical-align: top;\" valign=\"top\">\r\n\t\t\t\t\t<!--[if (mso)|(IE)]><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td align=\"center\" style=\"background-color:#35a594\"><![endif]-->\r\n\t\t\t\t\t<div style=\"background-color:#ffffff;\">\r\n\t\t\t\t\t\t<div class=\"block-grid \" style=\"min-width: 320px; max-width: 520px; overflow-wrap: break-word; word-wrap: break-word; word-break: break-word; Margin: 0 auto; background-color: #ffffff;\">\r\n\t\t\t\t\t\t\t<div style=\"border-collapse: collapse;display: table;width: 100%;background-color:#ffffff;\">\r\n\t\t\t\t\t\t\t\t<!--[if (mso)|(IE)]><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"background-color:#ffffff;\"><tr><td align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:520px\"><tr class=\"layout-full-width\" style=\"background-color:#ffffff\"><![endif]-->\r\n\t\t\t\t\t\t\t\t<!--[if (mso)|(IE)]><td align=\"center\" width=\"520\" style=\"background-color:#ffffff;width:520px; border-top: 0px solid transparent; border-left: 0px solid transparent; border-bottom: 0px solid transparent; border-right: 0px solid transparent;\" valign=\"top\"><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td style=\"padding-right: 0px; padding-left: 0px; padding-top:5px; padding-bottom:5px;\"><![endif]-->\r\n\t\t\t\t\t\t\t\t<div class=\"col num12\" style=\"min-width: 320px; max-width: 520px; display: table-cell; vertical-align: top; width: 520px;\">\r\n\t\t\t\t\t\t\t\t\t<div class=\"col_cont\" style=\"width:100% !important;\">\r\n\t\t\t\t\t\t\t\t\t\t<!--[if (!mso)&(!IE)]><!-->\r\n\t\t\t\t\t\t\t\t\t\t<div style=\"border-top:0px solid transparent; border-left:0px solid transparent; border-bottom:0px solid transparent; border-right:0px solid transparent; padding-top:5px; padding-bottom:5px; padding-right: 0px; padding-left: 0px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t<!--<![endif]-->\r\n\t\t\t\t\t\t\t\t\t\t\t<div class=\"img-container center autowidth\" align=\"center\" style=\"padding-right: 0px;padding-left: 0px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t<!--[if mso]><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr style=\"line-height:0px\"><td style=\"padding-right: 0px;padding-left: 0px;\" align=\"center\"><![endif]--><img class=\"center autowidth\" align=\"center\" border=\"0\" src=\"https://d15k2d11r6t6rl.cloudfront.net/public/Clients/Integrators/BeeProAgency/688498_671054/Zuvviiheader.png\" style=\"text-decoration: none; -ms-interpolation-mode: bicubic; height: auto; border: 0; width: 520px; max-width: 100%; display: block;\" width=\"520\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t<!--[if mso]></td></tr></table><![endif]-->\r\n\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t<!--[if (!mso)&(!IE)]><!-->\r\n\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t<!--<![endif]-->\r\n\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t<!--[if (mso)|(IE)]></td></tr></table><![endif]-->\r\n\t\t\t\t\t\t\t\t<!--[if (mso)|(IE)]></td></tr></table></td></tr></table><![endif]-->\r\n\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t</div>\r\n\t\t\t\t\t</div>\r\n\t\t\t\t\t<div style=\"background-color:#ffffff;\">\r\n\t\t\t\t\t\t<div class=\"block-grid \" style=\"min-width: 320px; max-width: 520px; overflow-wrap: break-word; word-wrap: break-word; word-break: break-word; Margin: 0 auto; background-color: #ffffff;\">\r\n\t\t\t\t\t\t\t<div style=\"border-collapse: collapse;display: table;width: 100%;background-color:#ffffff;\">\r\n\t\t\t\t\t\t\t\t<!--[if (mso)|(IE)]><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"background-color:#ffffff;\"><tr><td align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:520px\"><tr class=\"layout-full-width\" style=\"background-color:#ffffff\"><![endif]-->\r\n\t\t\t\t\t\t\t\t<!--[if (mso)|(IE)]><td align=\"center\" width=\"520\" style=\"background-color:#ffffff;width:520px; border-top: 0px solid transparent; border-left: 0px solid transparent; border-bottom: 0px solid transparent; border-right: 0px solid transparent;\" valign=\"top\"><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td style=\"padding-right: 0px; padding-left: 0px; padding-top:5px; padding-bottom:5px;\"><![endif]-->\r\n\t\t\t\t\t\t\t\t<div class=\"col num12\" style=\"min-width: 320px; max-width: 520px; display: table-cell; vertical-align: top; width: 520px;\">\r\n\t\t\t\t\t\t\t\t\t<div class=\"col_cont\" style=\"width:100% !important;\">\r\n\t\t\t\t\t\t\t\t\t\t<!--[if (!mso)&(!IE)]><!-->\r\n\t\t\t\t\t\t\t\t\t\t<div style=\"border-top:0px solid transparent; border-left:0px solid transparent; border-bottom:0px solid transparent; border-right:0px solid transparent; padding-top:5px; padding-bottom:5px; padding-right: 0px; padding-left: 0px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t<!--<![endif]-->\r\n\t\t\t\t\t\t\t\t\t\t\t<!--[if mso]><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td style=\"padding-right: 10px; padding-left: 10px; padding-top: 10px; padding-bottom: 10px; font-family: Arial, sans-serif\"><![endif]-->\r\n\t\t\t\t\t\t\t\t\t\t\t<div style=\"color:#393d47;font-family:Arial, Helvetica Neue, Helvetica, sans-serif;line-height:1.2;padding-top:10px;padding-right:10px;padding-bottom:10px;padding-left:10px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t<div class=\"txtTinyMce-wrapper\" style=\"font-size: 14px; line-height: 1.2; color: #393d47; font-family: Arial, Helvetica Neue, Helvetica, sans-serif; mso-line-height-alt: 17px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; font-size: 38px; line-height: 1.2; word-break: break-word; text-align: center; mso-line-height-alt: 46px; margin-top: 0; margin-bottom: 0;\"><span style=\"font-size: 38px; color: #35a594;\">Welcome to Zuvvii</span></p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t<!--[if mso]></td></tr></table><![endif]-->\r\n\t\t\t\t\t\t\t\t\t\t\t<table class=\"divider\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"table-layout: fixed; vertical-align: top; border-spacing: 0; border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; min-width: 100%; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;\" role=\"presentation\" valign=\"top\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<tr style=\"vertical-align: top;\" valign=\"top\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"divider_inner\" style=\"word-break: break-word; vertical-align: top; min-width: 100%; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; padding-top: 10px; padding-right: 10px; padding-bottom: 10px; padding-left: 10px;\" valign=\"top\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<table class=\"divider_content\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"table-layout: fixed; vertical-align: top; border-spacing: 0; border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; border-top: 1px solid #BBBBBB; width: 100%;\" align=\"center\" role=\"presentation\" valign=\"top\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr style=\"vertical-align: top;\" valign=\"top\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td style=\"word-break: break-word; vertical-align: top; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;\" valign=\"top\"><span></span></td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t<!--[if (!mso)&(!IE)]><!-->\r\n\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t<!--<![endif]-->\r\n\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t<!--[if (mso)|(IE)]></td></tr></table><![endif]-->\r\n\t\t\t\t\t\t\t\t<!--[if (mso)|(IE)]></td></tr></table></td></tr></table><![endif]-->\r\n\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t</div>\r\n\t\t\t\t\t</div>\r\n\t\t\t\t\t<div style=\"background-color:#ffffff;\">\r\n\t\t\t\t\t\t<div class=\"block-grid two-up\" style=\"min-width: 320px; max-width: 520px; overflow-wrap: break-word; word-wrap: break-word; word-break: break-word; Margin: 0 auto; background-color: #ffffff;\">\r\n\t\t\t\t\t\t\t<div style=\"border-collapse: collapse;display: table;width: 100%;background-color:#ffffff;\">\r\n\t\t\t\t\t\t\t\t<!--[if (mso)|(IE)]><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"background-color:#ffffff;\"><tr><td align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:520px\"><tr class=\"layout-full-width\" style=\"background-color:#ffffff\"><![endif]-->\r\n\t\t\t\t\t\t\t\t<!--[if (mso)|(IE)]><td align=\"center\" width=\"260\" style=\"background-color:#ffffff;width:260px; border-top: 0px solid transparent; border-left: 0px solid transparent; border-bottom: 0px solid transparent; border-right: 0px solid transparent;\" valign=\"top\"><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td style=\"padding-right: 0px; padding-left: 0px; padding-top:5px; padding-bottom:5px;\"><![endif]-->\r\n\t\t\t\t\t\t\t\t<div class=\"col num6\" style=\"display: table-cell; vertical-align: top; max-width: 320px; min-width: 258px; width: 260px;\">\r\n\t\t\t\t\t\t\t\t\t<div class=\"col_cont\" style=\"width:100% !important;\">\r\n\t\t\t\t\t\t\t\t\t\t<!--[if (!mso)&(!IE)]><!-->\r\n\t\t\t\t\t\t\t\t\t\t<div style=\"border-top:0px solid transparent; border-left:0px solid transparent; border-bottom:0px solid transparent; border-right:0px solid transparent; padding-top:5px; padding-bottom:5px; padding-right: 0px; padding-left: 0px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t<!--<![endif]-->\r\n\t\t\t\t\t\t\t\t\t\t\t<div class=\"img-container center autowidth\" align=\"center\" style=\"padding-right: 0px;padding-left: 0px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t<!--[if mso]><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr style=\"line-height:0px\"><td style=\"padding-right: 0px;padding-left: 0px;\" align=\"center\"><![endif]--><img class=\"center autowidth\" align=\"center\" border=\"0\" src=\"https://d15k2d11r6t6rl.cloudfront.net/public/Clients/Integrators/BeeProAgency/688498_671054/IMG_5254_iphone12black_portrait.png\" style=\"text-decoration: none; -ms-interpolation-mode: bicubic; height: auto; border: 0; width: 260px; max-width: 100%; display: block;\" width=\"260\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t<!--[if mso]></td></tr></table><![endif]-->\r\n\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t<!--[if (!mso)&(!IE)]><!-->\r\n\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t<!--<![endif]-->\r\n\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t<!--[if (mso)|(IE)]></td></tr></table><![endif]-->\r\n\t\t\t\t\t\t\t\t<!--[if (mso)|(IE)]></td><td align=\"center\" width=\"260\" style=\"background-color:#ffffff;width:260px; border-top: 0px solid transparent; border-left: 0px solid transparent; border-bottom: 0px solid transparent; border-right: 0px solid transparent;\" valign=\"top\"><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td style=\"padding-right: 0px; padding-left: 0px; padding-top:5px; padding-bottom:5px;\"><![endif]-->\r\n\t\t\t\t\t\t\t\t<div class=\"col num6\" style=\"display: table-cell; vertical-align: top; max-width: 320px; min-width: 258px; width: 260px;\">\r\n\t\t\t\t\t\t\t\t\t<div class=\"col_cont\" style=\"width:100% !important;\">\r\n\t\t\t\t\t\t\t\t\t\t<!--[if (!mso)&(!IE)]><!-->\r\n\t\t\t\t\t\t\t\t\t\t<div style=\"border-top:0px solid transparent; border-left:0px solid transparent; border-bottom:0px solid transparent; border-right:0px solid transparent; padding-top:5px; padding-bottom:5px; padding-right: 0px; padding-left: 0px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t<!--<![endif]-->\r\n\t\t\t\t\t\t\t\t\t\t\t<!--[if mso]><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td style=\"padding-right: 10px; padding-left: 10px; padding-top: 10px; padding-bottom: 10px; font-family: Arial, sans-serif\"><![endif]-->\r\n\t\t\t\t\t\t\t\t\t\t\t<div style=\"color:#393d47;font-family:Arial, Helvetica Neue, Helvetica, sans-serif;line-height:1.2;padding-top:10px;padding-right:10px;padding-bottom:10px;padding-left:10px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t<div class=\"txtTinyMce-wrapper\" style=\"font-size: 14px; line-height: 1.2; color: #393d47; font-family: Arial, Helvetica Neue, Helvetica, sans-serif; mso-line-height-alt: 17px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; font-size: 15px; line-height: 1.2; word-break: break-word; mso-line-height-alt: 18px; margin-top: 0; margin-bottom: 0;\"><span style=\"font-size: 15px;\">Hey, Brandon here, Co-Founder of Zuvvii; thanks for signing up.</span></p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; font-size: 15px; line-height: 1.2; word-break: break-word; mso-line-height-alt: 18px; margin-top: 0; margin-bottom: 0;\">&nbsp;</p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; font-size: 15px; line-height: 1.2; word-break: break-word; mso-line-height-alt: 18px; margin-top: 0; margin-bottom: 0;\"><span style=\"font-size: 15px;\">Please keep in mind that this is currently a live beta product, and there may be a few tweaks and changes that need to be made.&nbsp;</span></p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; font-size: 15px; line-height: 1.2; word-break: break-word; mso-line-height-alt: 18px; margin-top: 0; margin-bottom: 0;\">&nbsp;</p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; font-size: 15px; line-height: 1.2; word-break: break-word; mso-line-height-alt: 18px; margin-top: 0; margin-bottom: 0;\"><span style=\"font-size: 15px;\">If you want to provide feedback and help us shape the platform, <a href=\"https://docs.google.com/forms/d/e/1FAIpQLSftabmwYEJTJOjXvEfxo_SddkV9b8bPpXOHRzXUsuxxpY1azA/viewform\" style=\"color: #35a594;\"><strong>please do so here.</strong></a></span></p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; font-size: 15px; line-height: 1.2; word-break: break-word; mso-line-height-alt: 18px; margin-top: 0; margin-bottom: 0;\">&nbsp;</p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; font-size: 15px; line-height: 1.2; word-break: break-word; mso-line-height-alt: 18px; margin-top: 0; margin-bottom: 0;\"><span style=\"font-size: 15px;\">Your feedback is super valuable and will help us shape a platform that gamers truly enjoy using.</span></p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; font-size: 15px; line-height: 1.2; word-break: break-word; mso-line-height-alt: 18px; margin-top: 0; margin-bottom: 0;\">&nbsp;</p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; font-size: 15px; line-height: 1.2; word-break: break-word; mso-line-height-alt: 18px; margin-top: 0; margin-bottom: 0;\"><span style=\"font-size: 15px;\">We are active on all social platforms and will be sharing all of our exciting updates and the latest news there, as well as in our Discord server, so be sure to <a href=\"https://www.zuvvii.com/social-media\" style=\"color: #35a594;\"><strong>check us out.</strong></a></span></p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; font-size: 15px; line-height: 1.2; word-break: break-word; mso-line-height-alt: 18px; margin-top: 0; margin-bottom: 0;\">&nbsp;</p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; font-size: 15px; line-height: 1.2; word-break: break-word; mso-line-height-alt: 18px; margin-top: 0; margin-bottom: 0;\"><span style=\"font-size: 15px;\">Thanks again for signing up to Zuvvii, and please tell your friends about us; we look forward to seeing you all on the platform.</span></p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t<!--[if mso]></td></tr></table><![endif]-->\r\n\t\t\t\t\t\t\t\t\t\t\t<!--[if (!mso)&(!IE)]><!-->\r\n\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t<!--<![endif]-->\r\n\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t<!--[if (mso)|(IE)]></td></tr></table><![endif]-->\r\n\t\t\t\t\t\t\t\t<!--[if (mso)|(IE)]></td></tr></table></td></tr></table><![endif]-->\r\n\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t</div>\r\n\t\t\t\t\t</div>\r\n\t\t\t\t\t<div style=\"background-color:#ffffff;\">\r\n\t\t\t\t\t\t<div class=\"block-grid \" style=\"min-width: 320px; max-width: 520px; overflow-wrap: break-word; word-wrap: break-word; word-break: break-word; Margin: 0 auto; background-color: #ffffff;\">\r\n\t\t\t\t\t\t\t<div style=\"border-collapse: collapse;display: table;width: 100%;background-color:#ffffff;\">\r\n\t\t\t\t\t\t\t\t<!--[if (mso)|(IE)]><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"background-color:#ffffff;\"><tr><td align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:520px\"><tr class=\"layout-full-width\" style=\"background-color:#ffffff\"><![endif]-->\r\n\t\t\t\t\t\t\t\t<!--[if (mso)|(IE)]><td align=\"center\" width=\"520\" style=\"background-color:#ffffff;width:520px; border-top: 0px solid transparent; border-left: 0px solid transparent; border-bottom: 0px solid transparent; border-right: 0px solid transparent;\" valign=\"top\"><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td style=\"padding-right: 0px; padding-left: 0px; padding-top:5px; padding-bottom:5px;\"><![endif]-->\r\n\t\t\t\t\t\t\t\t<div class=\"col num12\" style=\"min-width: 320px; max-width: 520px; display: table-cell; vertical-align: top; width: 520px;\">\r\n\t\t\t\t\t\t\t\t\t<div class=\"col_cont\" style=\"width:100% !important;\">\r\n\t\t\t\t\t\t\t\t\t\t<!--[if (!mso)&(!IE)]><!-->\r\n\t\t\t\t\t\t\t\t\t\t<div style=\"border-top:0px solid transparent; border-left:0px solid transparent; border-bottom:0px solid transparent; border-right:0px solid transparent; padding-top:5px; padding-bottom:5px; padding-right: 0px; padding-left: 0px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t<!--<![endif]-->\r\n\t\t\t\t\t\t\t\t\t\t\t<!--[if mso]><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td style=\"padding-right: 10px; padding-left: 10px; padding-top: 10px; padding-bottom: 10px; font-family: Arial, sans-serif\"><![endif]-->\r\n\t\t\t\t\t\t\t\t\t\t\t<div style=\"color:#393d47;font-family:Arial, Helvetica Neue, Helvetica, sans-serif;line-height:1.2;padding-top:10px;padding-right:10px;padding-bottom:10px;padding-left:10px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t<div class=\"txtTinyMce-wrapper\" style=\"font-size: 14px; line-height: 1.2; color: #393d47; font-family: Arial, Helvetica Neue, Helvetica, sans-serif; mso-line-height-alt: 17px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; font-size: 14px; line-height: 1.2; word-break: break-word; text-align: center; mso-line-height-alt: 17px; margin-top: 0; margin-bottom: 0;\"><a href=\"https://www.zuvvii.com/privacy-policy\" style=\"color: #35a594;\">Privacy Policy</a> | <a href=\"https://www.zuvvii.com/terms-of-service\" style=\"color: #35a594;\">Terms of Service</a> | <a href=\"https://www.zuvvii.com/\" style=\"color: #35a594;\">\u00A9 Zuvvii 2021</a></p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; font-size: 14px; line-height: 1.2; word-break: break-word; text-align: center; mso-line-height-alt: 17px; margin-top: 0; margin-bottom: 0;\">Atria One, 144 Morrison Street, Edinburgh, United Kingdom, EH3 8EX</p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t<!--[if mso]></td></tr></table><![endif]-->\r\n\t\t\t\t\t\t\t\t\t\t\t<!--[if (!mso)&(!IE)]><!-->\r\n\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t<!--<![endif]-->\r\n\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t<!--[if (mso)|(IE)]></td></tr></table><![endif]-->\r\n\t\t\t\t\t\t\t\t<!--[if (mso)|(IE)]></td></tr></table></td></tr></table><![endif]-->\r\n\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t</div>\r\n\t\t\t\t\t</div>\r\n\t\t\t\t\t<!--[if (mso)|(IE)]></td></tr></table><![endif]-->\r\n\t\t\t\t</td>\r\n\t\t\t</tr>\r\n\t\t</tbody>\r\n\t</table>\r\n\t<!--[if (IE)]></div><![endif]-->\r\n</body>\r\n\r\n</html>";

                    var emailTask = new Task(() =>
                    {
                        EmailService.SendEmail("Welcome to Zuvvii!", emailText, u.emailAddress);
                    });

                    emailTask.Start();
                    */
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (HandledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw LogsService.HandleException(ex, "Client error", "There was an error registering the Client", this.GetType().ToString());
            }
        }


        public Client GetClient(Guid Id)
        {
            try
            {

                Client u = ClientRepository.Get(Id);
                return u;

            }
            catch (HandledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw LogsService.HandleException(ex, "Client error", "There was an error getting the Client", this.GetType().ToString());
            }
        }
        
        public Client GetClientByCPF(string CPF)
        {
            try
            {
                return ClientRepository.GetCPF(CPF); ;

            }
            catch (HandledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw LogsService.HandleException(ex, "Client error", "There was an error getting the Client", this.GetType().ToString());
            }
        }

        public Client GetClientByCNPJ(string CNPJ)
        {
            try
            {
                return ClientRepository.GetCNPJ(CNPJ); ;

            }
            catch (HandledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw LogsService.HandleException(ex, "Client error", "There was an error getting the Client", this.GetType().ToString());
            }
        }
        public Client GetClientByEmail(string email)
        {
            try
            {
                return ClientRepository.GetEmail(email);
            }
            catch (HandledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw LogsService.HandleException(ex, "Client error", "There was an error getting the Client", this.GetType().ToString());
            }
        }

        public Client GetClientByClientname(string Clientname)
        {
            try
            {
                return ClientRepository.GetByName(Clientname);
            }
            catch (HandledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw LogsService.HandleException(ex, "Client error", "There was an error getting the Client", this.GetType().ToString());
            }
        }
        /*
        public bool UpdateClient(Client u)
        {
            try
            {
                var actualClient = ClientRepository.Get(u.Id);

                if (string.IsNullOrEmpty(u.PassHash))
                    u.PassHash = actualClient.PassHash;
                if (string.IsNullOrEmpty(u.NameCompleto)) u.NameCompleto = actualClient.NameCompleto;
                if (string.IsNullOrEmpty(u.Email)) u.Email = actualClient.Email;

                if (u.PassHash != actualClient.PassHash) u.PassHash = HashingService.GetHash(u.PassHash);
                if (uNameCompleto != actualClient.NameCompleto )
                {

                    var l = new Log();
                    l.ClientId = u.Id.ToString();
                    l.Class = LogType.UserUpdate;
                    l.TimeStamp = DateTime.Now;
                    l.Id = Guid.NewGuid();
                    l.Message = "Updated Client";
                    LogsService.HandleLog(l);
                }
                ClientRepository.Update(u);

                return true;
            }
            catch (HandledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw LogsService.HandleException(ex, "Client error", "There was an error updating the Client", this.GetType().ToString());
            }
        }

        

        public Client ResetPassword(string email, string IpAddress)
        {
            try
            {
                var result = ClientRepository.GetAll().FirstOrDefault(x => x.emailAddress == email || x.ClientName == email);
                if (result != null)
                {
                    var newPassword = StringHelper.RandomString(12, "lI");
                    var newPassHash = HashingService.GetHash(newPassword);
                    result.passHash = newPassHash;
                    ClientRepository.Update(result);
                    result.passHash = newPassword;

                    var log = new Log() { Class = LogType.PasswordReset, Id = Guid.NewGuid(), TimeStamp = DateTime.Now, ClientId = result.Id, Message = IpAddress };
                    LogsService.HandleLog(log);

                    return result;
                }

                return null;
            }
            catch (HandledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw LogsService.HandleException(ex, "Client error", "There was an error resetting the password", this.GetType().ToString());
            }
        }

        public void FollowClient(int loggedInClient, int ClientId)
        {
            try
            {
                var oldfriend = FriendshipRepository.GetAll().Where(x => x.FriendId == ClientId && x.ClientId == loggedInClient && x.StillFriends).FirstOrDefault();
                if (oldfriend != null) return;
                var f = new Friendship
                {
                    ClientId = loggedInClient,
                    FriendId = ClientId,
                    dtCreated = DateTime.Now,
                    StillFriends = true
                };

                FriendshipRepository.Add(f);

                var ClientFollowing = ClientRepository.Get(loggedInClient);
                ClientFollowing.following++;
                ClientRepository.Update(ClientFollowing);

                var ClientFollowed = ClientRepository.Get(ClientId);
                ClientFollowed.followers++;
                ClientRepository.Update(ClientFollowed);

                //if followed does not follow following Client, add "Follow them back!"
                var mutualFriendship = FriendshipRepository.GetAll().FirstOrDefault(f =>
                    f.ClientId == ClientId && f.FriendId == loggedInClient && f.StillFriends == true);

                bool mutualFriendshipExists = mutualFriendship != null;

                Notification notification = new Notification()
                {
                    TimeStamp = DateTime.UtcNow,
                    ItemThumb = null,
                    SenderClientImagePath = ClientFollowing.profilePicPath,
                    ReceiverClientId = ClientFollowed.Id,
                    SenderClientId = ClientFollowing.Id,
                    SenderClientName = ClientFollowing.ClientName,
                    ItemId = null,
                    Deleted = false,
                    Read = false,
                    Type = NotificationType.Follow,
                    Description = "Started following you on Zuvvii." + (!mutualFriendshipExists ? " Follow them back!" : ""),
                    ImagePath = null
                };
                NotificationService.CreateNotification(notification);
            }
            catch (HandledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw LogsService.HandleException(ex, "Client error", "There was an error following the Client", this.GetType().ToString());
            }
        }
        public void UnFollowClient(int loggedInClient, int ClientId)
        {
            try
            {
                var friendship = FriendshipRepository.GetAll().FirstOrDefault(x => x.FriendId == ClientId && x.ClientId == loggedInClient && x.StillFriends);
                if (friendship == null) return;

                FriendshipRepository.Remove(friendship.FriendshipId);
                var ClientFollowing = ClientRepository.Get(loggedInClient);
                ClientFollowing.following--;
                ClientRepository.Update(ClientFollowing);

                var ClientFollowed = ClientRepository.Get(ClientId);
                ClientFollowed.followers--;
                ClientRepository.Update(ClientFollowed);
            }
            catch (HandledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw LogsService.HandleException(ex, "Client error", "There was an error unfollowing the Client", this.GetType().ToString());
            }
        }

        public IEnumerable<Friendship> GetFriends(int loggedInClient)
        {
            try
            {
                var list = FriendshipRepository.GetAll().Where(x => x.ClientId == loggedInClient && x.StillFriends);
                return list.AsEnumerable();
            }
            catch (HandledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw LogsService.HandleException(ex, "Client error", "There was an error getting the friends", this.GetType().ToString());
            }
        }
        public int UpdateClipCount(int ClientId, int countToAdd)
        {
            try
            {
                var u = ClientRepository.GetAll().FirstOrDefault(x => x.Id == ClientId);
                u.clipCount += countToAdd;

                string password = ClientRepository.GetAll().FirstOrDefault(x =>
                    (x.emailAddress == u.emailAddress || x.ClientName == u.emailAddress)).passHash;
                if (string.IsNullOrEmpty(u.passHash))
                    u.passHash = password;
                ClientRepository.Update(u);
                return u.clipCount;
            }
            catch (HandledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw LogsService.HandleException(ex, "Client error", "There was an error updating the clip count", this.GetType().ToString());
            }
        }

        public IEnumerable<Client> SearchClients(string searchTerms)
        {
            try
            {
                var Clients = ClientRepository.GetAll().Where(x => x.ClientName.Contains(searchTerms));
                return Clients;
            }
            catch (HandledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw LogsService.HandleException(ex, "Client error", "There was an error searching Clients", this.GetType().ToString());
            }
        }

        public IEnumerable<string> SearchClientsForTag(string searchTerms)
        {
            try
            {
                var Clients = ClientRepository.GetAll().Where(x => x.ClientName.StartsWith(searchTerms)).Select(Client => Client.ClientName).Take(5);
                return Clients;
            }
            catch (HandledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw LogsService.HandleException(ex, "Client error", "There was an error searching Clients", this.GetType().ToString());
            }
        }
        public bool BlockClient(BlockedClient u)
        {
            try
            {
                var uOld = BlockedClientRepository.GetAll().Where(x => x.ClientId == u.ClientId && x.BlockedClientId == u.BlockedClientId).FirstOrDefault();
                if (uOld == null) BlockedClientRepository.Add(u);
                return true;
            }
            catch (Exception Ex)
            { return false; }
        }
        public bool UnBlockClient(BlockedClient u)
        {

            try
            {
                var uOld = BlockedClientRepository.GetAll().Where(x => x.ClientId == u.ClientId && x.BlockedClientId == u.BlockedClientId).FirstOrDefault();
                if (uOld != null) BlockedClientRepository.Remove(uOld.Id);
                return true;
            }
            catch { return false; }
        }

        public IEnumerable<BlockedClient> GetBlockedClients(int ClientId)
        {
            return BlockedClientRepository.GetAll().Where(x => x.ClientId == ClientId).ToList();
        }
        */
    }
}
